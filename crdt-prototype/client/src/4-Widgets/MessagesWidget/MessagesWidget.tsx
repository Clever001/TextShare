type Props = {
  messageInputRef: React.RefObject<HTMLInputElement | null>,
  onSendMessage: () => void,
  messages: string[]
}

export function MessagesWidget(props: Props) {
  return <>
    <div className="messages-block">
      <h2>Введите сообщение:</h2>
      <input ref={props.messageInputRef} type="text" />
      <button onClick={() => props.onSendMessage()}>Отправить сообщение</button>

      <div className="messages">
        {props.messages.length > 0 ?
          (props.messages.map((m, i) => {
            return (
              <div key={i}>
                <div className="message">{m}</div>
                <br />
              </div>
            )
          }))
          :
          (<div className="no-message" key={-1}>Нету сообщений</div>)
        }
      </div>
    </div>
  </>
}