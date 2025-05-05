import React, { createContext, useState } from 'react';
import Cookies from 'js-cookie';

interface AuthContextType {
    validAuth: boolean;
    setValidAuth: (isValid: boolean) => void;
}

export const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
    const [validAuth, setValidAuth] = useState<boolean>(() => {
        const userName = Cookies.get("userName");
        return userName ? true : false;
    })

    return (
        <AuthContext.Provider value={{ validAuth, setValidAuth }}>
            {children}
        </AuthContext.Provider>
    );
};