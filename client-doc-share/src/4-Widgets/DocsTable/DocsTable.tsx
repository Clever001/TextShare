import { Link } from "react-router-dom";
import "./DocsTable.css";

type Props = {
  docinfos: DocumentInfo[];
  showCreators: boolean;
};

export type DocumentInfo = {
  title: string;
  createdOn: Date;
  creator: string;
  tags: string[];
  discription: string;
};

export default function DocsTable({ docinfos, showCreators }: Props) {
  const dateToString = (d: Date): string => {
    var day: string = d.getDate().toString();
    if (day.length == 1) {
      day = "0" + day;
    }

    var month: string = (d.getMonth() + 1).toString();
    if (month.length == 1) {
      month = "0" + month;
    }

    return `${day}.${month}.${d.getFullYear()}`;
  };

  const tagsToShortTags = (tags: string[]): string => {
    const resultBuilder: string[] = [];
    var totalLength: number = 0;
    for (var i = 0; i != tags.length; i++) {
      resultBuilder.push(tags[i]);
      totalLength += tags[i].length;
      if (totalLength > 20 && i + 1 != tags.length) {
        resultBuilder.push("...");
        break;
      }
    }

    return resultBuilder.join(" ");
  };

  return (
    <div className="table-container">
      <table className="docs-table">
        <thead>
          <tr>
            <td>
              <p>Название</p>
            </td>
            <td>
              <p>Дата создания</p>
            </td>
            {showCreators && (
              <td>
                <p>Создатель</p>
              </td>
            )}
            <td>
              <p>Теги</p>
            </td>
          </tr>
        </thead>
        <tbody>
          {docinfos.map((di) => {
            return (
              <tr>
                <td>
                  <Link to="/" title={di.discription}>
                    {di.title}
                  </Link>
                </td>
                <td>
                  <Link to="/" title={di.createdOn.toDateString()}>
                    {dateToString(di.createdOn)}
                  </Link>
                </td>
                {showCreators && (
                  <td>
                    <Link to="/" title={di.creator}>
                      {di.creator}
                    </Link>
                  </td>
                )}
                <td>
                  <Link to="/" title={di.tags.join(" ")}>
                    {tagsToShortTags(di.tags)}
                  </Link>
                </td>
              </tr>
            );
          })}
        </tbody>
      </table>
    </div>
  );
}
