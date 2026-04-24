import ValueInput from "../ValueInput/ValueInput";
import "./DocSettingsTable.css";

type Props = {};

export default function DocSettingsTable(props: Props) {
  return (
    <form className="doc-settings-table">
      <ValueInput
        type="input"
        keyPosition="left"
        label="Название документа"
        formSearchName="title"
        hasRollbackButton={false}
      />
      <ValueInput
        type="input"
        keyPosition="left"
        label="Теги"
        formSearchName="tags"
        hasRollbackButton={false}
      />
      <ValueInput
        type="input"
        keyPosition="left"
        label="Описание"
        formSearchName="discription"
        hasRollbackButton={false}
      />
    </form>
  );
}
