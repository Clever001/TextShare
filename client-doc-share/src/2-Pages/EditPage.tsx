import Bookmark from "../4-Widgets/Bookmark/Bookmark";
import EditorPanel from "../4-Widgets/EditorPanel/EditorPanel";

export default function EditPage() {
  return <div>
    <p>Страница редактирования документа</p>
    <Bookmark text="Редактирование" isActive={false} onClick={() => {}} />
    <Bookmark text="Версионирование" isActive={true} onClick={() => {}} />
    <EditorPanel />
  </div>
}
