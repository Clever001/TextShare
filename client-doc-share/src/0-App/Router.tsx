import { createBrowserRouter } from "react-router-dom"
import App from "./App"
import { SearchDocPage } from "../2-Pages/SearchDoc/SearchDocPage"
import { AuthPage } from "../2-Pages/Auth/AuthPage"
import { CreatePage } from "../2-Pages/Create/CreatePage"
import { ViewPage } from "../2-Pages/View/ViewPage"
import { EditPage } from "../2-Pages/Edit/EditPage"
import { ProfilePage } from "../2-Pages/Profile/ProfilePage"
import { SearchUserPage } from "../2-Pages/SearchUser/SearchUserPage"
import { NotFoundPage } from "../2-Pages/NotFound/NotFound"

export const router = createBrowserRouter([
  {
    path: "/",
    element: <App />,
    children: [
      { path: "", element: <SearchDocPage /> },
      { path: "auth", element: <AuthPage /> },
      { path: "create", element: <CreatePage /> },
      { path: "view/:docid", element: <ViewPage /> },
      { path: "edit/:docid", element: <EditPage /> },
      { path: "profile/:userid", element: <ProfilePage /> },
      { path: "searchUser/", element: <SearchUserPage /> },
      { path: "*", element: <NotFoundPage /> }
    ]
  }
])
