import './ClientFormWidget.css';

type Props = {
  onSubmit: React.SubmitEventHandler<HTMLFormElement>,
  userName: string,
  onUserNameChange: React.ChangeEventHandler<HTMLInputElement, HTMLInputElement>,
}

export default function ClientFormWidget(props: Props) {
  return (
    <div className="client-form-card">
      <h2>Добро пожаловать!</h2>
      <p style={{ marginBottom: '1.5rem', color: '#718096' }}>
        Введите ваше имя, чтобы начать совместное редактирование документа
      </p>
      <form onSubmit={props.onSubmit}>
        <div className="client-form-group">
          <label htmlFor="name">Имя</label>
          <input
            type="text"
            id="name"
            value={props.userName}
            onChange={props.onUserNameChange}
            placeholder="Например, Иван"
            required
            autoFocus
          />
        </div>
        <button
          type="submit"
          className="client-form-submit"
        >
          Продолжить
        </button>
      </form>
    </div>
  )
}