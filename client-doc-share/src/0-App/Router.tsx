import { createBrowserRouter } from "react-router-dom";
import App from "./App";
import SearchDocPage from "../2-Pages/SearchDocPage";
import AuthPage from "../2-Pages/AuthPage";
import CreatePage from "../2-Pages/CreatePage";
import ViewPage from "../2-Pages/ViewPage";
import EditPage from "../2-Pages/Edit/EditPage";
import ProfilePage from "../2-Pages/ProfilePage";
import SearchUserPage from "../2-Pages/SearchUserPage";
import NotFoundPage from "../2-Pages/NotFound";

export const router = createBrowserRouter([
  {
    path: "/",
    element: <App />,
    children: [
      { path: "", element: <SearchDocPage /> },
      { path: "auth", element: <AuthPage /> },
      { path: "create", element: <CreatePage /> },
      { path: "view/:docId", element: <ViewPage /> },
      { path: "edit/:docId", element: <EditPage /> },
      { path: "profile/:userName", element: <ProfilePage /> },
      { path: "searchUser/", element: <SearchUserPage /> },
      { path: "*", element: <NotFoundPage /> },
    ],
  },
]);
