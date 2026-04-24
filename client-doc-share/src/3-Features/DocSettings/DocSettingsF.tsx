import CustomButton from "../../4-Widgets/CustomButton/CustomButton";
import DocSettingsTable from "../../4-Widgets/DocSettingsTable/DocSettingsTable";
import RolesTable from "../../4-Widgets/RolesTable/RolesTable";
import SectionTitle from "../../4-Widgets/SectionTitle/SectionTitle";
import "./DocSettingsF.css";

type Props = {};

export default function DocSettingsF(props: Props) {
  const userNames: string[] = [
    "Виталий Никончук",
    "Андрей Желудин",
    "Виктор Карпов",
    "Семен Котов",
  ];

  return (
    <form className="doc-settings-feature">
      <SectionTitle title="Основный настройки" />
      <DocSettingsTable />
      <SectionTitle title="Настройка ролей доступа" />
      <RolesTable userNames={userNames} onDelete={() => {}} />
      <div className="button-container">
        <CustomButton
          leftIconUrl={null}
          rightIconUrl={null}
          text="Создать документ"
          color="green"
          type="submit"
          target="save_settings"
        />
      </div>
    </form>
  );
}
