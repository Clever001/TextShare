import { Link } from "react-router-dom";
import "./RolesTable.css";

type Props = {
  userNames: string[];
  onDelete: (userName: string) => void;
};

export default function RolesTable({ userNames, onDelete }: Props) {
  type Role = {
    htmlValue: string;
    presentValue: string;
  };

  const possibleRoles: Role[] = [
    { htmlValue: "none", presentValue: "-" },
    { htmlValue: "writer", presentValue: "Редактор" },
    { htmlValue: "commentator", presentValue: "Комментатор" },
    { htmlValue: "reader", presentValue: "Читатель" },
  ];

  return (
    <div className="roles-table-container">
      <table className="roles-table">
        <thead>
          <tr>
            <td>Имя пользователя</td>
            <td>Роль доступа</td>
            <td></td>
          </tr>
        </thead>
        <tbody>
          {userNames.map((userName) => {
            return (
              <tr>
                <td>
                  <Link to="/">{userName}</Link>
                </td>
                <td>
                  <select>
                    {possibleRoles.map((r) => {
                      return (
                        <option value={r.htmlValue}>{r.presentValue}</option>
                      );
                    })}
                  </select>
                </td>
                <td>
                  <button
                    type="button"
                    onClick={() => {
                      onDelete(userName);
                    }}
                  >
                    <img src="/img/delete.svg" alt="delete" />
                  </button>
                </td>
              </tr>
            );
          })}
        </tbody>
      </table>
    </div>
  );
}
