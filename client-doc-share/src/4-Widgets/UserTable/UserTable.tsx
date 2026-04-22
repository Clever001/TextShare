import "./UserTable.css"

type Props = {
  usernames: string[]
}

export default function UserTable({
  usernames
}: Props) {
  return <div className="user-table">
    {usernames.map((name) => {
      return <a className="user-row">
        <img src="/img/plain_user_black.svg" alt="user" />
        <span>{name}</span>
      </a>
    })}
  </div>
}