import { createContext } from 'react';
import type { UserDto } from '../6-Shared/Dtos';

export type UserContextType = {
  getUser: () => UserDto | null,
  setUser: (userName: string) => void,
  clearUser: () => void,
}

export const UserContext = createContext<UserContextType>({
  getUser: () => {throw new Error("Usage of not inited context");},
  setUser: (_userName: string) => {throw new Error("Usage of not inited context");},
  clearUser: () => {throw new Error("Usage of not inited context");},
});

export const UserContextProvider : React.FC<{ children: React.ReactNode }> = ({children}) => {
  const caretColors: string[] = ["#FFF0F5", "#FFE4E1", "#FFDAB9", "#FFECB3", "#FFE599", "#FFF9B0", "#F0FFC0", "#D4F1C6", 
    "#C1E1C1", "#B2DFDB", "#B0E0E6", "#ADD8E6", "#B0C4DE", "#C6E2FF", "#E0FFFF", "#E6E6FA", "#DDA0DD", "#EEC5E5", 
    "#FFD1DC", "#FFE8D4"];
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
    const colorIndex = hash % caretColors.length;
    return caretColors[colorIndex];
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