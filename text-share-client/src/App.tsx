import React, { useContext, useEffect, useRef, useState } from 'react';
import { Link, Outlet, useNavigate } from 'react-router-dom';
import './App.css';
import Header from './Components/Header/Header';
import Cookies from 'js-cookie';
import { AuthContext } from './Context/AuthContext';
import SidePanel from './Components/SidePanel/SidePanel';

const App = () => {
    // Global
    const navigate = useNavigate();
    const [header_userName, header_setUserName] = useState<string>("");

    const authContext = useContext(AuthContext);
    if (!authContext) {
        throw new Error("AuthContext must be used within a AuthProvider");
    }
    const validAuth = authContext.validAuth;
    const setValidAuth = authContext.setValidAuth;

    // Header
    const [header_query, header_setQuery] = useState<string>("");
    const [header_menuHidden, header_setMenuHidden] = useState<boolean>(true);

    const header_onSearchQueryChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        header_setQuery(e.target.value);
    }

    const header_onSearchSubmit = (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        navigate(`/?title=${encodeURIComponent(header_query)}`);
    }

    const header_onUserClick = (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        const form = e.currentTarget;
        const input = form.elements[0] as HTMLInputElement;
        navigate(`/profile/${input.value}`);
    }

    const header_onShowMenuButtonClick = (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
        const isHidden = header_menuHidden as boolean;
        header_setMenuHidden(!isHidden);
    }

    const Header_onLogOutClick = (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
        Cookies.remove("userId");
        Cookies.remove("userName");
        Cookies.remove("email");
        Cookies.remove("token");
        setValidAuth(false);
    }

    useEffect(() => {
        const getUserName = async () => {
            if (validAuth) {
                const userName = Cookies.get("userName");
                if (typeof (userName) == "string") {
                    header_setUserName(userName);
                } else {
                    header_setUserName("")
                }
            } else {
                header_setUserName("");
            }
        };
        getUserName();
    }, [validAuth]);

    const Header_onProfileClick = (e: React.MouseEvent<HTMLButtonElement, MouseEvent>): void => {
        const userName = Cookies.get("userName");
        if (!userName) {
            alert("Перед перезодом на страницу профиля сначала необходимо зарегистрироваться в системе.");
            navigate("/auth");
            return;
        }

        navigate("/profile/" + userName);
    }


    const header_menuButtonRef = useRef<HTMLButtonElement>(null);
    const header_menuRef = useRef<HTMLDivElement>(null);

    useEffect(() => {
        const handleClickOutside = (e: MouseEvent) => {
            if (
                header_menuHidden ||
                header_menuButtonRef.current?.contains(e.target as Node) ||
                header_menuRef.current?.contains(e.target as Node)
            ) {
                return;
            }
            header_setMenuHidden(true);
        }

        document.addEventListener('mousedown', handleClickOutside);
        return () => {
            document.removeEventListener('mousedown', handleClickOutside);
        }
    }, [header_menuHidden]);

    const header_closeMenu = () => {
        header_setMenuHidden(true);
    }


    return (
        <div className="app">
            <Header
                userName={header_userName}
                query={header_query}
                menuHidden={header_menuHidden}
                onSearchQueryChange={header_onSearchQueryChange}
                onSearchSubmit={header_onSearchSubmit}
                onUserClick={header_onUserClick}
                onShowMenuButtonClick={header_onShowMenuButtonClick}
                onLogOutClick={Header_onLogOutClick}
                onProfileClick={Header_onProfileClick}
                menuButtonRef={header_menuButtonRef}
                menuRef={header_menuRef}
                closeMenu={header_closeMenu}
            />

            <div className="container-fluid">
                <div className="container">
                    <div className="row">
                        <div className="col-lg-9 col-12 content">
                            <Outlet />
                        </div>
                        <div className="col-lg-3 col-12 side-bar">
                            <SidePanel />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default App;