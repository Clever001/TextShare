import CustomButton from "../../4-Widgets/CustomButton/CustomButton"
import ValueInput from "../../4-Widgets/ValueInput/ValueInput"
import "./ProfileSettingsF.css"

type Props = {
  defaultName: string,
  defaultEmail: string,
}

export default function ProfileSettingsF({
  defaultName, defaultEmail
}: Props) {
  return <form className="profile-settings-feature">
    <div className="settings-container">
      <ValueInput type={"input"} keyPosition={"left"} 
      label="Имя пользователя" formSearchName="username"
      defaultValue={defaultName}
      hasRollbackButton={true} />
      <ValueInput type={"input"} keyPosition={"left"} 
      label="Электронная почта" formSearchName="email"
      defaultValue={defaultEmail}
      hasRollbackButton={true} />
    </div>
    <CustomButton leftIconUrl={null} rightIconUrl="/img/save_white.svg"
      text="Сохранить" color="green" type="submit" target="save"/>
  </form>
}