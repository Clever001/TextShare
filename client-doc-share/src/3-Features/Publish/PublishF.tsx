import EditorWidget from "../../4-Widgets/Editor/EditorWidget";
import PubVersion from "../../4-Widgets/PubVersion/PubVersion";
import "./PublishF.css";

type Props = {};

export default function PublishF(props: Props) {
  return (
    <div className="publish-feature">
      <div className="preview">
        <div className="preview-title">
          <p className="title">Просмотр опубликованной версии</p>
          <p className="suptitle">Опубликована версия: "Версия 3"</p>
        </div>
        <EditorWidget />
      </div>
      <div className="versions">
        <PubVersion
          title="Версия 3"
          createdOn={new Date("2026-04-01 17:30:02")}
          isPublished={true}
        />
        <PubVersion
          title="Версия 2"
          createdOn={new Date("2026-04-01 17:30:02")}
          isPublished={false}
        />
        <PubVersion
          title="Версия 1"
          createdOn={new Date("2026-04-01 17:30:02")}
          isPublished={false}
        />
      </div>
    </div>
  );
}
