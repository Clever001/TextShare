import { EditorContent, type Editor } from "@tiptap/react";
import type { Level } from '@tiptap/extension-heading'
import "./EditorWidget.css";
import { useRef, useState } from "react";

type Props = {
  editor: Editor,
  changeFormatting: (opt: FormattingOption) => void,
  addImage: React.MouseEventHandler<HTMLButtonElement>,
  isEditing: boolean,
  toggleDocumentVersionsBlock: () => void;
  onCreateNewVersion: ((versionName: string) => void) | null;
}

export type FormattingOption = {
  formattingType: 'bold' | 'italic' | 'underlined' | 'strike' | 'header' | 'code' | 'quote' | 'image',
  headerLevel?: Level,
}

export function EditorWidget ({editor, changeFormatting, addImage, isEditing, toggleDocumentVersionsBlock, onCreateNewVersion}: Props) {
  const newVersionNameRef = useRef<HTMLInputElement | null>(null);
  const [isDropdownOpen, setIsDropdownOpen] = useState<boolean>(false);

  const toggleIsDropdownOpen = () : void => {
    console.log("toggle dropdown. CurrentValue: " + !isDropdownOpen);
    setIsDropdownOpen(!isDropdownOpen);
  };

  const onMouseLeave = () : void => {
    setIsDropdownOpen(false);
  }

  const createNewVersion = (event: React.SubmitEvent<Element>): void => {
    event.preventDefault();
    if (onCreateNewVersion && newVersionNameRef.current) {
      const newVersionName = newVersionNameRef.current.value;
      onCreateNewVersion(newVersionName);
      alert("Новая версия создана!");
    }
  }

  return (
    <div className="document-page">
      {isEditing &&
        <button 
          className="switch-button to-versions"
          onClick={() => toggleDocumentVersionsBlock()} >
          Посмотреть список версий
        </button>
      }

      {!isEditing &&
        <button 
          className="switch-button to-editing"
          onClick={() => toggleDocumentVersionsBlock()} >
          Перейти к редактированию
        </button>
      }

      {isEditing && 
        <div className="toolbar">
          <button
            onClick={() => changeFormatting({formattingType: 'bold'})}
            title="Жирный"
          >
            <strong>B</strong>
          </button>
          <button
            onClick={() => changeFormatting({formattingType: 'italic'})}
            title="Курсив"
          >
            <em>I</em>
          </button>
          <button
            onClick={() => changeFormatting({formattingType: 'underlined'})}
            title="Подчеркнутый"
          >
            <u>U</u>
          </button>
          <button
            onClick={() => changeFormatting({formattingType: 'strike'})}
            title="Зачеркнутый"
          >
            <s>S</s>
          </button>

          <div className="dropdown-container">
            <button 
              className="dropdown-button"
              onClick={() => toggleIsDropdownOpen()}
              type="button"
            >
              <span>{"Заголовки"}</span>
            </button>

            {isDropdownOpen &&
              <div className="dropdown-menu" onMouseLeave={() => onMouseLeave()}>
                <button onClick={() => changeFormatting({formattingType: 'header', headerLevel: 1})}>Заголовок 1</button>
                <button onClick={() => changeFormatting({formattingType: 'header', headerLevel: 2})}>Заголовок 2</button>
                <button onClick={() => changeFormatting({formattingType: 'header', headerLevel: 3})}>Заголовок 3</button>
                <button onClick={() => changeFormatting({formattingType: 'header', headerLevel: 4})}>Заголовок 4</button>
                <button onClick={() => changeFormatting({formattingType: 'header', headerLevel: 5})}>Заголовок 5</button>
                <button onClick={() => changeFormatting({formattingType: 'header', headerLevel: 6})}>Заголовок 6</button>
              </div>
            }
          </div>

          <button title="Код" onClick={() => changeFormatting({formattingType: 'code'})}>
            &lt;/&gt;
          </button>

          <button title="Цитата" onClick={() => changeFormatting({formattingType: 'quote'})}>
            ❝
          </button>

          
          <button title="Вставить изображение" onClick={addImage}>
            🖼️
          </button>

          {isEditing && 
            <form className="new-version-container" onSubmit={createNewVersion} >
              <input type="text" ref={newVersionNameRef} required />
              <button title="Создать новую версию" type="submit" >
                Создать новую версию
              </button>
            </form>
          }

        </div>
      }


      <div className="editor-container">
        <EditorContent editor={editor} />
      </div>
    </div>
  )
};