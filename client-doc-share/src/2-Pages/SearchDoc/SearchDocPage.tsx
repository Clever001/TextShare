import CustomButton from "../../4-Widgets/CustomButton/CustomButton";
import SectionTitle from "../../4-Widgets/Title/SectionTitle";
import ValueInput, { type SelectionInfo } from "../../4-Widgets/ValueInput/ValueInput";

export function SearchDocPage() {

  const categoryOptions: SelectionInfo[] = [
    { htmlValue: "private", presentValue: "Приватный" },
    { htmlValue: "public", presentValue: "Публичный" },
    { htmlValue: "protected", presentValue: "Защищенный" },
  ];

  return (
    <div>
      <SectionTitle title="Название секции" />
      <CustomButton text={"Добавить документ"} target="button-check" color="green" rightIconUrl={null} />
      <ValueInput
        type="textarea"
        keyPosition="left"
        label="Описание"
        formSearchName="description"
        hint="Опишите назначение документа"
      />
      <ValueInput
        type="select"
        keyPosition="right"
        label="Тип доступа"
        formSearchName="access-type"
        hint="Выберите тип доступа к документу"
        possibleSelections={categoryOptions}
      />
      <ValueInput
        type="input"
        keyPosition="left"
        label="Название"
        formSearchName="doc-title"
        hint="Название документа"
      />
      <ValueInput
        type="checkbox"
        keyPosition="right"
        label="Наличие пароля"
        formSearchName="password-existance"
        hint=""
      />
      <p>Страница поиска документов</p>
    </div>
  )
}
