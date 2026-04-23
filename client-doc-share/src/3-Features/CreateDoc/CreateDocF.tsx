import CustomButton from "../../4-Widgets/CustomButton/CustomButton";
import RolesTable from "../../4-Widgets/RolesTable/RolesTable";
import Search from "../../4-Widgets/Search/Search";
import SectionTitle from "../../4-Widgets/SectionTitle/SectionTitle";
import ValueInput from "../../4-Widgets/ValueInput/ValueInput";
import "./CreateDocF.css";

type Props = {};

export default function CreateDocF(props: Props) {
  const users: string[] = [
    "Виталий Никончук",
    "Андрей Желудин",
    "Виктор Карпов",
    "Семен Котов",
  ];

  return (
    <form className="create-doc-feature">
      <SectionTitle title="Создание пустого документа" />
      <div className="settings-container">
        <ValueInput
          type="input"
          keyPosition="left"
          label="Название"
          formSearchName="name"
          hasRollbackButton={false}
        />
        <ValueInput
          type="input"
          keyPosition="left"
          label="Теги"
          formSearchName="tags"
          hint="первый_тег второй_тег"
          hasRollbackButton={false}
        />
        <ValueInput
          type="textarea"
          keyPosition="left"
          label="Описание"
          formSearchName="discription"
          hasRollbackButton={false}
        />
      </div>
      <SectionTitle title="Настройка ролей доступа (режим редактирования)" />
      <Search
        formSearchName="userName"
        hint="Введите имя пользователя"
        buttonText="Добавить пользователя"
      />
      <RolesTable userNames={users} onDelete={() => {}} />
      <CustomButton
        leftIconUrl={null}
        rightIconUrl={null}
        text="Создать документ"
        color="green"
        type="submit"
        target="create"
      />
    </form>
  );
}
