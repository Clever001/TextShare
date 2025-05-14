import React, { useContext, useEffect, useState } from 'react'
import "./SidePanel.css"
import { Link, useNavigate } from 'react-router-dom'
import { PaginatedResponseDto, PaginationDto, SortDto, TextFilterDto, TextWithoutContentDto } from '../../Dtos'
import { AuthContext } from '../../Context/AuthContext'
import Cookies from 'js-cookie'
import { SearchSocietyTextsAPI, SearchTextsAPI } from '../../Services/API/TextSearchService'

type Props = {}

const SidePanel = (props: Props) => {
  const authContext = useContext(AuthContext);
  if (!authContext) {
    throw new Error('AuthContext must be used within a AuthProvider');
  }
  const validAuth = authContext.validAuth;
  const setValidAuth = authContext.setValidAuth;
  const navigate = useNavigate();

  const [myTexts, setMyTexts] = useState<TextWithoutContentDto[]>([]);
  const [societyTexts, setSocietyTexts] = useState<TextWithoutContentDto[]>([]);

  const getMyTexts = async () => {
    if (validAuth) {
      const userName = Cookies.get("userName") ?? "";
      const token = Cookies.get("token") ?? "";
      if (userName === "" || token === "") {
        setValidAuth(false);
        setMyTexts([]);
        return;
      }

      const pagination: PaginationDto = {
        pageNumber: 1,
        pageSize: 3
      }

      const sort: SortDto = {
        sortBy: "createdOn",
        sortAscending: false
      }

      const filter: TextFilterDto = {
        ownerName: userName,
        title: null,
        tags: null,
        syntax: null,
        accessType: null,
        hasPassword: undefined
      }

      const result = await SearchTextsAPI(pagination, sort, filter, token);
      if (Array.isArray(result)) {
        if (result[0] === "Токен невалиден.") {
          setValidAuth(false);
        }
        setMyTexts([]);
        return;
      }

      setMyTexts(result.items);

      return;
    }
    setMyTexts([]);
  }

  useEffect(() => {
    getMyTexts();
  }, [validAuth]);

  const getSocietyTexts = async () => {
    const result = await SearchSocietyTextsAPI();

    if (result.length == 0) {
      setSocietyTexts([])
      return
    }

    if (typeof(result[0]) === "string") {
      console.log(result)
      return
    }

    setSocietyTexts(result as TextWithoutContentDto[])
  }

  useEffect(() => {
    getSocietyTexts();
  }, []);

  const russianPluralWords : {[Key: string] : string[]} = {
    'year': ['лет', 'года', 'год'],
    'month': ['месяцев', 'месяца', 'месяц'],
    'day': ['дней', 'дня', 'день'],
    'hour': ['часов', 'часа', 'час'],
    'minute': ['минут', 'минуты', 'минута'],
    'second': ['секунд', 'секунды', 'секунда']
  }

  const getRussianPlural = (num: number, period: string) :string => {
    const pluralWords = russianPluralWords[period];
    if (Math.floor(num / 10) % 10 == 1)
      return pluralWords[0];
    if ([2, 3, 4].includes(Math.floor(num) % 10))
      return pluralWords[1];
    if (Math.floor(num) % 10 == 1)
      return pluralWords[2];
    return pluralWords[0];
  }

  const getTimeAgo = (d: Date) :string => {
    const now = new Date();
    const ellapsed = now.getTime() - d.getTime();

    const seconds = Math.floor(ellapsed / 1000);
    const minutes = Math.floor(seconds / 60);
    const hours = Math.floor(minutes / 60);
    const days = Math.floor(hours / 24);
    const months = Math.floor(days / 30);
    const years = Math.floor(months / 12);

    var result:string = "";

    if (years > 0) {
      result = `${years} ${getRussianPlural(years, 'year')}`;
    } else if (months > 0) {
      result = `${months} ${getRussianPlural(months, 'month')}`;
    } else if (days > 0) {
      result = `${days} ${getRussianPlural(days, 'day')}`;
    } else if (hours > 0) {
      result = `${hours} ${getRussianPlural(hours, 'hour')}`;
    } else if (minutes > 0) {
      result = `${minutes} ${getRussianPlural(minutes, 'minute')}`;
    } else if (seconds > 0) {
      result = `${seconds} ${getRussianPlural(seconds, 'second')}`;
    }

    result += ' назад';
    return result;
  }

  const accessTypes : {[Key: string] : string} = {
    'ByReferencePublic' : 'Публичный',
    'ByReferenceAuthorized': 'С авторизацией',
    'OnlyFriends' : 'Для друзей',
    'Personal' : 'Приватный'
  }

  return (
    <div className="side-panel">
      {validAuth && 
        <div className="my-texts">
          <div className="title">Мои тексты</div>
          <div className="text">
            {myTexts.map(t => {
              return (
                <Link to={"/reader/" + encodeURIComponent(t.id)}>
                  <p>{t.title}</p>
                  <p>{(t.syntax !== "") ? t.syntax : "text"} | {getTimeAgo(t.createdOn)} | {accessTypes[t.accessType]}</p>
                </Link>
              )
            })}
          </div>
        </div>
      }

      <div className="society-texts">
        <div className="title">Тексты сообщества</div>
        <div className="text">
            {societyTexts.map(t => {
              return (
                <Link to={"/reader/" + encodeURIComponent(t.id)}>
                  <p>{t.title}</p>
                  <p>{(t.syntax !== "") ? t.syntax : "text"} | {getTimeAgo(t.createdOn)} | {accessTypes[t.accessType]}</p>
                </Link>
              )
            })}
          </div>
      </div>
    </div>
  )
}

export default SidePanel