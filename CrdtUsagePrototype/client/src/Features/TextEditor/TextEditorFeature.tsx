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
import Heading, { type Level } from '@tiptap/extension-heading'
import { EditorContent, useEditor } from '@tiptap/react'
import Collaboration from '@tiptap/extension-collaboration'
import CollaborationCursor from '@tiptap/extension-collaboration-caret'
import { useCallback, useEffect, useState } from 'react'
import { WebsocketProvider } from 'y-websocket'
import * as Y from 'yjs'
import './TextEditorFeature.css';

type User = {
  name: string,
  color: string
};

type FormattingOption = {
  formattingType: 'bold' | 'italic' | 'underlined' | 'strike' | 'header' | 'code' | 'quote' | 'image',
  headerLevel?: Level,
}

const doc = new Y.Doc()
const provider = new WebsocketProvider('http://192.168.0.227:1234/ws', 'my-roomname', doc);

const possibleNames = ["Sasha", "Dasha", "Kostya", "Pasha", "Yulia", "Anastasia", "Dmitriy", "Igor", "Egor"];
const possibleColors = ["ff5252", "4caf50", "2196f3", "ff9800", "9c27b0", "673ab7", "ffeb3b", "795548", "00bcd4", "e91e63"];

const user: User = {
  name: randomValue(possibleNames),
  color: randomValue(possibleColors),
};

function randomValue<T>(arr: T[]) {
  return arr[Math.floor(Math.random() * arr.length)];
}


const TextEditorFeature = () => {
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
      Collaboration.configure({
        document: doc,
      }),
      CollaborationCursor.configure({
        provider: provider,
        user: {
          name: user.name,
          color: user.color
        },
      })
    ],
  }, []);

  const changeFormatting = useCallback<(opt: FormattingOption) => void>((opt: FormattingOption) => {
    var chain = editor.chain().focus();
    switch (opt.formattingType) {
      case 'bold': chain = chain.toggleBold(); break;
      case 'italic': chain = chain.toggleItalic(); break;
      case 'underlined': chain = chain.toggleUnderline(); break;
      case 'strike': chain = chain.toggleStrike(); break;
      case 'code': chain = chain.toggleCode(); break;
      case 'quote': chain = chain.toggleBlockquote(); break;
      case 'image':
        const url: string | null = prompt('Введите URL картинки:');
        if (!url) {
          alert("Url введен неправильно");
          return;
        }
        chain = chain.setImage({src: url});
        break;
      case 'header':
        if (!opt.headerLevel) {
          alert("Уровень заголовка не указан");
          return;
        }
        chain = chain.toggleHeading({level: opt.headerLevel});
        break;
    }
    chain.run();
  }, []);


  return <>
    <div className='editor-buttons'>
      <button onClick={() => changeFormatting({formattingType: 'bold'})}>Жирный</button>
      <button onClick={() => changeFormatting({formattingType: 'underlined'})}>Подчеркнутый</button>
      <button onClick={() => changeFormatting({formattingType: 'italic'})}>Курсив</button>
      <button onClick={() => changeFormatting({formattingType: 'strike'})}>Зачеркнуть</button>
      
      <br />

      <p>Сделать заголовок</p>
      <button onClick={() => changeFormatting({formattingType: 'header', headerLevel: 1})}>Заголовок 1 уровня</button>
      <button onClick={() => changeFormatting({formattingType: 'header', headerLevel: 2})}>Заголовок 2 уровня</button>
      <button onClick={() => changeFormatting({formattingType: 'header', headerLevel: 3})}>Заголовок 3 уровня</button>
      <button onClick={() => changeFormatting({formattingType: 'header', headerLevel: 4})}>Заголовок 4 уровня</button>
      <button onClick={() => changeFormatting({formattingType: 'header', headerLevel: 5})}>Заголовок 5 уровня</button>
      <button onClick={() => changeFormatting({formattingType: 'header', headerLevel: 6})}>Заголовок 6 уровня</button>

      <br />

      <button onClick={() => changeFormatting({formattingType: 'code'})}>Листинг</button>
      <button onClick={() => changeFormatting({formattingType: 'quote'})}>Цитата</button>
      <button onClick={() => changeFormatting({formattingType: 'image'})}>Вставить картинку</button>
    </div>
    <EditorContent editor={editor} />
  </>
};

export default TextEditorFeature;
