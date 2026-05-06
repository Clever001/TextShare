import { useCallback, useContext, useMemo, useState } from "react";
import CustomButton from "../../4-Widgets/CustomButton/CustomButton";
import SectionTitle from "../../4-Widgets/SectionTitle/SectionTitle";
import ValueInput from "../../4-Widgets/ValueInput/ValueInput";
import { generateAccountApi, getApiErrors } from "../../6-Shared/utils";
import "./RegisterF.css";
import { AuthContext } from "../../1-Processes/AuthContext";
import type { RegisterDto } from "../../6-Shared/ApiClient";
import { useNavigate } from "react-router-dom";

type Props = {
  onLoginClick: () => void;
};

export default function RegisterF({ onLoginClick }: Props) {
  const authApi = useMemo(generateAccountApi, [])
  const navigate = useNavigate()

  const [errors, setErrors] = useState<string[]>([])
  const authContext = useContext(AuthContext)
  if (!authContext) {
    throw new Error("Auth Context cannot be null")
  }

  const setUserInfo = authContext.setUserInfo

  const onRegisterSubmit = useCallback(async (e: React.SubmitEvent<HTMLFormElement>) => {
    e.preventDefault()

    const formData = new FormData(e.currentTarget)
    const registerDto: RegisterDto = {
      userName: formData.get("username")?.toString() ?? "",
      email: formData.get("email")?.toString() ?? "",
      password: formData.get("password")?.toString() ?? ""
    }
    const passwordRepeat = formData.get("confirm-password")?.toString() ?? ""

    if (passwordRepeat !== registerDto.password) {
      setErrors(["Пароли должны совпадать"]);
      return;
    }

    try {
      const { status, data } = await authApi.register(registerDto)
      setUserInfo(data)
      navigate("/")
    } catch (err) {
      const errors = getApiErrors(err)
      setErrors(errors)
    }
  }, [authApi])

  return (
    <div className="register-feature">
      <SectionTitle title="Регистрация" />
      <form className="auth-form" onSubmit={onRegisterSubmit}>
        <ValueInput
          widgetType="input"
          keyPosition="right"
          label="Имя пользователя"
          formSearchName="username"
          hasRollbackButton={false}
        />
        <ValueInput
          widgetType="input"
          keyPosition="right"
          label="Электронная почта"
          formSearchName="email"
          hasRollbackButton={false}
        />
        <ValueInput
          widgetType="password"
          keyPosition="right"
          label="Пароль"
          formSearchName="password"
          hasRollbackButton={false}
        />
        <ValueInput
          widgetType="password"
          keyPosition="right"
          label="Повтор пароля"
          formSearchName="confirm-password"
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
          text="Зарегистрироваться"
          color="green"
          type="submit"
          target="auth"
        />
      </form>
      <p>Нет учетной записи?</p>
      <CustomButton
        leftIconUrl={null}
        rightIconUrl={null}
        text="Войти"
        color="blue"
        type="button"
        target="to-register"
        onClick={() => {
          onLoginClick();
        }}
      />
    </div>
  );
}
