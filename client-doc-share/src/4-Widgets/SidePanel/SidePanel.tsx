import React, { useState } from 'react';
import './SidePanel.css';
import { Link } from 'react-router-dom';
import SectionTitle from '../Title/SectionTitle';

type Doc = {
  id: string,
  title: string,
  createdOn: Date,
}

const SidePanel: React.FC = () => {

  const russianPluralWords: { [Key: string]: string[] } = {
    'year': ['лет', 'года', 'год'],
    'month': ['месяцев', 'месяца', 'месяц'],
    'day': ['дней', 'дня', 'день'],
    'hour': ['часов', 'часа', 'час'],
    'minute': ['минут', 'минуты', 'минута'],
    'second': ['секунд', 'секунды', 'секунда']
  }

  const getTimeAgo = (d: Date): string => {
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

  const getRussianPlural = (num: number, period: string): string => {
    const pluralWords = russianPluralWords[period];
    if (Math.floor(num / 10) % 10 == 1)
      return pluralWords[0];
    if ([2, 3, 4].includes(Math.floor(num) % 10))
      return pluralWords[1];
    if (Math.floor(num) % 10 == 1)
      return pluralWords[2];
    return pluralWords[0];
  }

  const now: Date = new Date();
  const [myDocs, setMyDocs] = useState<Doc[]>([
    { id: "001", title: "Мой блог о ООП", createdOn: new Date(now.getTime() - 19 ** 1000) },
    { id: "002", title: "Мой блок о Socket", createdOn: new Date(now.getTime() - 12 * 60 * 1000) },
    { id: "001", title: "Мой блог о ООП", createdOn: new Date(now.getTime() - 19 ** 1000) },
    { id: "002", title: "Мой блок о Socket", createdOn: new Date(now.getTime() - 12 * 60 * 1000) },
    { id: "001", title: "Мой блог о ООП", createdOn: new Date(now.getTime() - 19 ** 1000) },
    { id: "002", title: "Мой блок о Socket", createdOn: new Date(now.getTime() - 12 * 60 * 1000) },
  ])
  const [societyDocs, setSocietyDocs] = useState<Doc[]>([
    { id: "003", title: "Об языке Typescript", createdOn: new Date(now.getTime() - 10 * 60 * 1000) },
    { id: "004", title: "Отличия мутекса от семафора", createdOn: new Date(now.getTime() - 11 * 60 * 1000) },
    { id: "005", title: "Как поступить в ВУЦ?", createdOn: new Date(now.getTime() - 14 * 60 * 1000) },
    { id: "006", title: "Теория струн электрогитары", createdOn: new Date(now.getTime() - 1 * 60 * 60 * 1000) },
    { id: "007", title: "Как выучить RUST и не перегореть?", createdOn: new Date(now.getTime() - 24 * 60 * 60 * 1000) },
  ])


  return (
    <div className="side-panel-body">
      {true &&
        <div className="my-texts">
          <SectionTitle title="Мои документы"/>
          <div className="text">
            {myDocs.length === 0 && "Вы еще не создали ни одного документа."}
            {myDocs.map(t => {
              return (
                <Link to={"/reader/" + encodeURIComponent(t.id)}>
                  <p>{t.title}</p>
                  <p>{getTimeAgo(t.createdOn)}</p>
                </Link>
              )
            })}
          </div>
        </div>
      }

      <div className="society-texts">
        <SectionTitle title="Документы сообщества"/>
        <div className="text">
          {societyDocs.length === 0 && "Еще ни один документ не был создан."}
          {societyDocs.map(t => {
            return (
              <Link to={"/reader/" + encodeURIComponent(t.id)}>
                <p>{t.title}</p>
                <p>{getTimeAgo(t.createdOn)}</p>
              </Link>
            )
          })}
        </div>
      </div>
    </div>
  );
};

export default SidePanel;
