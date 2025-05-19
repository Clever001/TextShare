import React from 'react'
import { createBrowserRouter } from 'react-router-dom';
import App from '../App';
import Reader from '../Pages/Reader/Reader';
import Editor from '../Pages/Editor/Editor';
import Profile from '../Pages/Profile/Profile';
import TextSearch from '../Pages/TextSearch/TextSearch';
import UserSearch from '../Pages/UserSearch/UserSearch';
import Auth from '../Pages/Auth/Auth';
import ProfileContentChange from '../Pages/ProfileContentChange/ProfileContentChange';
import CreateText from '../Pages/CreateText/CreateText';


export const router = createBrowserRouter([
    {
        path: "/",
        element: <App/>,
        children: [
            { index: true, element: <TextSearch />},
            { path: "reader/:textId", element: <Reader />},
            { path: "editor/:textId", element: <Editor />},
            { path: "profile/:userName", element: <Profile />},
            { path: "profile/", element: <Profile />},
            { path: "createText/", element: <CreateText />},
            { path: "userSearch/", element: <UserSearch />},
            { path: "auth/", element: <Auth/>},
            { path: "profileContentChange/", element: <ProfileContentChange />}
        ]
    }
]);

