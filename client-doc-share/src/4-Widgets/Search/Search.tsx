import CustomButton from "../CustomButton/CustomButton";
import "./Search.css";

type Props = {
  formSearchName: string;
  hint?: string | undefined;
  buttonText?: string | undefined;
};

export default function Search({
  formSearchName,
  hint = undefined,
  buttonText = undefined,
}: Props) {
  return (
    <div className="form-search">
      <input
        type="text"
        name={formSearchName}
        placeholder={hint}
        className="search-input"
      />
      <CustomButton
        leftIconUrl={null}
        rightIconUrl={null}
        text={buttonText ? buttonText : "Искать"}
        type="submit"
        target="search"
        onClick={() => { }}
      />
    </div>
  );
}
