import "./ValueInput.css";

type Props = {
  type: "textarea" | "select" | "input" | "checkbox";
  keyPosition: "left" | "right";
  label: string;
  formSearchName: string;
  hint?: string | undefined;
  possibleSelections?: SelectionInfo[];
  defaultValue?: string | undefined;
  hasRollbackButton: boolean;
  onRollback?: () => void | undefined;
};

export type SelectionInfo = {
  htmlValue: string;
  presentValue: string;
};

export default function ValueInput({
  type,
  keyPosition,
  label,
  formSearchName,
  hint = undefined,
  possibleSelections = [],
  defaultValue = undefined,
  hasRollbackButton,
  onRollback = undefined,
}: Props) {
  return (
    <div className="value-input">
      <p className={`key ${keyPosition}`}>{label}</p>
      {type == "textarea" && (
        <div className="input-container">
          <textarea
            className="value"
            name={formSearchName}
            id={formSearchName}
            placeholder={hint}
            defaultValue={defaultValue}
          />
        </div>
      )}
      {type == "select" && (
        <div className="input-container">
          <select name={formSearchName} id={formSearchName}>
            {possibleSelections.map((s) => {
              return (
                <option defaultValue={s.htmlValue}>{s.presentValue}</option>
              );
            })}
          </select>
        </div>
      )}
      {type == "input" && (
        <div className="input-container">
          <input
            className="value"
            type="text"
            name={formSearchName}
            id={formSearchName}
            placeholder={hint}
            defaultValue={defaultValue}
          />
        </div>
      )}
      {type == "checkbox" && (
        <div className="input-container">
          <input
            className="value"
            type="checkbox"
            name={formSearchName}
            id={formSearchName}
            placeholder={hint}
            defaultValue={defaultValue}
          />
        </div>
      )}
      {hasRollbackButton && (
        <button
          type="button"
          className="rollback"
          onClick={() => {
            onRollback && onRollback();
          }}
        >
          <img src="/img/undo.svg" alt="rollback" />
        </button>
      )}
    </div>
  );
}
