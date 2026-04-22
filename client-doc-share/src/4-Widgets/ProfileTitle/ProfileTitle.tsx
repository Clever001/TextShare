import { dateToString } from "../../6-Shared/utils";
import CustomButton from "../CustomButton/CustomButton";
import "./ProfileTitle.css"

type Props = {
  userName: string,
  createdOn: Date,
  showSwitchButton: boolean,
  onSwitch: () => void,
  switchBtnColor: "blue" | "yellow",
  switchBtnIconUrl: string,
  switchBtnText: string,
}

export default function ProfileTitle({
  userName, createdOn, showSwitchButton, onSwitch,
  switchBtnColor, switchBtnIconUrl, switchBtnText
}: Props) {
  return <div className="profile-title">
    <div className="title">
      <img src="/img/plain_user_black.svg" alt="user" />
      <div className="title-content">
        <p className="username">Профиль {userName}</p>
        <p className="created-on">Дата создания {dateToString(createdOn)}</p>
      </div>
    </div>

    {showSwitchButton &&
      <CustomButton leftIconUrl={null} rightIconUrl={switchBtnIconUrl}
        text={switchBtnText} color={switchBtnColor} type="button" target="settings"
        onClick={() => onSwitch()} />
    }
  </div>
}
