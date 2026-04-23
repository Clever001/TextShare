import { useState } from "react";
import "./EditorPanel.css";

type Props = {};

export default function EditorPanel() {
  const [showDropdown, setShowDropdown] = useState<boolean>(false);

  const switchShowDropdown = () => {
    setShowDropdown((pr) => !pr);
  };

  return (
    <div className="editor-panel">
      <div className="edit-btns">
        <button className="act-btn">
          <strong>Ж</strong>
        </button>
        <button className="act-btn">
          <em>К</em>
        </button>
        <button className="act-btn">
          <u>П</u>
        </button>
        <button className="act-btn">
          <s>З</s>
        </button>
        <div className="dropdown-container">
          <button
            className="dropdown-button act-btn"
            onClick={() => {
              switchShowDropdown();
            }}
          >
            <span>Заголовки</span>
            <img src="/img/arrow_down_black.svg" alt="arrow_down" />
          </button>
          {showDropdown && (
            <div
              className="dropdown-menu"
              onMouseLeave={() => {
                switchShowDropdown();
              }}
            >
              <button>Заголовок 1</button>
              <button>Заголовок 2</button>
              <button>Заголовок 3</button>
              <button>Заголовок 4</button>
              <button>Заголовок 5</button>
              <button>Заголовок 6</button>
            </div>
          )}
        </div>
        <button className="act-btn">{"</>"}</button>
        <button className="act-btn">
          <img src="/img/quote.svg" alt={'"'} />
        </button>
        <button className="act-btn">
          <img src="/img/image.svg" alt="img" />
        </button>
      </div>
      <form className="version-create">
        <input type="text" placeholder="Название версии" />
        <button type="submit" className="act-btn">
          <img src="/img/version.svg" alt="version" />
        </button>
      </form>
    </div>
  );
}
