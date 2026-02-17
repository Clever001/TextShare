import { EditorContent, type Editor } from "@tiptap/react";
import type { FormattingOption } from "../../Features/TextEditor/TextEditorFeature";

type Props = {
  editor: Editor,
  changeFormatting: (opt: FormattingOption) => void
}

export function EditorWidget ({editor, changeFormatting}: Props) {
  return <>
    <div className='editor-buttons'>
      <button onClick={() => changeFormatting({ formattingType: 'bold' })}>Жирный</button>
      <button onClick={() => changeFormatting({ formattingType: 'underlined' })}>Подчеркнутый</button>
      <button onClick={() => changeFormatting({ formattingType: 'italic' })}>Курсив</button>
      <button onClick={() => changeFormatting({ formattingType: 'strike' })}>Зачеркнуть</button>

      <br />

      <p>Сделать заголовок</p>
      <button onClick={() => changeFormatting({ formattingType: 'header', headerLevel: 1 })}>Заголовок 1 уровня</button>
      <button onClick={() => changeFormatting({ formattingType: 'header', headerLevel: 2 })}>Заголовок 2 уровня</button>
      <button onClick={() => changeFormatting({ formattingType: 'header', headerLevel: 3 })}>Заголовок 3 уровня</button>
      <button onClick={() => changeFormatting({ formattingType: 'header', headerLevel: 4 })}>Заголовок 4 уровня</button>
      <button onClick={() => changeFormatting({ formattingType: 'header', headerLevel: 5 })}>Заголовок 5 уровня</button>
      <button onClick={() => changeFormatting({ formattingType: 'header', headerLevel: 6 })}>Заголовок 6 уровня</button>

      <br />

      <button onClick={() => changeFormatting({ formattingType: 'code' })}>Листинг</button>
      <button onClick={() => changeFormatting({ formattingType: 'quote' })}>Цитата</button>
      <button onClick={() => changeFormatting({ formattingType: 'image' })}>Вставить картинку</button>
    </div>

    <EditorContent editor={editor} />
  </>
};