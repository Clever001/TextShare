import "./DocViewF.css";
import DocTitle from "../../4-Widgets/DocTitle/DocTitle";
import EditorWidget from "../../4-Widgets/Editor/EditorWidget";

type Props = {};

export default function DocViewF(props: Props) {
  return (
    <div className="doc-view-feature">
      <DocTitle
        docTitle="C Hello World"
        creatorName="UserNickname"
        createdOn={new Date("2026-04-30T07:44:00")}
      />
      <EditorWidget />
    </div>
  );
}
