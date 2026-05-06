import React, { useCallback, useContext, useEffect, useMemo, useState } from "react";
import "./SidePanel.css";
import { Link } from "react-router-dom";
import SectionTitle from "../SectionTitle/SectionTitle";
import type { ShortDocumentDto } from "../../6-Shared/ApiClient";
import { AuthContext } from "../../1-Processes/AuthContext";
import { generateDocumentApi, generateDocumentApiAuth } from "../../6-Shared/utils";

const SidePanel: React.FC = () => {
  const russianPluralWords = useMemo<{ [Key: string]: string[] }>(() => {
    return {
      year: ["лет", "года", "год"],
      month: ["месяцев", "месяца", "месяц"],
      day: ["дней", "дня", "день"],
      hour: ["часов", "часа", "час"],
      minute: ["минут", "минуты", "минута"],
      second: ["секунд", "секунды", "секунда"],
    }
  }, [])

  const getRussianPlural = useCallback((num: number, period: string): string => {
    const pluralWords = russianPluralWords[period];
    if (Math.floor(num / 10) % 10 == 1) return pluralWords[0];
    if ([2, 3, 4].includes(Math.floor(num) % 10)) return pluralWords[1];
    if (Math.floor(num) % 10 == 1) return pluralWords[2];
    return pluralWords[0];
  }, [russianPluralWords])

  const getTimeAgo = useCallback((d: Date): string => {
    const now = new Date();
    const ellapsed = now.getTime() - d.getTime();

    const seconds = Math.floor(ellapsed / 1000);
    const minutes = Math.floor(seconds / 60);
    const hours = Math.floor(minutes / 60);
    const days = Math.floor(hours / 24);
    const months = Math.floor(days / 30);
    const years = Math.floor(months / 12);

    var result: string = "";

    if (years > 0) {
      result = `${years} ${getRussianPlural(years, "year")}`;
    } else if (months > 0) {
      result = `${months} ${getRussianPlural(months, "month")}`;
    } else if (days > 0) {
      result = `${days} ${getRussianPlural(days, "day")}`;
    } else if (hours > 0) {
      result = `${hours} ${getRussianPlural(hours, "hour")}`;
    } else if (minutes > 0) {
      result = `${minutes} ${getRussianPlural(minutes, "minute")}`;
    } else if (seconds > 0) {
      result = `${seconds} ${getRussianPlural(seconds, "second")}`;
    }

    result += " назад";
    return result;
  }, [getRussianPlural])

  const [myDocs, setMyDocs] = useState<ShortDocumentDto[]>([]);
  const [societyDocs, setSocietyDocs] = useState<ShortDocumentDto[]>([]);

  const authContext = useContext(AuthContext)
  if (!authContext) {
    throw new Error("Auth Context cannot be undefined")
  }
  const isAuthenticated = authContext.isAuthenticated
  const getUserInfo = authContext.getUserInfo

  const getPersonalDocuments = useCallback(async (userId: string) => {
    console.log("Started Personal loading")
    const docApi = generateDocumentApi()

    try {
      const { data } = await docApi.searchDocuments(
        undefined, // sortBy
        undefined, // sortAscending
        1,         // pageNumber
        5,         // pageSize
        undefined, // title
        undefined, // tags
        undefined, // fromDate
        undefined, // toDate
        undefined, // ownerName
        userId     // ownerId
      )
      console.log("Finished Personal loading")
      setMyDocs(data.items)
    } catch (err) {

    }
  }, [])

  const getLatestsDocuments = useCallback(async () => {
    console.log("Started latests loading")
    const docApi = generateDocumentApi()

    try {
      const { data } = await docApi.searchDocuments(
        undefined, // sortBy
        undefined, // sortAscending
        1,         // pageNumber
        5,         // pageSize
        undefined, // title
        undefined, // tags
        undefined, // fromDate
        undefined, // toDate
        undefined, // ownerName
        undefined  // ownerId
      )
      console.log("Finished latests loading")
      setSocietyDocs(data.items)
    } catch (err) {

    }
  }, [])

  useEffect(() => {
    console.log("Started docs loading")
    const userInfo = getUserInfo()
    if (userInfo) {
      getPersonalDocuments(userInfo.id)
    }
    getLatestsDocuments()
  }, [isAuthenticated, getUserInfo])

  return (
    <div className="side-panel-body">
      {isAuthenticated && (
        <div className="my-texts">
          <SectionTitle title="Мои документы" />
          <div className="text">
            {myDocs.length === 0 && "Вы еще не создали ни одного документа."}
            {myDocs.map((t) => {
              return (
                <Link to={"/view/" + encodeURIComponent(t.id)}>
                  <p>{t.title}</p>
                  <p>{getTimeAgo(new Date(t.createdOn))}</p>
                </Link>
              );
            })}
          </div>
        </div>
      )}

      <div className="society-texts">
        <SectionTitle title="Документы сообщества" />
        <div className="text">
          {societyDocs.length === 0 && "Еще ни один документ не был создан."}
          {societyDocs.map((t) => {
            return (
              <Link to={"/view/" + encodeURIComponent(t.id)}>
                <p>{t.title}</p>
                <p>{getTimeAgo(new Date(t.createdOn))}</p>
              </Link>
            );
          })}
        </div>
      </div>
    </div>
  );
};

export default SidePanel;
