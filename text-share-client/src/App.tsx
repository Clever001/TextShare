import React, { useState } from 'react';
import { Link, Outlet, useNavigate } from 'react-router-dom';
import './App.css';

const App = () => {
    const [query, setQuery] = useState("");
    const [menuHidden, setMenuHidden] = useState(true);
    const navigate = useNavigate();

    const onSearchQueryChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setQuery(e.target.value);
    }

    const onSearchSubmit = (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        navigate(`/search?query=${encodeURIComponent(query)}`);
    }

    const onUserClick = (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        const form = e.currentTarget;
        const input = form.elements[0] as HTMLInputElement;
        navigate(`/profile/${input.value}`);
    }

    const onShowMenuButtonClick = (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
        const isHidden = menuHidden as boolean;
        setMenuHidden(!isHidden);
    }

    return (
        <div className="app">
            <header>
                <div className="my-container">
                    <div className="logo">
                        <Link to="/">
                            <img src="img/logo.svg" alt="logo"></img>
                            <div>TextShare</div>
                        </Link>
                    </div>
                    <div className="add-button">
                        <Link to="/">
                            <img src="img/add_icon.svg" alt="add" />
                            <div>Добавить текст</div>
                        </Link>
                    </div>
                    <div className="search-text">
                        <form onSubmit={onSearchSubmit} className="search-bar">
                            <input type="text" placeholder="Поиск" value={query} onChange={onSearchQueryChange} className="search-input"/>
                            <button type="submit" className="search-button">
                                <img src="img/search_icon_black.svg" alt=""/>
                            </button>
                        </form>
                    </div>
                    <div className="user-info">
                        <form onSubmit={onUserClick}> {/* TODO: API */}
                            <input value="test" type="text" hidden={true} disabled={true}/>
                            <button type="submit">
                                <img src="img/user_icon.svg" alt="user" />
                                <div className="user-name">TextShare</div>
                            </button>
                        </form>
                        <button type="button" className="menu-button" onClick={onShowMenuButtonClick}>
                            <img src="img/dropout_menu.svg" alt="menu" />
                        </button>
                        
                    </div>
                    <div className="dropout-menu" hidden={menuHidden}>
                            <div>
                                <Link to="/profile">
                                    <img src="img/user_simple_icon.svg" alt="" />
                                    <div className="text">Мой профиль</div>
                                </Link>
                            </div>
                            <div>
                                <Link to="/profileContentChange">
                                    <img src="img/edit_icon.svg" alt="" />
                                    <div className="text">Редактировать профиль</div>
                                </Link>
                            </div>
                            <div>
                                <Link to="/textSearch">
                                    <img src="img/search_icon.svg" alt="" />
                                    <div className="text">Поиск текстов</div>
                                </Link>
                            </div>
                            <div>
                                <Link to="/userSearch">
                                    <img src="img/users_icon.svg" alt="" />
                                    <div className="text">Поиск пользователей</div>
                                </Link>
                            </div>
                            <div>
                                <Link to="/"> {/* TODO: Реализовать выход из системы */}
                                    <img src="img/log_out_icon.svg" alt="" />
                                    <div className="text">Выйти из системы</div>
                                </Link>
                            </div>
                        </div>
                </div>
            </header>

            {/* <div className="fluid-container">
                <div className="container">
                    <div className="row">
                        <div className="col-1">Пример</div>
                        <div className="col-1">Пример</div>
                        <div className="col-1">Пример</div>
                        <div className="col-1">Пример</div>
                        <div className="col-1">Пример</div>
                        <div className="col-1">Пример</div>
                        <div className="col-1">Пример</div>
                        <div className="col-1">Пример</div>
                        <div className="col-1">Пример</div>
                        <div className="col-1">Пример</div>
                        <div className="col-1">Пример</div>
                        <div className="col-1">Пример</div>
                    </div>
                </div>
            </div> */}

            {/* <div className="my-container test">
                <div className="column">Пример</div>
                <div className="column">Пример</div>
                <div className="column">Пример</div>
                <div className="column">Пример</div>
                <div className="column">Пример</div>
                <div className="column">Пример</div>
                <div className="column">Пример</div>
                <div className="column">Пример</div>
                <div className="column">Пример</div>
                <div className="column">Пример</div>
                <div className="column">Пример</div>
                <div className="column">Пример</div>
            </div> */}
            <Outlet />
        </div>
    );
};

export default App;