import { useCallback, useReducer } from "react";
import "./Pagination.css";

type Props = {
  totalPages: number;
  curPage: number;
  onChangePage: (newPage: number) => void;
};

export default function Pagination({
  totalPages, curPage, onChangePage
}: Props) {
  const canGoToPage = useCallback((newPage: number) => {
    return newPage >= 1 && newPage <= totalPages;
  }, [totalPages])

  return (
    <nav className="pagination" aria-label="Search navigation">
      <button
        type="button"
        onClick={() => onChangePage(curPage - 1)}
        disabled={!canGoToPage(curPage - 1)}
      >
        <img src="/img/left_arrow.svg" alt="left_arrow" />
      </button>
      <ul className="page-list">
        {curPage >= 2 && (
          <li>
            <button
              type="button"
              onClick={() => onChangePage(1)}
            >
              1
            </button>
          </li>
        )}
        {curPage >= 5 && <li className="dots">...</li>}
        {curPage >= 4 && (
          <li>
            <button
              type="button"
              onClick={() => onChangePage(curPage - 2)}
            >
              {curPage - 2}
            </button>
          </li>
        )}
        {curPage >= 3 && (
          <li>
            <button
              type="button"
              onClick={() => onChangePage(curPage - 1)}
            >
              {curPage - 1}
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
              <u>{curPage}</u>
            </b>
          </button>
        </li>
        {curPage <= totalPages - 2 && (
          <li>
            <button
              type="button"
              onClick={() => onChangePage(curPage + 1)}
            >
              {curPage + 1}
            </button>
          </li>
        )}
        {curPage <= totalPages - 3 && (
          <li>
            <button
              type="button"
              onClick={() => onChangePage(curPage + 2)}
            >
              {curPage + 2}
            </button>
          </li>
        )}
        {curPage <= totalPages - 4 && (
          <li className="dots">...</li>
        )}
        {curPage <= totalPages - 1 && (
          <li>
            <button
              type="button"
              onClick={() => onChangePage(totalPages)}
            >
              {totalPages}
            </button>
          </li>
        )}
      </ul>
      <button
        type="button"
        onClick={() => onChangePage(curPage + 1)}
        disabled={!canGoToPage(curPage + 1)}
      >
        <img src="/img/right_arrow.svg" alt="right_arrow" />
      </button>
    </nav>
  );
}
