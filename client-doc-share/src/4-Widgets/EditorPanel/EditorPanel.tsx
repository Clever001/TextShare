import { useState } from "react"
import "./EditorPanel.css"

type Props = {

}

export default function EditorPanel() {
  const [showDropdown, setShowDropdown] = useState<boolean>(false);

  return <div className="editor-panel">
    <div className="edit-btns">
      <button><strong>Ж</strong></button>
      <button><em>К</em></button>
      <button><u>П</u></button>
      <button><s>З</s></button>
      <div className="dropdown-container">
        <button className="dropdown-button">
          Заголовки
        </button>
        {showDropdown &&
          <div className="dropdown-menu">
            <button>Заголовок 1</button>
            <button>Заголовок 2</button>
            <button>Заголовок 3</button>
            <button>Заголовок 4</button>
            <button>Заголовок 5</button>
            <button>Заголовок 6</button>
          </div>
        }
      </div>
      <button>{"</>"}</button>
      <button><img src="/img/quote.svg" alt={"\""} /></button>
      <button><img src="/img/image.svg" alt="img" /></button>
    </div>
    <form className="version-create">
      <input type="text" placeholder="Название версии" />
      <button type="submit">
        <img src="/img/version.svg" alt="version" />
      </button>
    </form>
  </div>
}