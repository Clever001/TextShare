import { dateToStringWithTime } from "../../6-Shared/utils";
import "./DocVersion.css";

type Props = {
  title: string;
  createdOn: Date;
};

export default function DocVersion({ title, createdOn }: Props) {
  return (
    <div className="doc-version">
      <div className="version-info">
        <div className="version-title-wrapper">
          <span className="version-title">{title}</span>
          <button
            className="title-edit-btn"
            title="Изменить название"
            id="edit"
          >
            <img src="/img/edit_2_white.svg" alt="edit" />
          </button>
        </div>
        <span className="version-date">{dateToStringWithTime(createdOn)}</span>
      </div>
      <div className="version-action">
        <button id="look">
          <img src="/img/take_a_look_white.svg" alt="look" />
        </button>
        <button id="delete">
          <img src="/img/delete_white.svg" alt="delete" />
        </button>
        <button id="accept">
          <img src="/img/accept_white.svg" alt="accept" />
        </button>
      </div>
    </div>
  );
}
