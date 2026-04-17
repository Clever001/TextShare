import { useReducer } from "react";
import "./Pagination.css";

type Props = {
  totalPages: number;
  initialPage: number;
  onChangePage: (newPage: number) => void;
};

export default function Pagination(props: Props) {
  type State = {
    curPage: number;
  };

  type Action =
    | { type: "SetCurPage"; payload: number }
    | { type: "PrevPage" }
    | { type: "NextPage" };

  const reducer = (state: State, action: Action): State => {
    var newPage: number;
    switch (action.type) {
      case "SetCurPage":
        if (action.payload < 1 || action.payload > props.totalPages) {
          return { ...state };
        } else {
          newPage = action.payload;
        }
        break;
      case "PrevPage":
        if (canGoToPage(1)) {
          newPage = state.curPage - 1;
        } else {
          return { ...state };
        }
        break;
      case "NextPage":
        if (canGoToPage(props.totalPages)) {
          newPage = state.curPage + 1;
        } else {
          return { ...state };
        }
        break;
      default:
        return { ...state };
    }

    props.onChangePage(newPage);
    return {
      curPage: newPage,
    };
  };

  const canGoToPage = (newPage: number) => {
    return newPage >= 1 && newPage <= props.totalPages;
  };

  const [pagState, pagDispatch] = useReducer(reducer, {
    curPage: props.initialPage,
  });

  return (
    <nav className="pagination" aria-label="Search navigation">
      <button
        type="button"
        onClick={() => pagDispatch({ type: "PrevPage" })}
        disabled={!canGoToPage(pagState.curPage - 1)}
      >
        <img src="/img/left_arrow.svg" alt="left_arrow" />
      </button>
      <ul className="page-list">
        {pagState.curPage >= 2 && (
          <li>
            <button
              type="button"
              onClick={() => pagDispatch({ type: "SetCurPage", payload: 1 })}
            >
              1
            </button>
          </li>
        )}
        {pagState.curPage >= 5 && <li className="dots">...</li>}
        {pagState.curPage >= 4 && (
          <li>
            <button
              type="button"
              onClick={() =>
                pagDispatch({
                  type: "SetCurPage",
                  payload: pagState.curPage - 2,
                })
              }
            >
              {pagState.curPage - 2}
            </button>
          </li>
        )}
        {pagState.curPage >= 3 && (
          <li>
            <button
              type="button"
              onClick={() =>
                pagDispatch({
                  type: "SetCurPage",
                  payload: pagState.curPage - 1,
                })
              }
            >
              {pagState.curPage - 1}
            </button>
          </li>
        )}
        <li>
          <button
            type="button"
            className="active"
            style={{ cursor: "default" }}
          >
            <b>
              <u>{pagState.curPage}</u>
            </b>
          </button>
        </li>
        {pagState.curPage <= props.totalPages - 2 && (
          <li>
            <button
              type="button"
              onClick={() =>
                pagDispatch({
                  type: "SetCurPage",
                  payload: pagState.curPage + 1,
                })
              }
            >
              {pagState.curPage + 1}
            </button>
          </li>
        )}
        {pagState.curPage <= props.totalPages - 3 && (
          <li>
            <button
              type="button"
              onClick={() =>
                pagDispatch({
                  type: "SetCurPage",
                  payload: pagState.curPage + 2,
                })
              }
            >
              {pagState.curPage + 2}
            </button>
          </li>
        )}
        {pagState.curPage <= props.totalPages - 4 && (
          <li className="dots">...</li>
        )}
        {pagState.curPage <= props.totalPages - 1 && (
          <li>
            <button
              type="button"
              onClick={() =>
                pagDispatch({ type: "SetCurPage", payload: props.totalPages })
              }
            >
              {props.totalPages}
            </button>
          </li>
        )}
      </ul>
      <button
        type="button"
        onClick={() => pagDispatch({ type: "NextPage" })}
        disabled={!canGoToPage(pagState.curPage + 1)}
      >
        <img src="/img/right_arrow.svg" alt="right_arrow" />
      </button>
    </nav>
  );
}
