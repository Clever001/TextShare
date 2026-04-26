import { useContext } from "react";
import { AuthContext } from "../../1-Processes/AuthContext";
import CustomButton from "../../4-Widgets/CustomButton/CustomButton";
import SectionTitle from "../../4-Widgets/SectionTitle/SectionTitle";
import ValueInput from "../../4-Widgets/ValueInput/ValueInput";
import "./AuthF.css";

type Props = {
  onRegisterClick: () => void;
};

export default function AuthF({ onRegisterClick }: Props) {
  const [errors, setErrors] = useState<string[]>([])
  const authContext = useContext(AuthContext)
  if (!authContext) {
    throw new Error("Auth Context cannot be null")
  }

  const setUserInfo = authContext.setUserInfo
  const showAuth = authContext.showAuth
  const setShowAuth = authContext.setShowAuth

  

  return (
    <div className="auth-feature">
      <SectionTitle title="Вход" />
      <form className="auth-form">
        <ValueInput
          type="input"
          keyPosition="right"
          label="Имя пользователя или почта"
          formSearchName="login-or-email"
          hasRollbackButton={false}
        />
        <ValueInput
          type="input"
          keyPosition="right"
          label="Пароль"
          formSearchName="password"
          hasRollbackButton={false}
        />
        <CustomButton
          leftIconUrl={null}
          rightIconUrl={null}
          text="Войти"
          color="green"
          type="submit"
          target="auth"
        />
      </form>
      <p>Нет учетной записи?</p>
      <CustomButton
        leftIconUrl={null}
        rightIconUrl={null}
        text="Зарегистрироваться"
        color="blue"
        type="button"
        target="to-register"
        onClick={() => onRegisterClick()}
      />
    </div>
  );
}
