import { createContext } from 'react';
import type { UserDto } from '../6-Shared/Dtos';

export type UserContextType = {
  getUser: () => UserDto | null,
  setUser: (userName: string) => void,
  clearUser: () => void,
}

export const UserContext = createContext<UserContextType>({
  getUser: () => {throw new Error("Usage of not inited context");},
  setUser: (userName: string) => {throw new Error("Usage of not inited context");},
  clearUser: () => {throw new Error("Usage of not inited context");},
});

export const UserContextProvider : React.FC<{ children: React.ReactNode }> = ({children}) => {
  var user : UserDto | null = null;

  const getUser = (): UserDto | null => {
    return user;
  };

  const setUser = (userName: string) : void => {
    if (userName.trim() === "") {
      throw Error("Cannot set such user name.");
    }

    user = {
      name: userName,
      cursorColor: stringToColor(userName)
    }
  };

  function stringToColor(str: string) {
    let hash = 0;
    for (let i = 0; i < str.length; i++) {
      hash = str.charCodeAt(i) + ((hash << 5) - hash);
    }
    let color = '#';
    for (let i = 0; i < 3; i++) {
      const value = (hash >> (i * 8)) & 0xff;
      color += ('00' + value.toString(16)).substr(-2);
    }
    return color;
  }

  const clearUser = (): void => {
    user = null;
  }

  return (
    <UserContext.Provider value={{ getUser, setUser, clearUser }}>
      {children}
    </UserContext.Provider>
  )
} 