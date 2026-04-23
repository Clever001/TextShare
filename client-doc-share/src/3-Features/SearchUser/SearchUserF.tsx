import Pagination from "../../4-Widgets/Pagination/Pagination";
import Search from "../../4-Widgets/Search/Search";
import SectionTitle from "../../4-Widgets/SectionTitle/SectionTitle";
import UserTable from "../../4-Widgets/UserTable/UserTable";
import "./SearchUserF.css";

type Props = {};

export default function SearchUserF(props: Props) {
  const userNames: string[] = [
    "CoolUserName1",
    "SomeCoolName",
    "VeryMuchUltraName",
    "AnotherCoolUserName",
  ];

  return (
    <div className="search-user-feature">
      <SectionTitle title="Поиск пользователей на сайте" />
      <Search formSearchName="userName" hint="Введите имя пользователя" />
      <SectionTitle title="Результат поиска" />
      <UserTable usernames={userNames} />
      <div className="pagination-container">
        <Pagination totalPages={10} initialPage={1} onChangePage={() => {}} />
      </div>
    </div>
  );
}
