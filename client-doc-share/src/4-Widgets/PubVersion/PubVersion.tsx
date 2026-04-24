import { dateToStringWithTime } from "../../6-Shared/utils";
import CustomButton from "../CustomButton/CustomButton";
import "./PubVersion.css";

type Props = {
  title: string;
  createdOn: Date;
  isPublished: boolean;
};

export default function PubVersion({ title, createdOn, isPublished }: Props) {
  return (
    <div className="pub-version">
      <div className="version-info">
        <div className="version-title-wrapper">
          <span className="version-title">{title}</span>
        </div>
        <span className="version-date">{dateToStringWithTime(createdOn)}</span>
      </div>
      <div className="version-action">
        {isPublished ? (
          <CustomButton
            leftIconUrl={null}
            rightIconUrl={null}
            text="Снять с публикации"
            color="yellow"
            type="button"
            target="remove"
          />
        ) : (
          <CustomButton
            leftIconUrl={null}
            rightIconUrl={null}
            text="Опубликовать"
            color="green"
            type="button"
            target="publish"
          />
        )}
      </div>
    </div>
  );
}
