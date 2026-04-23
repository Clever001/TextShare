import Bookmark from "../4-Widgets/Bookmark/Bookmark";
import DocVersion from "../4-Widgets/DocVersion/DocVersion";
import EditorPanel from "../4-Widgets/EditorPanel/EditorPanel";

export default function EditPage() {
  return (
    <div>
      <p>Страница редактирования документа</p>
      <Bookmark text="Редактирование" isActive={false} onClick={() => {}} />
      <Bookmark text="Версионирование" isActive={true} onClick={() => {}} />
      <EditorPanel />
      <DocVersion
        type="editing"
        title="Версия 3"
        createdOn={new Date("2026-04-03 06:04:01")}
      />
    </div>
  );
}
