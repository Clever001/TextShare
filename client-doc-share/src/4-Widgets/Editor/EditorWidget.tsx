import { EditorContent, useEditor, type Editor } from "@tiptap/react";
import Document from "@tiptap/extension-document";
import Paragraph from "@tiptap/extension-paragraph";
import Text from "@tiptap/extension-text";
import Bold from "@tiptap/extension-bold";
import Italic from "@tiptap/extension-italic";
import Underlined from "@tiptap/extension-underline";
import Strike from "@tiptap/extension-strike";
import Code from "@tiptap/extension-code";
import Blockquote from "@tiptap/extension-blockquote";
import Image from "@tiptap/extension-image";
import Heading from "@tiptap/extension-heading";
import "./EditorWidget.css";

type Props = {};

export default function EditorWidget(props: Props) {
  const editor = useEditor(
    {
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
      ],
    },
    [],
  );

  return (
    <div className="editor-container">
      <EditorContent editor={editor} />
    </div>
  );
}
