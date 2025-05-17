import React from 'react';
import ReactDOM from 'react-dom/client';
import { RouterProvider } from 'react-router-dom';
import { router } from "./Router/Router";
import { AuthProvider } from './Context/AuthContext';

const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);
root.render(
  <AuthProvider>
    <RouterProvider router={router} />
  </AuthProvider>

  // <React.StrictMode>
  //   <AuthProvider>
  //     <RouterProvider router={router} />
  //   </AuthProvider>
  // </React.StrictMode>
);
