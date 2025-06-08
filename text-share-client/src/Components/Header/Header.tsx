import React, { useEffect, useRef } from 'react'
import { Link } from 'react-router-dom'
import './Header.css'

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

const Header = ({
  userName, query, menuHidden, onSearchQueryChange,
  onSearchSubmit, onUserClick, onShowMenuButtonClick,
  onLogOutClick, onProfileClick, menuButtonRef,
  menuRef, closeMenu
}: Props) => {

  return (
    <header>
      <div className="my-container">
        <div className="logo">
          <Link to="/">
            <img src="/img/logo.svg" alt="logo"></img>
            <div>TextShare</div>
          </Link>
        </div>
        <div className="add-button">
          <Link to="/createText">
            <img src="/img/add_icon.svg" alt="add" />
            <div>Добавить текст</div>
          </Link>
        </div>
        <div className="search-text">
          <form onSubmit={onSearchSubmit} className="search-bar">
            <input type="text" placeholder="Поиск" value={query} onChange={onSearchQueryChange} className="search-input" />
            <button type="submit" className="search-button">
              <img src="/img/search_icon_black.svg" alt="" />
            </button>
          </form>
        </div>
        <div className="user-info">
          {userName ?
            <button onClick={onProfileClick}>
              <img src="/img/user_icon.svg" alt="user" />
              <div className="user-name">{userName}</div>
            </button>
            :
            <Link to="/auth">
              <img src="/img/log_in_icon.svg" alt="log in" />
              <div className="user-name">Авторизоваться</div>
            </Link>
          }
          <button type="button" className="menu-button" onClick={onShowMenuButtonClick} ref={menuButtonRef}>
            <img src="/img/dropout_menu.svg" alt="menu" />
          </button>
        </div>
        <div className="dropout-menu" hidden={menuHidden} ref={menuRef}>
          {userName &&
            <div>
              <button type="button" onClick={(e) => {onProfileClick(e); closeMenu();}}>
                <img src="/img/user_simple_icon.svg" alt="" />
                <div className="text">Мой профиль</div>
              </button>
            </div>
          }
          {userName &&
            <div>
              <Link to="/profileContentChange" onClick={(e) => closeMenu()}>
                <img src="/img/edit_icon.svg" alt="" />
                <div className="text">Редактировать профиль</div>
              </Link>
            </div>
          }
          <div>
            <Link to="/" onClick={(e) => closeMenu()}>
              <img src="/img/search_icon.svg" alt=""/>
              <div className="text">Поиск текстов</div>
            </Link>
          </div>
          <div>
            <Link to="/userSearch" onClick={(e) => closeMenu()}>
              <img src="/img/users_icon.svg" alt="" />
              <div className="text">Поиск пользователей</div>
            </Link>
          </div>
          {userName &&
            <div>
              <button type="button" onClick={(e) => {onLogOutClick(e); closeMenu()}}>
                <img src="/img/log_out_icon.svg" alt="" />
                <div className="text">Выйти из системы</div>
              </button>
            </div>
          }
        </div>
      </div>
    </header>
  )
}

export default Header