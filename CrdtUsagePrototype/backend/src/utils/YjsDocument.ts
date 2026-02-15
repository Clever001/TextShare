import * as Y from 'yjs';
import { Document as DocumentDto } from "./Dtos";
import { Mutex } from 'async-mutex';
import Document from '@tiptap/extension-document';
import Paragraph from '@tiptap/extension-paragraph';
import Text from '@tiptap/extension-text';
import Bold from '@tiptap/extension-bold';
import Italic from '@tiptap/extension-italic';
import Underlined from '@tiptap/extension-underline';
import Strike from '@tiptap/extension-strike';
import Code from '@tiptap/extension-code';
import Blockquote from '@tiptap/extension-blockquote';
import Image from '@tiptap/extension-image';
import Heading from '@tiptap/extension-heading';
import { Editor } from '@tiptap/core';
import Collaboration from '@tiptap/extension-collaboration';

import { JSDOM } from 'jsdom'
const dom = new JSDOM('<!DOCTYPE html><html><body></body></html>');
(global as any).window = dom.window;
(global as any).document = dom.window.document;
(global as any).navigator = dom.window.navigator;

export class YjsDocument {
  private readonly doc: Y.Doc;
  private static readonly serverExtensions = [
    Document,
    Paragraph,
    Text,
    Bold,
    Italic,
    Underlined,
    Strike,
    Heading.configure({ levels: [1, 2, 3, 4, 5, 6] }), 
    Code,
    Blockquote,
    Image,
  ];

  private readonly mutex: Mutex;

  private constructor(doc: Y.Doc) {
    this.doc = doc;
    this.mutex = new Mutex();
  }

  public createEmptyDocument(): YjsDocument {
    return new YjsDocument(new Y.Doc());
  }

  public createDocumentFromDocumentDto(documentDto: DocumentDto): YjsDocument {
    const ydoc = YjsDocument.htmlToYDoc(documentDto.htmlText);
    return new YjsDocument(ydoc);
  }

  public async updateDocument(update: Uint8Array): Promise<void> {
    const release = await this.mutex.acquire();
    try {
      Y.applyUpdate(this.doc, update);
    } finally {
      await release();
    }
  }

  public async getDocument(): Promise<DocumentDto> {
    const release = await this.mutex.acquire();
    try {
      return {
        htmlText: YjsDocument.yDocToHtml(this.doc),
      };
    } finally {
      await release();
    }
  }

  private static htmlToYDoc(html: string): Y.Doc {
    // TODO: Need optimization.
    const ydoc = new Y.Doc();
    const editor = new Editor({
      extensions: [
        ...YjsDocument.serverExtensions,
        Collaboration.configure({
          document: ydoc,
        }),
      ],
      content: html,
    });
    
    editor.destroy();
    
    return ydoc;
  }

  private static yDocToHtml(ydoc: Y.Doc): string {
    // TODO: Need optimization.
    const editor = new Editor({
      extensions: [
        ...YjsDocument.serverExtensions,
        Collaboration.configure({
          document: ydoc,
        }),
      ],
    });

    const html = editor.getHTML();
    
    editor.destroy();
    return html;
  }

  public getUpdate(): Uint8Array {
    return Y.encodeStateAsUpdate(this.doc);
  }
}