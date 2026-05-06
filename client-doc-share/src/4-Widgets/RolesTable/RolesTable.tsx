import { Link } from "react-router-dom";
import "./RolesTable.css";
import { UserDevRole, type UserWithoutTokenDto } from "../../6-Shared/ApiClient";

type Props = {
  users: {user: UserWithoutTokenDto, role: UserDevRole}[];
  onRoleChange: (userId: string, newRole: UserDevRole) => void;
  onDelete: (userId: string) => void;
};

export default function RolesTable({ users, onRoleChange, onDelete }: Props) {
  // type Role = {
  //   htmlValue: string;
  //   presentValue: string;
  // };

  // const possibleRoles: Role[] = [
  //   { htmlValue: "none", presentValue: "-" },
  //   { htmlValue: "writer", presentValue: "Редактор" },
  //   { htmlValue: "commentator", presentValue: "Комментатор" },
  //   { htmlValue: "reader", presentValue: "Читатель" },
  // ];

  const possibleRoles = [
    {role: UserDevRole.Reader, presentValue: "Читатель"},
    {role: UserDevRole.Commentor, presentValue: "Комментатор"},
    {role: UserDevRole.Editor, presentValue: "Редактор"},
    {role: UserDevRole.Administrator, presentValue: "Администратор"}
  ]

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
          {users.map((u) => {
            return (
              <tr key={u.user.id}>
                <td>
                  <Link to="/">{u.user.userName}</Link>
                </td>
                <td>
                  <select value={u.role}
                  onChange={(e) => {onRoleChange(u.user.id, e.target.value as UserDevRole)}}>
                    {possibleRoles.map((r) => {
                      return (
                        <option value={r.role}>{r.presentValue}</option>
                      );
                    })}
                  </select>
                </td>
                <td>
                  <button
                    type="button"
                    onClick={() => {
                      onDelete(u.user.id);
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
