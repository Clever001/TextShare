import { useState } from "react";
import SearchDocF from "../3-Features/SearchDoc/SearchDocF";
import ProfileTitle from "../4-Widgets/ProfileTitle/ProfileTitle";
import SectionTitle from "../4-Widgets/SectionTitle/SectionTitle";
import ProfileSettingsF from "../3-Features/ProfileSettings/ProfileSettingsF";

export default function ProfilePage() {
  const [showProfile, setShowProfile] = useState<boolean>(true);

  const switchShowProfile = () => {
    setShowProfile((sp) => !sp);
  }

  return <div>
    <ProfileTitle userName="NicknName001" createdOn={new Date("2026-04-01T12:00:00")}
      showSwitchButton={true} onSwitch={() => { switchShowProfile(); }}
      switchBtnColor={showProfile ? "blue" : "yellow"}
      switchBtnIconUrl={showProfile ? "/img/settings_white.svg" : "/img/undo_white.svg"}
      switchBtnText={showProfile ? "Изменение профиля" : "Отменить"} />
    {showProfile ?
      <SearchDocF isProfilePage={true} />
    :
      <div>
        <SectionTitle title="Изменение данных профиля" />
        <ProfileSettingsF defaultName={"some random name"} 
          defaultEmail={"random@email.com"} />
      </div>
    }
  </div>;
}
