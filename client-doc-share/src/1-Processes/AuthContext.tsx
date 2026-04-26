import React, { createContext, useCallback, useState } from 'react';
import Cookies from 'js-cookie';
import { type UserWithTokenDto } from '../5-Entities/Auth/dtos'
import { useNavigate } from 'react-router-dom';

type AuthContextType = {
  isAuthenticated: boolean,
  getUserInfo: () => UserWithTokenDto | null,
  setUserInfo: (userInfo: UserWithTokenDto | null) => void,
  showAuth: boolean,
  setShowAuth: React.Dispatch<React.SetStateAction<boolean>>,
  handleUnauthorized: () => void
}

export const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const navigate = useNavigate();
  const [isAuthenticated, setIsAuthenticated] = useState<boolean>(() => {
    const userName = Cookies.get("userName")
    const token = Cookies.get("token")
    if (typeof userName == "undefined" || typeof token == "undefined") {
      return false
    }
    return true
  })

  const getUserInfo = () : UserWithTokenDto | null => {
    const userName = Cookies.get("userName")
    const token = Cookies.get("token")
    if (typeof userName == "undefined" || typeof token == "undefined") {
      return null
    }

    return {
      userName: userName,
      token: token
    }
  }

  const setUserInfo = (userInfo: UserWithTokenDto | null) : void => {
    if (userInfo == null) {
      Cookies.remove("userName")
      Cookies.remove("token")
      setIsAuthenticated(false)
      return
    }
    Cookies.set("userName", userInfo.userName)
    Cookies.set("token", userInfo.token)
    setIsAuthenticated(true)
  }

  const [showAuth, setShowAuth] = useState<boolean>(false)

  const handleUnauthorized = () => {
    alert("Время вашей сессии истекло. Требуется повторная авторизация.")
    setUserInfo(null)
    setShowAuth(true)
    navigate("/auth")
  }

  return (
    <AuthContext.Provider value={{ isAuthenticated, getUserInfo, setUserInfo, showAuth, setShowAuth, handleUnauthorized }}>
      {children}
    </AuthContext.Provider>
  );
};