import { dateToString } from "../../6-Shared/utils"
import "./DocTitle.css"

type Props = {
  docTitle: string,
  creatorName: string,
  createdOn: Date
}

export default function DocTitle({
  docTitle, creatorName, createdOn
}: Props) {
  return <div className="doc-title">
    <img src="/img/plain_user_black.svg" alt="user_icon" />
    <div className="text-content">
      <p className="title">{docTitle}</p>
      <div className="subtitle">
        <p className="creator-name">{creatorName}</p>
        <p className="created-on">{dateToString(createdOn)}</p>
      </div>
    </div>
  </div>
}