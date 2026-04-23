import { dateToStringWithTime } from "../../6-Shared/utils";
import "./DocVersion.css";

type Props = {
  type: "editing" | "publication";
  title: string;
  createdOn: Date;
};

export default function DocVersion({ type, title, createdOn }: Props) {
  return (
    <div className="doc-version">
      <div className="version-info">
        {type == "editing" ? (
          <div className="version-title-wrapper">
            <span className="version-title">{title}</span>
            <button className="title-edit-btn" title="Изменить название">
              <img src="/img/edit.svg" alt="edit" />
            </button>
          </div>
        ) : (
          <div className="version-title-wrapper">
            <span className="version-title">{title}</span>
          </div>
        )}
        <span className="version-date">{dateToStringWithTime(createdOn)}</span>
      </div>
      <div className="version-action">
        <button>
          <img src="/img/take_a_look_white.svg" alt="look" />
        </button>
        <button>
          <img src="/img/delete_white.svg" alt="delete" />
        </button>
        <button>
          <img src="/img/accept_white.svg" alt="accept" />
        </button>
      </div>
    </div>
  );
}
