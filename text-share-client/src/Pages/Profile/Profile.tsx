import React, { useContext, useEffect, useState } from 'react'
import { PaginatedResponseDto, PaginationDto, SortDto, TextFilterWithoutNameDto, TextWithoutContentDto } from '../../Dtos'
import { getSyntaxes } from '../../Services/HelperService'
import TextRow from '../../Components/TextRow/TextRow'
import { useNavigate, useParams } from 'react-router-dom'
import Cookies from 'js-cookie'
import { SearchTextsByNameAPI } from '../../Services/API/TextAPIService'
import { isExceptionDto } from '../../Services/ErrorHandler'
import './Profile.css'
import { AreFriendsAPI, DeleteFriendAPI, DeleteFriendRequestAPI, GetRequestFromMeAPI, GetRequestToMeAPI, ProcessFriendRequestAPI, SendFriendRequestAPI } from '../../Services/API/AccountAPIService'
import { AuthContext } from '../../Context/AuthContext'

type Props = {}

const Profile = (props: Props) => {
  const navigate = useNavigate();

  const authContext = useContext(AuthContext);
  if (!authContext) {
    throw new Error('AuthContext must be used within a AuthProvider');
  }
  const setValidAuth = authContext.setValidAuth;

  const [errors, setErrors] = useState<string[]>([]);
  const [texts, setTexts] = useState<PaginatedResponseDto<TextWithoutContentDto> | null>(null);
  const { userName } = useParams();

  const [currentPage, setCurrentPage] = useState<number>(1);

  const PAGE_SIZE = 10;

  const token = Cookies.get("token") ?? null;

  var filter: TextFilterWithoutNameDto = {
    title: null,
    tags: null,
    syntax: null,
    accessType: null,
    hasPassword: null
  }

  const onFindProfile = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setTexts(null);
    setErrors([]);

    setCurrentPage(1);
    const pagination: PaginationDto = {
      pageNumber: 1,
      pageSize: PAGE_SIZE
    }
    const sort: SortDto = {
      sortBy: 'title',
      sortAscending: true
    }

    const formData = new FormData(e.currentTarget);
    const getFormValue = (s: string): string | null => {
      var data = formData.get(s)?.toString() || null;
      if (typeof (data) === "string" && data === "") {
        data = null;
      }
      return data;
    }

    filter = {
      title: getFormValue("title"),
      tags: getFormValue("tags")?.split(" ") ?? null,
      syntax: getFormValue("syntax"),
      accessType: getFormValue("accessType"),
      hasPassword:
        formData.get("hasPassword")?.toString() === "true"
          ? true
          : formData.get("hasPassword")?.toString() === "false"
            ? false
            : null,
    }

    await searchTexts(pagination, sort);
  }

  const searchTexts = async (pagination: PaginationDto, sort: SortDto) => {
    if (!userName) return;

    const result = await SearchTextsByNameAPI(pagination, sort, filter, userName, token);

    if (isExceptionDto(result)) {
      switch (result.httpCode) {
        case 404:
          setErrors([`Пользователь с именем ${userName} не был найден в системе.`]);
          return;
        default:
          if (result.details)
            setErrors(result.details);
          else
            setErrors([result.description]);
          return;
      }
    }

    setTexts(result);
  }


  useEffect(() => {
    if (!userName) {
      alert("Имя пользователя не может быть пустым.");
      navigate("/");
      return;
    }

    const pagination: PaginationDto = {
      pageNumber: 1,
      pageSize: PAGE_SIZE
    }
    const sort: SortDto = {
      sortBy: 'title',
      sortAscending: true
    }

    filter = {
      title: null,
      tags: null,
      syntax: null,
      accessType: null,
      hasPassword: null
    }

    const applyActionsInOrder = async () => {
      await searchTexts(pagination, sort);
      
      const curUser = Cookies.get("userName");
      if (!curUser) return;

      if (userName.toLowerCase() != curUser.toLowerCase()) {
        await checkFriendship();
      }
    }

    applyActionsInOrder();

  }, [userName]);

  const accessIcons: { [Key: string]: string } = {
    "Personal": "👤",
    "ByReferencePublic": "🌐",
    "ByReferenceAuthorized": "🛡️",
    "OnlyFriends": "👫",
  };

  const getIcon = (accessType: string): string => {
    return accessIcons[accessType] || "❓";
  };


  const handlePageChange = (newPage: number): void => {
    if (newPage < 1 || (texts && newPage > texts.totalPages)) {
      return;
    }
    setCurrentPage(newPage);

    const pagination: PaginationDto = {
      pageNumber: newPage,
      pageSize: PAGE_SIZE
    }

    const sort: SortDto = {
      sortBy: "title",
      sortAscending: true
    }

    searchTexts(pagination, sort);
  }

  const [areFriends, setArefriends] = useState<boolean>(false);
  const [sentRequest, setSentRequest] = useState<boolean>(false);
  const [recievedRequest, setRecievedRequest] = useState<boolean>(false);
  const [noBind, setNoBind] = useState<boolean>(false);

  const checkFriendship = async () => {
    if (!token) return;
    if (!userName) return;

    const friendsCheckPromise = AreFriendsAPI(token, userName);
    const sentRequestCheckPromise = GetRequestFromMeAPI(token, userName);
    const recievedRequestCheckPromise = GetRequestToMeAPI(token, userName);

    const friendsCheck = await friendsCheckPromise;
    const sentRequestCheck = await sentRequestCheckPromise;
    const recievedRequestCheck = await recievedRequestCheckPromise;

    if (typeof (friendsCheck) === "boolean" && friendsCheck) {
      setArefriends(true);
    } else if (!isExceptionDto(sentRequestCheck)) {
      setSentRequest(true);
    } else if (!isExceptionDto(recievedRequestCheck)) {
      setRecievedRequest(true);
    } else {
      setNoBind(true);
    }
  }

  const recheckFriendship = async () => {
    setArefriends(false);
    setSentRequest(false);
    setRecievedRequest(false);
    setNoBind(false);
    await checkFriendship();
  }

  const onSendRequestClick = async (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
    if (!token) return;
    if (!userName) return;

    const response = await SendFriendRequestAPI(token, userName);

    if (!isValidResponse(response)) return;

    await recheckFriendship();
  }

  const onDeleteRequestClick = async (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
    if (!token) return;
    if (!userName) return;

    const response = await DeleteFriendRequestAPI(token, userName);

    if (!isValidResponse(response)) return;

    await recheckFriendship();
  }

  const onAproveRequestClick = async (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
    if (!token) return;
    if (!userName) return;

    const response = await ProcessFriendRequestAPI(token, userName, {
      acceptRequest: true
    });

    if (!isValidResponse(response)) return;

    await recheckFriendship();
  }

  const onRejectRequestClick = async (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
    if (!token) return;
    if (!userName) return;

    const response = await ProcessFriendRequestAPI(token, userName, {
      acceptRequest: false
    });

    if (!isValidResponse(response)) return;

    await recheckFriendship();
  }

  const onDeleteFriendClick = async (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
    if (!token) return;
    if (!userName) return;

    const response = await DeleteFriendAPI(token, userName);

    if (!isValidResponse(response)) return;

    await recheckFriendship();
  }

  const isValidResponse = (e: any): boolean => {
    if (isExceptionDto(e)) {
      if (e.httpCode == 403) {
        alert("Перед выполнением запроса сначала необходимо повторно авторизоваться в системе.")
        setValidAuth(false);
        Cookies.remove("id");
        Cookies.remove("token");
        Cookies.remove("userName");
        Cookies.remove("email");
        navigate("/auth");
      }
      if (e.details)
        setErrors(e.details);
      else
        setErrors([e.description]);
      return false;
    }
    return true;
  }

  return (
    <div className="profile">
      <div className="header">
        <div className="info">
          <img src="img/user_icon_black.svg" alt="user" />
          <p className="name">{userName}</p>
        </div>
        <div className="action-button">
          {noBind &&
            <button type="button" onClick={onSendRequestClick}>Отправить запрос в друзья</button>
          }
          {sentRequest &&
            <button type="button" onClick={onDeleteRequestClick}>Удалить запрос в друзья</button>
          }
          {recievedRequest &&
            <div>
              <button type="button" onClick={onAproveRequestClick}>Одобрить запрос в друзья</button>
              <button type="button" onClick={onRejectRequestClick}>Отвергнуть запрос в друзья</button>
            </div>
          }
          {areFriends &&
            <button type="button" onClick={onDeleteFriendClick}>Удалить из списка друзей</button>
          }
        </div>
      </div>

      {errors.map(e => {
        return (<div className="error">{e}</div>)
      })}

      <form onSubmit={onFindProfile}>
        <div className="title">Тексты</div>
        <div className="filters">
          <table>
            <tbody>
              <tr>
                <td className="col1"><p>Название</p></td>
                <td className="col2"><input type="text" name="title" /></td>
              </tr>
              <tr>
                <td className="col1"><p>Теги</p></td>
                <td className="col2"><input type="text" name="tags" /></td>
              </tr>
              <tr>
                <td className="col1"><p>Тип синтаксиса</p></td>
                <td className="col2"><select name="syntax">
                  <option value=""></option>
                  {getSyntaxes().map(s => {
                    return (
                      <option key={s} value={s}>{s}</option>
                    );
                  })}
                </select></td>
              </tr>
              <tr>
                <td className="col1"><p>Тип доступа</p></td>
                <td className="col2">
                  <select name="accessType">
                    <option value=""></option>
                    <option value="byReferencePublic">Публичный</option>
                    <option value="byReferenceAuthorized">Публичный с авторизацией</option>
                    <option value="onlyFriends">Только для друзей</option>
                    <option value="personal">Приватный</option>
                  </select>
                </td>
              </tr>
              <tr>
                <td className="col1"><p>Наличие пароля</p></td>
                <td className="col2">
                  <select name="hasPassword">
                    <option value=""></option>
                    <option value="true">Есть пароль</option>
                    <option value="false">Нет пароля</option>
                  </select>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
        <button type="submit">Применить фильтр</button>
      </form>

      <div className="result">
        {texts &&
          <table>
            <thead>
              <tr>
                <td>Название</td>
                <td>Дата создания</td>
                <td>Теги</td>
                <td>Синтаксис</td>
                <td>Наличие пароля</td>
              </tr>
            </thead>
            <tbody>
              {texts && (texts.items.map(t => {
                return (<TextRow text={t} getIcon={getIcon} key={t.id} includeOwner={false} />)
              }))}
            </tbody>
          </table>
        }
      </div>

      {texts &&
        <div className="pagination-controls">
          <button
            onClick={() => handlePageChange(currentPage - 1)}
            disabled={currentPage === 1}
          >
            Предыдущая страница
          </button>
          <span>Страница {currentPage} из {texts.totalPages}</span>
          <button
            onClick={() => handlePageChange(currentPage + 1)}
            disabled={texts ? currentPage >= texts.totalPages : false}
          >
            Следующая страница
          </button>
        </div>
      }
    </div>
  )
}

export default Profile