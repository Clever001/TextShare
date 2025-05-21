import React, { useEffect, useState } from 'react'
import { PaginatedResponseDto, PaginationDto, ProcessFriendRequestDto, UserWithoutTokenDto } from '../../Dtos'
import './UserSearch.css'
import { DeleteFriendAPI, DeleteFriendRequestAPI, GetFriendsAPI, GetRecievedFriendRequestsAPI, GetSentFriendRequestsAPI, GetUsersAPI, ProcessFriendRequestAPI, SendFriendRequestAPI } from '../../Services/API/AccountAPIService'
import { isExceptionDto } from '../../Services/ErrorHandler'
import Cookies from 'js-cookie'
import { Link, useNavigate, useSearchParams } from 'react-router-dom'

type Props = {}

const UserSearch = (props: Props) => {
  const [searchParams, setSearchParams] = useSearchParams();
  const navigate = useNavigate();

  const [users, setUsers] = useState<PaginatedResponseDto<string> | null>(null);
  const [errors, setErrors] = useState<string[]>([]);
  const [currentPage, setCurrentPage] = useState<number>(1);
  const [searchType, setSearchType] = useState<string>("users");

  const PAGE_SIZE = 10;
  var _pagination: PaginationDto = {
    pageNumber: 1,
    pageSize: PAGE_SIZE
  }
  var _searchType: string = searchParams.get("searchType") ?? "users";
  var _searchName: string | null = searchParams.get("searchName");

  useEffect(() => {
    setSearchType(_searchType);

    performSearch();
  }, []);

  const onCurrentPageChange = (newPage: number) => {
    if (newPage < 1 || users && newPage > users.totalPages) {
      return;
    }

    _pagination.pageNumber = newPage;
    setCurrentPage(newPage);

    performSearch();
  }

  const onSearchSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setErrors([]);
    const formData = new FormData(e.currentTarget);

    _pagination = {
      pageNumber: 1,
      pageSize: PAGE_SIZE
    }
    setCurrentPage(1);

    _searchType = formData.get("searchType")?.toString() ?? "users";
    setSearchType(_searchType);
    _searchName = formData.get("searchName")?.toString() ?? null;

    setSearchParams({
      searchType: _searchType,
      searchName: _searchName ?? ""
    })

    performSearch();
  }

  const onDeleteFriendClick = async (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
    setErrors([]);
    const token = Cookies.get("token");
    if (!token) {
      alert("Перед выполнением действия необходимо сначала зарегистрироваться в системе.");
      navigate("/auth");
      return;
    }
    const response = await DeleteFriendAPI(token, e.currentTarget.value);

    if (isExceptionDto(response)) {
      setErrors([response.description]);
      return;
    }

    performSearch();
  }

  const onDeleteRequestClick = async (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
    setErrors([]);
    const token = Cookies.get("token");
    if (!token) {
      alert("Перед выполнением действия необходимо сначала зарегистрироваться в системе.");
      navigate("/auth");
      return;
    }
    const response = await DeleteFriendRequestAPI(token, e.currentTarget.value);

    if (isExceptionDto(response)) {
      setErrors([response.description]);
      return;
    }

    performSearch();
  }

  const onApproveRequestClick = async (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
    setErrors([]);
    const token = Cookies.get("token");
    if (!token) {
      alert("Перед выполнением действия необходимо сначала зарегистрироваться в системе.");
      navigate("/auth");
      return;
    }
    const dto: ProcessFriendRequestDto = {
      acceptRequest: true
    }
    const response = await ProcessFriendRequestAPI(token, e.currentTarget.value, dto);

    if (isExceptionDto(response)) {
      setErrors([response.description]);
      return;
    }

    performSearch();
  }

  const onRejectRequestClick = async (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
    setErrors([]);
    const token = Cookies.get("token");
    if (!token) {
      alert("Перед выполнением действия необходимо сначала зарегистрироваться в системе.");
      navigate("/auth");
      return;
    }
    const dto: ProcessFriendRequestDto = {
      acceptRequest: false
    }
    const response = await ProcessFriendRequestAPI(token, e.currentTarget.value, dto);

    if (isExceptionDto(response)) {
      setErrors([response.description]);
      return;
    }

    performSearch();
  }

  const performSearch = async () => {
    switch (_searchType) {
      case "users":
        await findUsers(_pagination, _searchName);
        return;
      case "friends":
        await findFriends(_pagination, _searchName);
        return;
      case "sentRequests":
        await findSentFriendRequests(_pagination, _searchName);
        return;
      default:
        await findRecievedFriendRequests(_pagination, _searchName);
        return;
    }
  }

  const findUsers = async (pagination: PaginationDto, searchName: string | null) => {
    const response = await GetUsersAPI(pagination, searchName);

    if (isExceptionDto(response)) {
      setErrors([response.description]);
      setUsers(null);
      return;
    }

    const users: PaginatedResponseDto<string> = {
      items: response.items.map(u => u.userName),
      totalItems: response.totalItems,
      totalPages: response.totalPages,
      currentPage: response.currentPage,
      pageSize: response.pageSize
    }

    setUsers(users);
  }

  const findFriends = async (pagination: PaginationDto, searchName: string | null) => {
    const token = Cookies.get("token");
    if (!token) {
      setErrors(["Только авторизованный пользователь может посмотреть список своих друзей."]);
      setUsers(null);
      return;
    }

    const response = await GetFriendsAPI(pagination, true, searchName, token);

    if (isExceptionDto(response)) {
      setErrors([response.description]);
      setUsers(null);
      return;
    }

    const users: PaginatedResponseDto<string> = {
      items: response.items.map(u => u.userName),
      totalItems: response.totalItems,
      totalPages: response.totalPages,
      currentPage: response.currentPage,
      pageSize: response.pageSize
    }

    setUsers(users);
  }

  const findSentFriendRequests = async (pagination: PaginationDto, searchName: string | null) => {
    const token = Cookies.get("token");
    if (!token) {
      setErrors(["Только авторизованный пользователь может посмотреть список отправленных запросов в друзья."]);
      setUsers(null);
      return;
    }

    const response = await GetSentFriendRequestsAPI(pagination, true, searchName, token);
    if (isExceptionDto(response)) {
      setErrors([response.description]);
      setUsers(null);
      return;
    }

    const users: PaginatedResponseDto<string> = {
      items: response.items.map(u => u.recipientName),
      totalItems: response.totalItems,
      totalPages: response.totalPages,
      currentPage: response.currentPage,
      pageSize: response.pageSize
    }

    setUsers(users);
  }

  const findRecievedFriendRequests = async (pagination: PaginationDto, searchName: string | null) => {
    const token = Cookies.get("token");
    if (!token) {
      setErrors(["Только авторизованный пользователь может посмотреть список полученных запросов в друзья."]);
      setUsers(null);
      return;
    }

    const response = await GetRecievedFriendRequestsAPI(pagination, true, searchName, token);
    if (isExceptionDto(response)) {
      setErrors([response.description]);
      setUsers(null);
      return;
    }

    const users: PaginatedResponseDto<string> = {
      items: response.items.map(u => u.senderName),
      totalItems: response.totalItems,
      totalPages: response.totalPages,
      currentPage: response.currentPage,
      pageSize: response.pageSize
    }

    setUsers(users);
  }


  return (
    <div className="user-search">
      {errors && errors.map(e => {
        return (<div className="error">{e}</div>)
      })}
      <form onSubmit={onSearchSubmit}>
        <div className="title">Поиск пользователей на сайте</div>
        <div className="search-bar">
          <table>
            <tbody>
              <tr>
                <td className="col1"><input type="text" name="searchName" defaultValue={_searchName || ""} /></td>
                <td><button type="submit">Искать</button></td>
              </tr>
            </tbody>
          </table>
        </div>
        <div className="filters">
          <table>
            <tbody>
              <tr>
                <td className="col1">
                  <p>Тип поиска</p>
                </td>
                <td className="col2">
                  <select name="searchType" defaultValue={_searchType}>
                    <option value="users">Поиск пользователей</option>
                    <option value="friends">Поиск друзей</option>
                    <option value="sentRequests">Отправленные запросы</option>
                    <option value="recievedRequests">Полученные запросы</option>
                  </select>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </form>

      <div className="result">
        <div className="title">Результаты поиска</div>
        {(!users || users.totalItems === 0) &&
          <p>Результат поиска пуст!</p>
        }
        <table>
          <tbody>
            {users && users.items.map(userName => {
              return (
                <tr>
                  <td className="myrow first-row">
                    <Link to={`/profile/${userName}`} className="icon"><img src="img/user_icon_black.svg" alt="user" /></Link>
                    <Link to={`/profile/${userName}`} className="userName">{userName}</Link>
                  </td>
                  {searchType === "friends" &&
                    <td>
                      <div className="buttons-container">
                        <button onClick={onDeleteFriendClick} value={userName}>Удалить из друзей</button>
                      </div>
                    </td>
                  }
                  {searchType === "sentRequests" &&
                    <td>
                      <div className="buttons-container">
                        <button onClick={onDeleteRequestClick} value={userName}>Удалить запрос в друзья</button>
                      </div>
                    </td>
                  }
                  {searchType === "recievedRequests" &&
                    <td>
                      <div className="buttons-container">
                        <button onClick={onApproveRequestClick} value={userName}>Одобрить запрос в друзья</button>
                        <button onClick={onRejectRequestClick} value={userName}>Отвергнуть запрос в друзья</button>
                      </div>
                    </td>
                  }
                </tr>
              )
            })}
          </tbody>
        </table>
      </div>

      {users && users.totalItems !== 0 &&
        <div className="pagination-controls">
          <button
            onClick={() => onCurrentPageChange(currentPage - 1)}
            disabled={currentPage === 1}
          >
            Предыдущая страница
          </button>
          <span>Страница {currentPage} из {users.totalPages}</span>
          <button
            onClick={() => onCurrentPageChange(currentPage + 1)}
            disabled={users ? currentPage >= users.totalPages : false}
          >
            Следующая страница
          </button>
        </div>
      }
    </div>
  )
}

export default UserSearch