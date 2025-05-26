import React, { useContext, useEffect, useState } from 'react'
import { getSyntaxes } from '../../Services/HelperService'
import './TextSearch.css'
import { Link, useNavigate, useSearchParams } from 'react-router-dom'
import TextRow from '../../Components/TextRow/TextRow'
import { PaginatedResponseDto, PaginationDto, SortDto, TextFilterDto, TextWithoutContentDto } from '../../Dtos'
import Cookies from 'js-cookie'
import { SearchTextsAPI } from '../../Services/API/TextAPIService'
import { AuthContext } from '../../Context/AuthContext'
import { isExceptionDto } from '../../Services/ErrorHandler'

type Props = {}

const TextSearch = (props: Props) => {
  const [searchParams, setSearchParams] = useSearchParams();
  const query = searchParams.get("title") ?? "";
  const [texts, setTexts] = useState<PaginatedResponseDto<TextWithoutContentDto> | null>(null);
  const [currentPage, setCurrentPage] = useState<number>(toNumber(searchParams.get("pageNumber")));

  var filter: TextFilterDto = {
    ownerName: searchParams.get("ownerName"),
    title: query,
    tags: (searchParams.get("tags") == "") ? null : searchParams.get("tags")?.split(" ") ?? null,
    syntax: searchParams.get("syntax"),
    accessType: searchParams.get("accessType"),
    hasPassword: toBoolean(searchParams.get("hasPassword"))
  }

  function toBoolean(value: string | null): boolean | null {
    if (value === "true") {
      return true;
    }
    if (value === "false") {
      return false;
    }
    return null;
  }

  function toNumber(value: string | null): number {
    if (!value) return 1;
    try {
      return Number.parseInt(value)
    } catch (parseError) {
      return 1;
    }
  }

  const navigate = useNavigate();
  const authContext = useContext(AuthContext);
  if (!authContext) {
    throw new Error('AuthContext must be used within a AuthProvider');
  }
  const setValidAuth = authContext.setValidAuth;


  useEffect(() => {
    const search = async () => {
      const pagination: PaginationDto = {
        pageNumber: toNumber(searchParams.get("pageNumber")),
        pageSize: 10
      }

      const sort: SortDto = {
        sortBy: "title",
        sortAscending: true
      }

      await performSearch(pagination, sort);
    }

    search()
  }, [query])

  const onSearchSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    const formData = new FormData(e.currentTarget);

    const getFormValue = (s: string): string | null => {
      var data = formData.get(s)?.toString() || null;
      if (typeof (data) === "string" && data === "") {
        data = null;
      }
      return data;
    }

    const newFilter: TextFilterDto = {
      ownerName: getFormValue("ownerName"),
      title: getFormValue("title"),
      tags: formData.get("tags")?.toString().split(" ") || null,
      syntax: getFormValue("syntax"),
      accessType: getFormValue("accessType"),
      hasPassword:
        formData.get("hasPassword")?.toString() === "true"
          ? true
          : formData.get("hasPassword")?.toString() === "false"
            ? false
            : null,
    };
    if (Array.isArray(newFilter.tags) && newFilter.tags[0] === "") {
      newFilter.tags = null;
    }

    filter = newFilter;
    setCurrentPage(1);

    const pagination: PaginationDto = {
      pageNumber: 1,
      pageSize: 10
    }

    const sort: SortDto = {
      sortBy: "title",
      sortAscending: true
    }

    await performSearch(pagination, sort);
  }

  const performSearch = async (pagination: PaginationDto, sort: SortDto) => {
    setSearchParams({
      ownerName: filter.ownerName ?? "",
      title: filter.title ?? "",
      tags: filter.tags?.join("") ?? "",
      syntax: filter.syntax ?? "",
      accessType: filter.accessType ?? "",
      hasPassword: (filter.hasPassword ?? "").toString(),
      pageNumber: pagination.pageNumber.toString()
    });

    var token = Cookies.get("token");

    const result = token ? await SearchTextsAPI(pagination, sort, filter, token) : await SearchTextsAPI(pagination, sort, filter, null);
    if (isExceptionDto(result)) {
      Cookies.remove("userId");
      Cookies.remove("userName");
      Cookies.remove("email");
      Cookies.remove("token");
      setValidAuth(false);
      navigate("/auth")
      return;
    }

    setTexts(result);
  }

  const accessIcons: { [Key: string]: string } = {
    "Personal": "👤",
    "ByReferencePublic": "🌐",
    "ByReferenceAuthorized": "🛡️",
    "OnlyFriends": "👫",
  };

  const getIcon = (accessType: string): string => {
    return accessIcons[accessType] || "❓";
  };

  function handlePageChange(newPage: number): void {
    if (newPage < 1 || (texts && newPage > texts.totalPages)) {
      return;
    }
    setCurrentPage(newPage);

    const pagination: PaginationDto = {
      pageNumber: newPage,
      pageSize: 10
    }

    const sort: SortDto = {
      sortBy: "title",
      sortAscending: true
    }

    performSearch(pagination, sort);
  }

  return (
    <div className="text-search">
      <div className="header">
        <form onSubmit={onSearchSubmit}>
          <div className="title">Поиск на сайте</div>
          <div className="search-bar">
            <table>
              <tbody>
                <tr>
                  <td className="col1"><input type="text" name="title" id="" defaultValue={query ? query : ""} /></td>
                  <td><button type="submit">Искать</button></td>
                </tr>
              </tbody>
            </table>
          </div>
          <div className="filters">
            <table>
              <tbody>
                <tr>
                  <td className="col1"><p>Автор</p></td>
                  <td className="col2"><input type="text" name="ownerName" defaultValue={filter.ownerName ?? ""} /></td>
                </tr>
                <tr>
                  <td className="col1"><p>Теги</p></td>
                  <td className="col2"><input type="text" name="tags" defaultValue={filter.tags?.join(" ") ?? ""}/></td>
                </tr>
                <tr>
                  <td className="col1"><p>Тип синтаксиса</p></td>
                  <td className="col2"><select name="syntax" defaultValue={filter.syntax ?? ""}>
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
                    <select name="accessType" defaultValue={filter.accessType ?? ""}>
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
                    <select name="hasPassword" defaultValue={filter.hasPassword?.toString() ?? ""}>
                      <option value=""></option>
                      <option value="true">Есть пароль</option>
                      <option value="false">Нет пароля</option>
                    </select>
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
        </form>
      </div>


      <div className="result">
        <div className="title">Результаты поиска</div>
        {texts ?
          <table>
            <thead>
              <tr>
                <td>Название</td>
                <td>Автор</td>
                <td>Дата создания</td>
                <td>Теги</td>
                <td>Синтаксис</td>
                <td>Наличие пароля</td>
              </tr>
            </thead>
            <tbody>
              {texts && (texts.items.map(t => {
                return (<TextRow text={t} getIcon={getIcon} key={t.id} includeOwner={true}></TextRow>)
              }))}
            </tbody>
          </table>
          :
          "Нет данных!"
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

export default TextSearch