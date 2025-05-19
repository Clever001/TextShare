import React, { useEffect, useState } from 'react'
import { PaginatedResponseDto, PaginationDto, SortDto, TextFilterWithoutNameDto, TextWithoutContentDto } from '../../Dtos'
import { getSyntaxes } from '../../Services/HelperService'
import TextRow from '../../Components/TextRow/TextRow'
import { useNavigate, useParams } from 'react-router-dom'
import Cookies from 'js-cookie'
import { SearchTextsByNameAPI } from '../../Services/API/TextAPIService'
import { isExceptionDto } from '../../Services/ErrorHandler'
import './Profile.css'

type Props = {}

const Profile = (props: Props) => {
  const navigate = useNavigate();

  const [errors, setErrors] = useState<string[]>([]);
  const [texts, setTexts] = useState<PaginatedResponseDto<TextWithoutContentDto> | null>(null);
  const { userName } = useParams();

  const [currentPage, setCurrentPage] = useState<number>(1);

  const PAGE_SIZE = 10;

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
    const token = Cookies.get("token") ?? null;

    if (!userName) {
      alert("Имя пользователя не может быть пустым.");
      navigate("/");
      return;
    }

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

    searchTexts(pagination, sort);

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

  return (
    <div className="profile">
      <div className="header">
        <div className="info">
          <img src="img/user_icon_black.svg" alt="user" />
          <p className="name">{userName}</p>
        </div>
        <div className="action-button">
          <button type="button">Добавить в друзья</button>
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