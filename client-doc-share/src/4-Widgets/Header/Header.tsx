import React, { useCallback, useContext, useEffect, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import "./Header.css";
import CustomButton from "../CustomButton/CustomButton";
import { AuthContext } from "../../1-Processes/AuthContext";

type Props = {
  userName: string;
  query: string;
  menuHidden: boolean;
  onSearchQueryChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
  onSearchSubmit: (e: React.FormEvent<HTMLFormElement>) => void;
  onUserClick: (e: React.FormEvent<HTMLFormElement>) => void;
  onShowMenuButtonClick: (
    e: React.MouseEvent<HTMLButtonElement, MouseEvent>,
  ) => void;
  onLogOutClick: (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => void;
  onProfileClick: (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => void;
  menuButtonRef: React.RefObject<HTMLButtonElement | null>;
  menuRef: React.RefObject<HTMLDivElement | null>;
  closeMenu: () => void;
};

export default function Header() {
  const navigate = useNavigate()
  const [dropoutHidden, setDropoutHidden] = useState<boolean>(true);

  const switchDropout = () => {
    setDropoutHidden((pr) => !pr);
  };

  const authContext = useContext(AuthContext)
  if (!authContext) {
    throw new Error("Auth Context cannot be null")
  }

  const isAuthenticated = authContext.isAuthenticated
  const getUserInfo = authContext.getUserInfo
  const setUserInfo = authContext.setUserInfo
  const user = authContext.user
  
  const [userName, setUserName] = useState<string>("")
  useEffect(() => {
    const userInfo = getUserInfo()
    if (userInfo) {
      setUserName(userInfo.userName)
    } else {
      setUserName("")
    }
  }, [isAuthenticated, user])

  const redirectToUserProfile = useCallback(() => {
    if (userName !== "") {
      navigate("/profile/" + encodeURIComponent(userName))
    }
    setDropoutHidden(true)
  }, [userName])

  const logout = useCallback(() => {
    setUserInfo(null)
    setDropoutHidden(true)
  }, [])

  const onSearchSubmit = useCallback((e: React.SubmitEvent<HTMLFormElement>) => {
    e.preventDefault()

    const formData = new FormData(e.currentTarget)
    const title = formData.get("title")?.toString() ?? ""

    navigate(`/?title=${encodeURIComponent(title)}`)
  }, [])

  return (
    <header className="header">
      <div className="my-container">
        <div className="logo">
          <Link to="/">
            <img src="/img/logo.svg" alt="logo"></img>
            <div style={{ color: "var(--background)" }}>DocShare</div>
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
            onClick={() => {navigate("create")}}
          />
        </div>
        <div className="search-text">
          <form onSubmit={onSearchSubmit} className="search-bar">
            <input
              type="text"
              placeholder="поиск"
              className="search-input"
              name="title"
            />
            <button type="submit" className="search-button">
              <img src="/img/search.svg" alt="" />
            </button>
          </form>
        </div>
        <div className="user-info">
          {isAuthenticated ? (
            <button onClick={redirectToUserProfile}
              style={{cursor: "pointer"}}
            >
              <img src="/img/user.svg" alt="user" />
              <div className="user-name">{userName}</div>
            </button>
          ) : (
            <Link to="/auth">
              <img src="/img/log_in_icon.svg" alt="log in" />
              <div className="user-name">Авторизоваться</div>
            </Link>
          )}
          <button
            type="button"
            className="menu-button"
            onClick={() => switchDropout()}
          >
            <img src="/img/arrow_down.svg" alt="menu" />
          </button>
        </div>
        {!dropoutHidden && (
          <div
            className="dropout-menu"
            onMouseLeave={() => {
              switchDropout();
            }}
          >
            {isAuthenticated && (
              <div>
                <button type="button" onClick={redirectToUserProfile}>
                  <img src="/img/plain_user.svg" alt="" />
                  <div className="menu-value">Мой профиль</div>
                </button>
              </div>
            )}
            <div>
              <Link to="/" onClick={() => {setDropoutHidden(true)}}>
                <img src="/img/search_white.svg" alt="" />
                <div className="menu-value">Поиск текстов</div>
              </Link>
            </div>
            <div>
              <Link to="/searchUser" onClick={() => {setDropoutHidden(true)}}>
                <img src="/img/users.svg" alt="" />
                <div className="menu-value">Поиск пользователей</div>
              </Link>
            </div>
            {isAuthenticated && (
              <div>
                <button type="button" onClick={logout}>
                  <img src="/img/log_out_icon.svg" alt="" />
                  <div className="menu-value">Выйти из системы</div>
                </button>
              </div>
            )}
          </div>
        )}
      </div>
    </header>
  );
}
