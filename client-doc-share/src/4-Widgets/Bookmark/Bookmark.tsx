import "./Bookmark.css";

type Props = {
  text: string;
  isActive: boolean;
  onClick: () => void;
};

export default function Bookmark({ text, isActive, onClick }: Props) {
  return (
    <button
      className={`bookmark ${isActive && "active"}`}
      onClick={() => {
        isActive && onClick();
      }}
    >
      {text}
    </button>
  );
}
