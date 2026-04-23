import "./CommentsF.css";
import Comment from "../../4-Widgets/Comment/Comment";
import SectionTitle from "../../4-Widgets/SectionTitle/SectionTitle";
import Pagination from "../../4-Widgets/Pagination/Pagination";
import CustomButton from "../../4-Widgets/CustomButton/CustomButton";

type Props = {};

export default function CommentsF(props: Props) {
  return (
    <div className="comments-feature">
      <SectionTitle title="Комментарии" />
      <CustomButton
        leftIconUrl={null}
        rightIconUrl={null}
        text="Прокомментировать"
        color="blue"
        type="button"
        target="comment"
      />
      <div className="comments-container">
        <Comment
          content="Вау! Какой документ! Так он мне понравился!!! :)"
          creatorName="Попов А.А."
          elapsedTimeString="3 часа назад"
        />
        <div className="nested-comments">
          <Comment
            content="И вправду хороший документ"
            creatorName="Львовна Д.Н."
            elapsedTimeString="3 часа назад"
          />
          <Comment
            content="Согласен!"
            creatorName="Желудин А.Б"
            elapsedTimeString="3 часа назад"
          />
        </div>
      </div>
      <div className="pagination-container">
        <Pagination totalPages={10} initialPage={0} onChangePage={() => {}} />
      </div>
    </div>
  );
}
