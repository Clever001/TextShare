import CustomButton from "../../4-Widgets/CustomButton/CustomButton";
import RolesTable from "../../4-Widgets/RolesTable/RolesTable";
import Search from "../../4-Widgets/Search/Search";
import SectionTitle from "../../4-Widgets/SectionTitle/SectionTitle";
import ValueInput from "../../4-Widgets/ValueInput/ValueInput";
import { UserDevRole, type CreateUpdateDocDto, type UserWithoutTokenDto, type UserWithTokenDto } from "../../6-Shared/ApiClient";
import "./CreateDocF.css";
import { useCallback, useContext, useEffect, useMemo, useState } from "react";
import { debounce } from 'lodash';
import { generateAccountApi, generateDocumentApiAuth, getApiErrors, isUnauthError } from "../../6-Shared/utils";
import { AuthContext } from "../../1-Processes/AuthContext";
import { useNavigate } from "react-router-dom";

type Props = {};

export default function CreateDocF(props: Props) {
  const navigate = useNavigate()
  const authContext = useContext(AuthContext)
  if (!authContext) {
    throw new Error("Auth Context cannot be null")
  }

  const getUserInfo = authContext.getUserInfo
  const setUserInfo = authContext.setUserInfo
  const [currentUser, setCurrentUser] = useState<UserWithTokenDto>(
    {id: "", userName: "", email: "", token: ""}
  )

  useEffect(() => {
    const userInfo = getUserInfo()
    if (!userInfo) {
      alert("Перед созданием документов необходимо авторизоваться в системе")
      navigate("/auth")
      return
    }
    setCurrentUser(userInfo)
  }, [getUserInfo])


  const docApi = useMemo(() => generateDocumentApiAuth(currentUser.token), [currentUser])
  const authApi = useMemo(generateAccountApi, [])

  const [errors, setErrors] = useState<string[]>([])
  const [userToRoles, setUserToRoles] = useState<{ user: UserWithoutTokenDto, role: UserDevRole }[]>([])
  const [searchVal, setSearchVal] = useState<string>("")
  const [suggestions, setSuggestions] = useState<UserWithoutTokenDto[]>([])

  const debouncedFetch = useMemo(
    () => debounce(async (search: string) => {
      if (search.length == 0) {
        setSuggestions([])
        return
      }

      try {
        const { data } = await authApi.getAccountsThatStartsWith(10, search)
        setSuggestions(data)
      } catch (err) {
        console.error(err)
        setSuggestions([])
      }
    }, 500),
    []
  )

  useEffect(() => {
    return () => debouncedFetch.cancel();
  }, [debouncedFetch]);

  const onSearchValChange = useCallback((e: React.ChangeEvent<HTMLInputElement, HTMLInputElement>) => {
    const value = e.target.value
    setSearchVal(value)
    debouncedFetch(value)
  }, [debouncedFetch])

  const addFoundUser = useCallback(() => {
    if (suggestions.length === 0) {
      if (searchVal === "") {
        alert("Введите, пожалуйста, запрос для поиска.");
      } else {
        alert("Пользователей с данным именем не существует")
      }
      return;
    }

    const foundUser = suggestions[0];

    if (foundUser.id == currentUser.id) {
      alert("Владельцу документа нельзя выдялеть никакую роль, так как ему изначально выдаются полные права")
      return
    }

    setUserToRoles((prev) => {
      const isAlreadyAdded = prev.some(userToRole => userToRole.user.id === foundUser.id);

      if (isAlreadyAdded) {
        alert("Этот пользователь уже был добавлен");
        return prev;
      }

      return [...prev, { user: foundUser, role: UserDevRole.Reader }];
    });
    setSearchVal("");
    setSuggestions([]);
  }, [suggestions, searchVal, currentUser])

  const changeRole = useCallback((userId: string, updateRole: UserDevRole) => {
    setUserToRoles(prev =>
      prev.map(item =>
        item.user.id === userId
          ? { ...item, role: updateRole }
          : item
      )
    );
  }, [])

  const deleteUser = useCallback((delUserId: string) => {
    setUserToRoles(prev => {
      return prev.filter(u => u.user.id != delUserId)
    })
  }, [])

  const onCreationSubmit = useCallback(async (e: React.SubmitEvent<HTMLFormElement>) => {
    e.preventDefault()

    const formData = new FormData(e.currentTarget)

    const createDto: CreateUpdateDocDto = {
      title: formData.get("title")?.toString() ?? "",
      description: formData.get("description")?.toString() ?? "",
      tags: (formData.get("tags")?.toString() ?? "").split(" ").filter(e => e.length > 0),
      roles: userToRoles.reduce((acc, ur) => {
        acc[ur.user.id] = ur.role;
        return acc;
      }, {} as Record<string, UserDevRole>)
    }

    try {
      const {data} = await docApi.createDocument(createDto)
      navigate(`/edit/${data.id}`)
    } catch (err) {
      if (isUnauthError(err)) {
        alert("Требуется перезайти в аккаунт")
        setUserInfo(null)
        navigate("/auth")
        return;
      }
      const errors = getApiErrors(err)
      setErrors(errors)
    }
  }, [docApi, userToRoles])

  return (
    <form className="create-doc-feature" onSubmit={onCreationSubmit}>
      <SectionTitle title="Создание пустого документа" />
      <div className="settings-container">
        <ValueInput
          widgetType="input"
          keyPosition="left"
          label="Название"
          formSearchName="title"
          hasRollbackButton={false}
        />
        <ValueInput
          widgetType="input"
          keyPosition="left"
          label="Теги"
          formSearchName="tags"
          hint="первый_тег второй_тег"
          hasRollbackButton={false}
        />
        <ValueInput
          widgetType="textarea"
          keyPosition="left"
          label="Описание"
          formSearchName="description"
          hasRollbackButton={false}
        />
      </div>
      <SectionTitle title="Настройка ролей доступа (режим редактирования)" />
      <Search
        widgetType="user-search"
        hint="Введите имя пользователя"
        buttonText="Добавить пользователя"
        value={searchVal}
        onValueChange={onSearchValChange}
        suggestions={suggestions.map(u => u.userName)}
        onActionBtnClick={addFoundUser}
      />
      <RolesTable users={userToRoles} onRoleChange={changeRole} onDelete={deleteUser} />
      <div className="errors">
        {errors.map(e => <p key={e}>{e}</p>)}
      </div>
      <CustomButton
        leftIconUrl={null}
        rightIconUrl={null}
        text="Создать документ"
        color="green"
        type="submit"
        target="create"
      />
    </form>
  );
}
