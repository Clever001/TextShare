import { useState } from "react";
import AuthF from "../3-Features/Auth/AuthF";
import RegisterF from "../3-Features/Register/RegisterF";

export default function AuthPage() {
  const [isAuth, setIsAuth] = useState<boolean>(true);

  const switchFeature = () => {
    setIsAuth((prev) => !prev);
  };

  return (
    <div>
      {isAuth ? (
        <AuthF
          onRegisterClick={() => {
            switchFeature();
          }}
        />
      ) : (
        <RegisterF
          onLoginClick={() => {
            switchFeature();
          }}
        />
      )}
    </div>
  );
}
