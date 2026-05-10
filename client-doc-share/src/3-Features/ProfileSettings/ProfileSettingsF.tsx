import { useCallback, useContext, useMemo, useRef, useState } from "react";
import CustomButton from "../../4-Widgets/CustomButton/CustomButton";
import ValueInput from "../../4-Widgets/ValueInput/ValueInput";
import "./ProfileSettingsF.css";
import { AuthContext } from "../../1-Processes/AuthContext";
import type { UpdateUserDto, UserWithTokenDto } from "../../6-Shared/ApiClient";
import { assignFormInput, generateAccountApiAuth, getApiErrors } from "../../6-Shared/utils";
import { useNavigate } from "react-router-dom";

type Props = {
  onUserUpdated: (newName: string) => void
};

export default function ProfileSettingsF({onUserUpdated}: Props) {
  const navigate = useNavigate()
  const authContext = useContext(AuthContext)
  if (!authContext) {
    throw new Error("Auth Context cannot be null")
  }
  const getUserInfo = authContext.getUserInfo
  const setUserInfo = authContext.setUserInfo
  const formRef = useRef<HTMLFormElement>(null);
  
  const [userWithToken, setUserWithToken] = useState<UserWithTokenDto>(() => {
    const userInfo = getUserInfo()
    if (userInfo) {
      return userInfo
    }
    throw new Error(
      "Нельзя открывать данную область страницы, будучи незарегестрированным"
    )
  })
  const [errors, setErrors] = useState<string[]>([])
  const accountApi = useMemo(() => generateAccountApiAuth(userWithToken.token), [userWithToken])

  const onUpdateSubmit = useCallback(async (e: React.SubmitEvent<HTMLFormElement>) => {
    e.preventDefault()

    const formData = new FormData(e.currentTarget)
    const updateDto: UpdateUserDto = {
      userName: formData.get("username")?.toString() ?? "",
      email: formData.get("email")?.toString() ?? ""
    }
    
    const localErrors: string[] = []
    if (!updateDto.userName || updateDto.userName == "") {
      localErrors.push("Поле имени пользователя должно быть заполнено")
    }
    if (!updateDto.email || updateDto.email == "") {
      localErrors.push("Поле электронной почты должно быть заполнено")
    }
    if (localErrors.length > 0) {
      setErrors(localErrors)
      return
    }

    try {
      const {data} = await accountApi.updateAccountInfo(updateDto)
      setUserWithToken(data)
      setUserInfo(data)
      setErrors([])
      navigate(`/profile/${data.userName}`)
      onUserUpdated(data.userName)
    } catch (err) {
      const errors = getApiErrors(err)
      setErrors(errors)
    }
  }, [])

  const onUserNameRollback = useCallback(() => {
    const form = formRef.current
    if (form) {
      assignFormInput(form, "username", userWithToken.userName)
    }
  }, [formRef.current])

  const onEmailRollback = useCallback(() => {
    const form = formRef.current
    if (form) {
      assignFormInput(form, "email", userWithToken.email)
    }
  }, [formRef.current])

  return (
    <form 
      className="profile-settings-feature"
      onSubmit={onUpdateSubmit}
      ref={formRef}
    >
      <div className="settings-container">
        <ValueInput
          widgetType={"input"}
          keyPosition={"left"}
          label="Имя пользователя"
          formSearchName="username"
          defaultValue={userWithToken.userName}
          hasRollbackButton={true}
          onRollback={onUserNameRollback}
        />
        <ValueInput
          widgetType={"input"}
          keyPosition={"left"}
          label="Электронная почта"
          formSearchName="email"
          defaultValue={userWithToken.email}
          hasRollbackButton={true}
          onRollback={onEmailRollback}
        />
      </div>
      {errors.length > 0 && 
        <div className="errors">
          {errors.map(e => <p>{e}</p>)}
        </div>
      }
      <CustomButton
        leftIconUrl={null}
        rightIconUrl="/img/save_white.svg"
        text="Сохранить"
        color="green"
        type="submit"
        target="save"
      />
    </form>
  );
}
