import React, { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import './Header.css';
import CustomButton from '../CustomButton/CustomButton';

type Props = {
  userName: string,
  query: string,
  menuHidden: boolean,
  onSearchQueryChange: (e: React.ChangeEvent<HTMLInputElement>) => void,
  onSearchSubmit: (e: React.FormEvent<HTMLFormElement>) => void,
  onUserClick: (e: React.FormEvent<HTMLFormElement>) => void,
  onShowMenuButtonClick: (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => void,
  onLogOutClick: (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => void,
  onProfileClick: (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => void,
  menuButtonRef: React.RefObject<HTMLButtonElement | null>,
  menuRef: React.RefObject<HTMLDivElement | null>,
  closeMenu: () => void
}

export default function Header() {
  const [dropoutHidden, setDropoutHidden] = useState<boolean>(true);

  return (
    <header className="header">
      <div className="my-container">
        <div className="logo">
          <Link to="/">
            <img src="/img/logo.svg" alt="logo"></img>
            <div>DocShare</div>
          </Link>
        </div>
        <div className="add-button">
          {/* <Link to="/createText">
            <img src="/img/add_circle.svg" alt="add" />
            <div>Добавить документ</div>
          </Link> */}
          <CustomButton 
            target="add" 
            rightIconUrl={null} 
            text="Добавить документ"
            color="green"
            />
        </div>
        <div className="search-text">
          <form onSubmit={() => { }} className="search-bar">
            <input type="text" placeholder="поиск" onChange={() => { }} className="search-input" />
            <button type="submit" className="search-button">
              <img src="/img/search.svg" alt="" />
            </button>
          </form>
        </div>
        <div className="user-info">
          {true ?
            <button onClick={() => { }}>
              <img src="/img/user.svg" alt="user" />
              <div className="user-name">UserName</div>
            </button>
            :
            <Link to="/auth">
              <img src="/img/log_in_icon.svg" alt="log in" />
              <div className="user-name">Авторизоваться</div>
            </Link>
          }
          <button type="button" className="menu-button" onClick={() => setDropoutHidden(!dropoutHidden)}>
            <img src="/img/arrow_down.svg" alt="menu" />
          </button>
        </div>
        {!dropoutHidden &&
          <div className="dropout-menu" >
            {true &&
              <div>
                <button type="button" onClick={() => { }}>
                  <img src="/img/plain_user.svg" alt="" />
                  <div className="menu-value">Мой профиль</div>
                </button>
              </div>
            }
            {true &&
              <div>
                <Link to="/profileContentChange" onClick={() => { }}>
                  <img src="/img/edit_white.svg" alt="" />
                  <div className="menu-value">Редактировать профиль</div>
                </Link>
              </div>
            }
            <div>
              <Link to="/" onClick={() => { }}>
                <img src="/img/search_white.svg" alt="" />
                <div className="menu-value">Поиск текстов</div>
              </Link>
            </div>
            <div>
              <Link to="/userSearch" onClick={() => { }}>
                <img src="/img/users.svg" alt="" />
                <div className="menu-value">Поиск пользователей</div>
              </Link>
            </div>
            {true &&
              <div>
                <button type="button" onClick={() => { }}>
                  <img src="/img/log_out_icon.svg" alt="" />
                  <div className="menu-value">Выйти из системы</div>
                </button>
              </div>
            }
          </div>
        }
      </div>
    </header>
  );
};
