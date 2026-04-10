import CustomButton from "../../4-Widgets/CustomButton/CustomButton";
import SectionTitle from "../../4-Widgets/Title/SectionTitle";
import ValueInput, { type SelectionInfo } from "../../4-Widgets/ValueInput/ValueInput";

export function SearchDocPage() {

  const categoryOptions: SelectionInfo[] = [
    { htmlValue: "docs", presentValue: "Документы" },
    { htmlValue: "apps", presentValue: "Заявления" },
    { htmlValue: "reports", presentValue: "Отчеты" },
  ];

  return (
    <div>
      <SectionTitle title="Название секции" />
      <CustomButton text={"Добавить документ"} target="button-check" color="green" rightIconUrl={null} />
      <ValueInput
        type="textarea"
        label="Описание"
        forVal="description"
        formSearchName="description"
        hint="Опишите назначение документа"
      />
      <ValueInput
        type="select"
        label="Тип доступа"
        forVal="access-type"
        formSearchName="access-type"
        hint="Выберите тип доступа к документу"
        possibleSelections={categoryOptions}
      />
      <ValueInput
        type="input"
        label="Название"
        forVal="doc-title"
        formSearchName="doc-title"
        hint="Название документа"
      />
      <ValueInput
        type="checkbox"
        label="Наличие пароля"
        forVal="password-existance"
        formSearchName="password-existance"
        hint=""
      />
      <p>Страница поиска документов</p>
    </div>
  )
}
