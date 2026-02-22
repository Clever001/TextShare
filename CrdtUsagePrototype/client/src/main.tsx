import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import App from './0-App/App.tsx'
import { RouterProvider } from 'react-router-dom'
import { router } from './Router/Router.tsx'
import { UserContextProvider } from './1-Processes/UserContext.tsx'

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <UserContextProvider>
      <RouterProvider router={router}/>
    </UserContextProvider>
  </StrictMode>,
)
