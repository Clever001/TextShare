import DocVersion from "../../4-Widgets/DocVersion/DocVersion";
import EditorWidget from "../../4-Widgets/Editor/EditorWidget";
import "./VersionF.css";

type Props = {};

export default function VersionF(props: Props) {
  return (
    <div className="version-feature">
      <div className="preview">
        <div className="preview-title">
          <p className="title">Просмотр содержимого версии</p>
          <p className="suptitle">Выберите версию для просмотра</p>
        </div>
        <EditorWidget />
      </div>
      <div className="versions">
        <DocVersion
          title="Версия 3"
          createdOn={new Date("2026-04-01 17:30:02")}
        />
        <DocVersion
          title="Версия 2"
          createdOn={new Date("2026-04-01 17:30:02")}
        />
        <DocVersion
          title="Версия 1"
          createdOn={new Date("2026-04-01 17:30:02")}
        />
      </div>
    </div>
  );
}
