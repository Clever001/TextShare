import CommentsF from "../3-Features/Comments/CommentsF";
import DocViewF from "../3-Features/DocView/DocViewF";

export default function ViewPage() {
  return <div style={{display: "flex", gap: "30px", flexDirection: "column"}}>
    <DocViewF />
    <CommentsF />
  </div>;
}
