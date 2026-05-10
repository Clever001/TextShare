import "./ValueInput.css";

type BaseProps = {
  keyPosition: "left" | "right",
  label: string,
  formSearchName: string,
  hasRollbackButton: boolean,
  onRollback?: () => void | undefined
}

type Props = BaseProps & (
  | {
    widgetType: "textarea" | "input" | "password",
    hint?: string | undefined,
    defaultValue?: string | undefined,
  } | {
    widgetType: "select",
    possibleSelections: SelectionInfo[],
  } | {
    widgetType: "checkbox",
    defaultValue: string,
  } | {
    widgetType: "date",
  }
)

export type SelectionInfo = {
  htmlValue: string;
  presentValue: string;
};

export default function ValueInput(props: Props) {
  return (
    <div className="value-input">
      <p className={`key ${props.keyPosition}`}>{props.label}</p>
      {props.widgetType === "textarea" && (
        <div className="input-container">
          <textarea
            className="value"
            name={props.formSearchName}
            id={props.formSearchName}
            placeholder={props.hint}
            defaultValue={props.defaultValue}
          />
        </div>
      )}
      {props.widgetType === "select" && (
        <div className="input-container">
          <select name={props.formSearchName} id={props.formSearchName}>
            {props.possibleSelections.map((s) => {
              return (
                <option defaultValue={s.htmlValue}>{s.presentValue}</option>
              );
            })}
          </select>
        </div>
      )}
      {(props.widgetType === "input" || props.widgetType == "password") && (
        <div className="input-container">
          <input
            className="value"
            type={(props.widgetType == "input") ? "text" : "password"}
            name={props.formSearchName}
            id={props.formSearchName}
            placeholder={props.hint}
            defaultValue={props.defaultValue}
          />
        </div>
      )}
      {props.widgetType === "checkbox" && (
        <div className="input-container">
          <input
            className="value"
            type="checkbox"
            name={props.formSearchName}
            id={props.formSearchName}
            defaultValue={props.defaultValue}
          />
        </div>
      )}
      {props.widgetType === "date" && (
        <div className="input-container">
          <input 
            className="value" 
            type="date" 
            name={props.formSearchName}
            id={props.formSearchName}
            lang="ru"
            placeholder="дд.мм.гггг"

          />
          </div>
      )}
      {props.hasRollbackButton && (
        <button
          type="button"
          className="rollback"
          onClick={() => {
            props.onRollback && props.onRollback();
          }}
        >
          <img src="/img/undo.svg" alt="rollback" />
        </button>
      )}
    </div>
  );
}
