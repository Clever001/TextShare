import DocsTable, { type DocumentInfo } from "../../4-Widgets/DocsTable/DocsTable";
import Pagination from "../../4-Widgets/Pagination/Pagination";
import Search from "../../4-Widgets/Search/Search";
import SectionTitle from "../../4-Widgets/SectionTitle/SectionTitle";
import ValueInput from "../../4-Widgets/ValueInput/ValueInput";
import "./SearchDocF.css";

type Props = {
  isProfilePage: boolean,
};

export default function SearchDocF({
  isProfilePage
}: Props) {
  const docInfos: DocumentInfo[] = [
    { title: "Как поступить в ВУЦ", createdOn: new Date("2026-03-10T03:24:00"), creator: "Виталий", tags: ["ЮУрГУ", "ВУЦ", "Армия"], discription: "Некоторое короткое описание" },
    { title: "Календарь абитуриента ЮУрГУ", createdOn: new Date("2026-03-11T03:24:00"), creator: "Дмитрий", tags: ["ЮУрГУ", "Аббитуриентам", "ВУЗ"], discription: "Длинное описание. Длинное описание. Длинное описание. Длинное описание. Длинное описание. Длинное описание. Длинное описание. Длинное описание. Длинное описание. Длинное описание. Длинное описание. Длинное описание. Длинное описание. Длинное описание. " },
    { title: "Простые рецепты блюд", createdOn: new Date("2026-03-20T03:24:00"), creator: "Дарья", tags: ["Кулинария"], discription: "Jgbcfybt" },
    { title: "Популярные вопросы собеседований", createdOn: new Date("2026-03-20T04:24:00"), creator: "Артем", tags: ["тег", "еще_один_тег", "и_еще_тег", "очередной_тег", "тег"], discription: "Описание" },
  ]

  return <div className="doc-search-feature">
    <form className="doc-search-form">
      <Search formSearchName="doc-search" hint="Название документа" />
      <div className="search-params">
        <ValueInput type="input" hint="тег1 тег2 тег3 тег4" keyPosition="left"
          label="Теги" formSearchName="tags" hasRollbackButton={false} />
        <ValueInput type="input" keyPosition="left"
          label="Опубликован с даты" formSearchName="start-release-date" hasRollbackButton={false} />
        <ValueInput type="input" keyPosition="left"
          label="Опубликован по дату" formSearchName="end-release-date" hasRollbackButton={false} />
        {!isProfilePage && 
          <ValueInput type="input" keyPosition="left"
            label="Создатель" formSearchName="creator" hasRollbackButton={false} />
        }
      </div>
      <DocsTable docinfos={docInfos} showCreators={!isProfilePage} />
      <div className="pagination-container">
        <Pagination totalPages={5} initialPage={1} onChangePage={() => { }} />
      </div>
    </form>
  </div>;
}
