import EditorWidget from "../../4-Widgets/Editor/EditorWidget";
import EditorPanel from "../../4-Widgets/EditorPanel/EditorPanel";
import CommentsF from "../Comments/CommentsF";
import "./EditF.css";

type Props = {};

export default function EditF(props: Props) {
  return (
    <div className="edit-feature">
      <EditorPanel />
      <EditorWidget />
      <CommentsF />
    </div>
  );
}
