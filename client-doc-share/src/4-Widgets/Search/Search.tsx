import CustomButton from "../CustomButton/CustomButton";
import "./Search.css"

type Props = | {
  widgetType: "user-search",
  hint: string;
  buttonText: string;
  value: string;
  onValueChange: (e: React.ChangeEvent<HTMLInputElement, HTMLInputElement>) => void;
  suggestions: string[];
  onActionBtnClick: () => void
} | {
  widgetType: "doc-search",
  formSearchName: string,
  hint: string
}

export default function Search(props: Props) {
  return (
    <div className="form-search">
      <input
        type="text"
        placeholder={props.hint}
        className="search-input"
        list="suggestions"
        name = {props.widgetType === "doc-search" ? props.formSearchName : undefined}
        autoComplete={props.widgetType === "user-search" ? "off" : "on"}
        value={props.widgetType === "user-search" ? props.value : undefined}
        onChange={props.widgetType === "user-search" ? props.onValueChange : undefined}
        onKeyDown={
          props.widgetType == "user-search" ?
            e => {
              if (e.key == "Enter") {
                e.preventDefault()
                props.onActionBtnClick()
              }
            }
            :
            undefined
        }
      />
      {props.widgetType === "user-search" && 
        <datalist id="suggestions">
          {props.suggestions.map(s => <option key={s} value={s} />)}
        </datalist>
      }
      <CustomButton
        leftIconUrl={null}
        rightIconUrl={null}
        text={props.widgetType === "user-search" ? props.buttonText : "Искать"}
        type={props.widgetType === "user-search" ? "button" : "submit"}
        target="search"
        onClick={props.widgetType === "user-search" ? props.onActionBtnClick : undefined}
      />
    </div>
  );
}
