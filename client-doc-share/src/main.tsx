import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { router } from "./0-App/Router.tsx";
import { RouterProvider } from "react-router-dom";
import { AuthProvider } from "./1-Processes/AuthContext.tsx";

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <AuthProvider>
      <RouterProvider router={router} />
    </AuthProvider>
  </StrictMode>,
);
