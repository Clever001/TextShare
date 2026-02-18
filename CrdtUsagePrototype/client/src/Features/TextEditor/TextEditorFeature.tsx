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
import { useCallback, useEffect, useRef, useState } from 'react'
import * as Y from 'yjs'
import './TextEditorFeature.css';
import { EditorWidget } from '../../Widgets/EditorWidget/EditorWidget'
import { MessagesWidget } from '../../Widgets/MessagesWidget/MessagesWidget'
import { WebsocketProvider } from 'y-websocket';


type User = {
  name: string,
  color: string
};

export type FormattingOption = {
  formattingType: 'bold' | 'italic' | 'underlined' | 'strike' | 'header' | 'code' | 'quote' | 'image',
  headerLevel?: Level,
}

const doc = new Y.Doc()
const yjsProvider = new WebsocketProvider(import.meta.env.VITE_YJS_WS_URL as string, 'document', doc);
const customWebSocket = new WebSocket(import.meta.env.VITE_CUSTOM_WS_URL as string);

const possibleNames = ["Sasha", "Dasha", "Kostya", "Pasha", "Yulia", "Anastasia", "Dmitriy", "Igor", "Egor"];
const possibleColors = ["#ff5252", "#4caf50", "#2196f3", "#ff9800", "#9c27b0", "#673ab7", "#ffeb3b", "#795548", "#00bcd4", "#e91e63"];

const user: User = {
  name: randomValue(possibleNames),
  color: randomValue(possibleColors),
};

function randomValue<T>(arr: T[]) {
  return arr[Math.floor(Math.random() * arr.length)];
}


export default function TextEditorFeature() {
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
        provider: yjsProvider,
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
        chain = chain.setImage({ src: url });
        break;
      case 'header':
        if (!opt.headerLevel) {
          alert("Уровень заголовка не указан");
          return;
        }
        chain = chain.toggleHeading({ level: opt.headerLevel });
        break;
    }
    chain.run();
  }, []);

  const [messages, setMessages] = useState<string[]>([]);
  const messageInputRef = useRef<HTMLInputElement | null>(null);
  const onSendMessage = useCallback<() => void>(() => {
    if (!messageInputRef.current || !customWebSocket) {
      return;
    }

    const message = messageInputRef.current.value;
    const json = JSON.stringify({
      type: 0,
      content: message
    });

    customWebSocket.send(json);
    addMessage(message);
  }, []);

  useEffect(() => {
    if (!customWebSocket) return;

    const handleNewMessage = (ev: MessageEvent) => {
      const rawMessage = ev.data;

      if (isJson(rawMessage)) {
        const parsedMessage = JSON.parse(rawMessage);
        if (typeof parsedMessage.content === 'string') {
          addMessage(parsedMessage.content);
          return;
        } else {
          console.log("Unknown message 1");
        }
      } else {
        console.log("Unknown message 2");
      }
    };

    customWebSocket.addEventListener('message', handleNewMessage);

    // Очистка при размонтировании
    return () => {
      customWebSocket?.removeEventListener('message', handleNewMessage);
    };
  }, [customWebSocket]);


  function isJson(data: any): boolean {
    if (typeof (data) === 'string') {
      try {
        const dto: any = JSON.parse(data);
        return true;
      } catch (e) {
        console.log("Not valid json.");
        return false;
      }
    }
    console.log("Not string: " + data);
    return false;
  }

  function addMessage(message: string) {
    console.log(`новое сообщение: ${message}.`);
    setMessages(prevMessages => [...prevMessages, message]);
  }


  return <>
    <EditorWidget editor={editor} changeFormatting={changeFormatting} />

    <MessagesWidget messageInputRef={messageInputRef} onSendMessage={onSendMessage} messages={messages} />
  </>
};
