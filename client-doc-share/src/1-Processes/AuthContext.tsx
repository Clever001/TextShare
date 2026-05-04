import React, { createContext, useCallback, useState } from 'react';
import Cookies from 'js-cookie';
import type { UserWithTokenDto } from '../6-Shared/ApiClient';

type AuthContextType = {
  isAuthenticated: boolean,
  getUserInfo: () => UserWithTokenDto | null,
  setUserInfo: (userInfo: UserWithTokenDto | null) => void,
  handleUnauthorized: () => void
}

export const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [isAuthenticated, setIsAuthenticated] = useState<boolean>(() => {
    const userName = Cookies.get("userName")
    const token = Cookies.get("token")
    if (typeof userName == "undefined" || typeof token == "undefined") {
      return false
    }
    return true
  })

  const getUserInfo = () : UserWithTokenDto | null => {
    const id = Cookies.get("userId")
    const userName = Cookies.get("userName")
    const email = Cookies.get("email")
    const token = Cookies.get("token")
    if (typeof id == "undefined" || typeof userName == "undefined" || 
      typeof email == "undefined" || typeof token == "undefined") {
      return null
    }

    return {
      id: id,
      userName: userName,
      email: email,
      token: token
    }
  }

  const setUserInfo = (userInfo: UserWithTokenDto | null) : void => {
    if (userInfo == null) {
      Cookies.remove("userId")
      Cookies.remove("userName")
      Cookies.remove("email")
      Cookies.remove("token")
      setIsAuthenticated(false)
      return
    }
    if (!userInfo.userName || !userInfo.token) {
      throw new Error("Got user with null data.")
    }
    Cookies.set("userId", userInfo.id)
    Cookies.set("userName", userInfo.userName)
    Cookies.set("email", userInfo.email)
    Cookies.set("token", userInfo.token)
    setIsAuthenticated(true)
  }

  const handleUnauthorized = () => {
    alert("Время вашей сессии истекло. Требуется повторная авторизация.")
    setUserInfo(null)
  }

  return (
    <AuthContext.Provider value={{ isAuthenticated, getUserInfo, setUserInfo, handleUnauthorized }}>
      {children}
    </AuthContext.Provider>
  );
};