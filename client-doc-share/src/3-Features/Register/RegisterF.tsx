import CustomButton from "../../4-Widgets/CustomButton/CustomButton"
import SectionTitle from "../../4-Widgets/SectionTitle/SectionTitle"
import ValueInput from "../../4-Widgets/ValueInput/ValueInput"
import "./RegisterF.css"

type Props = {
  onLoginClick: () => void,
}

export default function RegisterF({
  onLoginClick,
}: Props) {
  return <div className="register-feature">
    <SectionTitle title="Регистрация" />
    <form className="auth-form">
      <ValueInput type="input" keyPosition="right" label="Имя пользователя"
        formSearchName="login" hasRollbackButton={false} />
      <ValueInput type="input" keyPosition="right" label="Электронная почта"
        formSearchName="email" hasRollbackButton={false} />
      <ValueInput type="input" keyPosition="right" label="Пароль"
        formSearchName="password" hasRollbackButton={false} />
      <ValueInput type="input" keyPosition="right" label="Повтор пароля"
        formSearchName="password-repeat" hasRollbackButton={false} />
      <CustomButton leftIconUrl={null} rightIconUrl={null} text="Зарегистрироваться"
        color="green" type="submit" target="auth" />
    </form>
    <p>Нет учетной записи?</p>
    <CustomButton leftIconUrl={null} rightIconUrl={null} text="Войти"
      color="blue" type="button" target="to-register" onClick={() => {onLoginClick()}}/>
  </div>
}