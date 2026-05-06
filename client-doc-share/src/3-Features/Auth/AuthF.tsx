import { useCallback, useContext, useMemo, useState } from "react";
import { AuthContext } from "../../1-Processes/AuthContext";
import CustomButton from "../../4-Widgets/CustomButton/CustomButton";
import SectionTitle from "../../4-Widgets/SectionTitle/SectionTitle";
import ValueInput from "../../4-Widgets/ValueInput/ValueInput";
import "./AuthF.css";
import { generateAccountApi, getApiErrors } from "../../6-Shared/utils";
import type { LoginDto } from "../../6-Shared/ApiClient";
import { useNavigate } from "react-router-dom";

type Props = {
  onRegisterClick: () => void;
};

export default function AuthF({ onRegisterClick }: Props) {
  const authApi = useMemo(generateAccountApi, []);
  const navigate = useNavigate()

  const [errors, setErrors] = useState<string[]>([])
  const authContext = useContext(AuthContext)
  if (!authContext) {
    throw new Error("Auth Context cannot be null")
  }

  const setUserInfo = authContext.setUserInfo

  const onLoginSubmit = useCallback(async (e: React.SubmitEvent<HTMLFormElement>) => {
    e.preventDefault()

    const formData = new FormData(e.currentTarget)
    const loginDto: LoginDto = {
      userNameOrEmail: formData.get("login-or-email")?.toString() ?? "",
      password: formData.get("password")?.toString() ?? ""      
    }

    try {
      const {status, data} = await authApi.login(loginDto)
      setUserInfo(data)
      navigate("/")
    } catch (err) {
      const errors = getApiErrors(err)
      setErrors(errors)
    }
  }, [authApi])
  

  return (
    <div className="auth-feature">
      <SectionTitle title="Вход" />
      <form className="auth-form" onSubmit={onLoginSubmit}>
        <ValueInput
          widgetType="input"
          keyPosition="right"
          label="Имя пользователя или почта"
          formSearchName="login-or-email"
          hasRollbackButton={false}
        />
        <ValueInput
          widgetType="password"
          keyPosition="right"
          label="Пароль"
          formSearchName="password"
          hasRollbackButton={false}
        />
        {(errors.length > 0) &&
          <div className="errors">
            {errors.map(e => {
              return <p className="error">{e}</p>
            })}
          </div>
        }
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
