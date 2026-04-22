import CustomButton from "../CustomButton/CustomButton"
import "./Comment.css"

type Props = {
  content: string,
  creatorName: string,
  elapsedTimeString: string
}

export default function Comment({
  content, creatorName, elapsedTimeString,
}: Props) {
  return <div className="comment">
    <textarea readOnly={true} className="content" value={content}/>
    <div className="subcontent">
      <div className="info">
        <img src="/img/plain_user_black.svg" alt="user_icon" />
        <div className="about">
          <p className="name">{creatorName}</p>
          <p className="elapsed">{elapsedTimeString}</p>
        </div>
      </div>
      <CustomButton leftIconUrl={null} rightIconUrl={null} text="Ответить"
        color="yellow" type="button" target="respond" />
    </div>
  </div>
}
