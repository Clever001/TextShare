import "./CustomButton.css";

type Props = {
  leftIconUrl?: string | null;
  rightIconUrl?: string | null;
  text?: string | null;
  color?: "blue" | "green" | "yellow" | "red";
  type?: "submit" | "button";
  target: string;
  onClick?: () => void;
};

export default function CustomButton({
  leftIconUrl = "/img/add_circle.svg",
  rightIconUrl = "/img/add_circle.svg",
  text = "Нажми на меня!",
  color = "blue",
  type = "button",
  target,
  onClick = () => {},
}: Props) {
  const colorMap = {
    blue: "--accent",
    green: "--success",
    yellow: "--accent-2",
    red: "--error",
  };

  const buttonStyle: React.CSSProperties = {
    backgroundColor: `var(${colorMap[color] || "--accent"})`,
  };

  return (
    <button
      type={type}
      className="custom-button"
      onClick={onClick}
      style={buttonStyle}
    >
      {leftIconUrl && <img src={leftIconUrl} alt={target} />}
      {text && <p>{text}</p>}
      {rightIconUrl && <img src={rightIconUrl} alt={target} />}
    </button>
  );
}
