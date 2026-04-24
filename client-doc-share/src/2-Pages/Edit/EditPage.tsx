import { useState } from "react";
import Bookmark from "../../4-Widgets/Bookmark/Bookmark";
import "./EditPage.css";
import EditF from "../../3-Features/Edit/EditF";
import VersionF from "../../3-Features/Version/VersionF";
import PublishF from "../../3-Features/Publish/PublishF";
import DocSettingsF from "../../3-Features/DocSettings/DocSettingsF";

export default function EditPage() {
  const pageStates: string[] = [
    "Редактирование",
    "Версионирование",
    "Публикация",
    "Настройки",
  ];

  const [activeState, setActiveState] = useState<string>(pageStates[0]);

  return (
    <div className="edit-page">
      <div className="editor-tabs">
        {pageStates.map((s) => {
          return (
            <Bookmark
              text={s}
              isActive={activeState == s}
              onClick={() => {
                setActiveState(s);
              }}
            />
          );
        })}
      </div>
      {activeState == "Редактирование" && <EditF />}
      {activeState == "Версионирование" && <VersionF />}
      {activeState == "Публикация" && <PublishF />}
      {activeState == "Настройки" && <DocSettingsF />}
    </div>
  );
}
