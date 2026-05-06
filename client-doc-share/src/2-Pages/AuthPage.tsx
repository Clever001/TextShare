import { useContext, useEffect, useState } from "react";
import AuthF from "../3-Features/Auth/AuthF";
import RegisterF from "../3-Features/Register/RegisterF";
import { AuthContext } from "../1-Processes/AuthContext";
import { useNavigate } from "react-router-dom";

export default function AuthPage() {
  const [showLogin, setShowLogin] = useState<boolean>(true);
  const switchFeature = () => {
    setShowLogin((prev) => !prev);
  };

  const navigate = useNavigate()
  const authContext = useContext(AuthContext)
  if (!authContext) {
    throw new Error("Auth Context cannot be null")
  }

  const isAuthenticated = authContext.isAuthenticated
  useEffect(() => {
    if (isAuthenticated) {
      alert("Вы уже авторизованы в системе")
      navigate("/")
    }
  }, [])

  return (
    <div>
      {showLogin ? (
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
