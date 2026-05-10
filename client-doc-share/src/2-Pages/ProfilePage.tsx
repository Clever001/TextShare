import { useCallback, useContext, useEffect, useMemo, useState } from "react";
import SearchDocF from "../3-Features/SearchDoc/SearchDocF";
import ProfileTitle from "../4-Widgets/ProfileTitle/ProfileTitle";
import SectionTitle from "../4-Widgets/SectionTitle/SectionTitle";
import ProfileSettingsF from "../3-Features/ProfileSettings/ProfileSettingsF";
import { useNavigate, useParams } from "react-router-dom";
import type { FullUserDto } from "../6-Shared/ApiClient";
import { AuthContext } from "../1-Processes/AuthContext";
import { generateAccountApi } from "../6-Shared/utils";

export default function ProfilePage() {
  const accountApi = useMemo(generateAccountApi, [])
  const [showProfile, setShowProfile] = useState<boolean>(true)
  var { userName } = useParams()
  const navigate = useNavigate()
  const [profileInfo, setProfileInfo] = useState<FullUserDto>({
    id: "",
    userName: "",
    email: "",
    createdOn: ""
  })

  const authContext = useContext(AuthContext)
  if (!authContext) {
    throw new Error("Auth Context cannot be null")
  }
  const isAuthenticated = authContext.isAuthenticated

  useEffect(() => {
    if (userName) {
      fetchProfileInfo(userName)
    } else {
      alert("Имя пользователя не может быть пустым")
      navigate("/")
    }
  }, [])

  useEffect(() => {
    setShowProfile(true)
  }, [isAuthenticated])

  const fetchProfileInfo = useCallback(async (userName: string) => {
    try {
      const { data } = await accountApi.getAccountByUserName(userName)
      setProfileInfo(data)
    } catch (err) {
      console.error(err)
      alert("Сервис недоступен. Попробуйте позже")
      navigate("/")
    }
  }, [])

  const switchShowProfile = useCallback(() => {
    setShowProfile((sp) => !sp)
  }, [])

  const onUserUpdated = useCallback((un: string) => {
    userName = un
    setShowProfile(true)
    fetchProfileInfo(userName!)
  }, [userName])

  return (
    <div>
      <ProfileTitle
        userName={profileInfo.userName}
        createdOn={profileInfo.createdOn == "" ? 
          new Date()
          :
          new Date(profileInfo.createdOn)}
        showSwitchButton={isAuthenticated}
        onSwitch={() => {
          switchShowProfile();
        }}
        switchBtnColor={showProfile ? "blue" : "yellow"}
        switchBtnIconUrl={
          showProfile ? "/img/settings_white.svg" : "/img/undo_white.svg"
        }
        switchBtnText={showProfile ? "Изменение профиля" : "Отменить"}
      />
      {profileInfo.id != "" &&
        <div>
          {showProfile ? (
            <SearchDocF isProfilePage={true} userId={profileInfo.id} />
          ) : (
            <div>
              <SectionTitle title="Изменение данных профиля" />
              <ProfileSettingsF onUserUpdated={onUserUpdated}/>
            </div>
          )}
        </div>
      }
    </div>
  );
}
