import Document from '@tiptap/extension-document'
import Paragraph from '@tiptap/extension-paragraph'
import Text from '@tiptap/extension-text'
import Bold from '@tiptap/extension-bold'
import Italic from '@tiptap/extension-italic'
import Underlined from '@tiptap/extension-underline'
import Strike from '@tiptap/extension-strike'
import Code from '@tiptap/extension-code'
import Blockquote from '@tiptap/extension-blockquote'
import Image from '@tiptap/extension-image'
import Heading from '@tiptap/extension-heading'
import { useEditor } from '@tiptap/react'
import Collaboration from '@tiptap/extension-collaboration'
import CollaborationCursor from '@tiptap/extension-collaboration-caret'
import { useCallback, useContext, useEffect, useRef, useState, type MouseEvent } from 'react'
import * as Y from 'yjs'
import { EditorWidget, type FormattingOption } from '../../4-Widgets/EditorWidget/EditorWidget'
import { WebsocketProvider } from 'y-websocket';
import './DocumentFeature.css';
import { UserContext } from '../../1-Processes/UserContext'


type Props = {
  toggleDocumentVersionsBlock: () => void;
  onCreateNewVersion: ((versionName: string) => void);
}

export default function DocumentFeature({ toggleDocumentVersionsBlock, onCreateNewVersion }: Props) {
  const userContext = useContext(UserContext);
  const [yjsApp, setYjsApp] = useState<{doc: Y.Doc, provider: WebsocketProvider} | null>(null);

  useEffect(() => {
    const doc = new Y.Doc();
    const provider = new WebsocketProvider(
      import.meta.env.VITE_YJS_WS_URL,
      'document',
      doc
    );

    setYjsApp({ doc, provider });

    return () => {
      provider.destroy();
      doc.destroy();
      setYjsApp(null);
    };
  }, []);

  const editor = useEditor({
    extensions: [
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
      ...(yjsApp ? [
        Collaboration.configure({
          document: yjsApp.doc,
        }),
        CollaborationCursor.configure({
          provider: yjsApp.provider,
          user: {
            name: userContext.getUser()?.name ?? "Unknown",
            color: userContext.getUser()?.cursorColor ?? "#4caf50"
          },
        }),
      ] : []),
    ],
  }, [yjsApp]);

  const changeFormatting = useCallback<(opt: FormattingOption) => void>((opt: FormattingOption) => {
    try {
      var chain = editor.chain().focus();
      console.log("Execute change formatting.")
      switch (opt.formattingType) {
        case 'bold':
          chain = chain.toggleBold();
          console.log("make bold");
          break;
        case 'italic':
          chain = chain.toggleItalic();
          break;
        case 'underlined':
          chain = chain.toggleUnderline();
          break;
        case 'strike':
          chain = chain.toggleStrike();
          break;
        case 'image':
          const url: string | null = prompt('Введите URL картинки:');
          if (!url) {
            alert("Url введен неправильно");
            return;
          }
          chain = chain.setImage({ src: url });
          break;
        case 'quote':
          chain = chain.toggleBlockquote();
          break;
        case 'code':
          chain = chain.toggleCode();
          break;
        case 'header':
          if (!opt.headerLevel) {
            alert("Уровень заголовка не указан");
            return;
          }
          if (editor.isActive('heading', { level: 1 })) {
            chain = chain.setParagraph();
          } else {
            chain = chain.setHeading({ level: opt.headerLevel });
          }
          break;
      }
      chain.run();
      console.log("chain run");
    } catch (error) {
      console.log(error);
      throw error;
    }
  }, [editor]);

  const addImage = useCallback((event: MouseEvent<HTMLButtonElement, globalThis.MouseEvent>) => {
    event.preventDefault();
  }, []);

  return (
    <EditorWidget
      editor={editor}
      changeFormatting={changeFormatting}
      addImage={addImage}
      isEditing={true}
      toggleDocumentVersionsBlock={toggleDocumentVersionsBlock}
      onCreateNewVersion={onCreateNewVersion} />
  )
};
