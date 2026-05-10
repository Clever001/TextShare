import { useMemo, useState } from "react";
import DocsTable from "../../4-Widgets/DocsTable/DocsTable";
import Pagination from "../../4-Widgets/Pagination/Pagination";
import Search from "../../4-Widgets/Search/Search";
import ValueInput from "../../4-Widgets/ValueInput/ValueInput";
import type { PaginatedResponseDtoOfShortDocumentDto } from "../../6-Shared/ApiClient";
import "./SearchDocF.css";
import { generateDocumentApi, getApiErrors } from "../../6-Shared/utils";

type Props = {
  isProfilePage: boolean;
};

type SearchQuery = {
  pageNumber: number;
  pageSize: number;
  title?: string | undefined;
  tags?: Array<string> | undefined;
  fromDate?: string | undefined;
  toDate?: string | undefined;
  ownerName?: string | undefined;
};

const PAGE_SIZE = 5;
const START_PAGE = 1;

export default function SearchDocF({ isProfilePage }: Props) {
  const docApi = useMemo(generateDocumentApi, [])
  const [docs, setDocs] = useState<PaginatedResponseDtoOfShortDocumentDto>({
    items: [],
    totalItems: 0,
    totalPages: 0,
    currentPage: 1,
    pageSize: 5
  });
  const [errors, setErrors] = useState<string[]>([])

  var searchQuery: SearchQuery = {
    pageNumber: START_PAGE,
    pageSize: PAGE_SIZE
  }

  const onSearchClick = (e: React.SubmitEvent<HTMLFormElement>) => {
    e.preventDefault()

    const formData = new FormData(e.currentTarget)
    searchQuery = {
      pageNumber: START_PAGE,
      pageSize: PAGE_SIZE,
      title: formData.get("title")?.toString() ?? undefined,
      tags: (formData.get("tags")?.toString() ?? "").split(" ").filter(e => e.length > 0),
      fromDate: formData.get("start-release-date")?.toString(),
      toDate: formData.get("end-release-date")?.toString(),
      ownerName: formData.get("creator")?.toString()
    }
    onGetDocs()
  }

  const onChangePage = (newPage: number) => {
    searchQuery.pageNumber = newPage
    onGetDocs()
  }

  const onGetDocs = async () => {
    try {
      const { data } = await docApi.searchDocuments(
        undefined, undefined, searchQuery.pageNumber,
        searchQuery.pageSize, searchQuery.title, searchQuery.tags,
        searchQuery.fromDate, searchQuery.toDate, searchQuery.ownerName,
        undefined
      )
      setErrors([])
      setDocs(data)
    } catch (err) {
      const errors = getApiErrors(err)
      setErrors(errors)
    }
  }

  return (
    <div className="doc-search-feature">
      <form className="doc-search-form" onSubmit={onSearchClick}>
        <Search widgetType="doc-search" formSearchName="title" hint="Название документа" />
        <div className="search-params">
          <ValueInput
            widgetType="input"
            hint="тег1 тег2 тег3 тег4"
            keyPosition="left"
            label="Теги"
            formSearchName="tags"
            hasRollbackButton={false}
          />
          <ValueInput
            widgetType="date"
            keyPosition="left"
            label="Опубликован с даты"
            formSearchName="start-release-date"
            hasRollbackButton={false}
          />
          <ValueInput
            widgetType="date"
            keyPosition="left"
            label="Опубликован по дату"
            formSearchName="end-release-date"
            hasRollbackButton={false}
          />
          {!isProfilePage && (
            <ValueInput
              widgetType="input"
              keyPosition="left"
              label="Создатель"
              formSearchName="creator"
              hasRollbackButton={false}
            />
          )}
        </div>
        {errors.length > 0 &&
          <div className="errors">
            {errors.map(e => <p>{e}</p>)}
          </div>
        }
        <DocsTable docinfos={docs.items} showCreators={!isProfilePage} />
        <div className="pagination-container">
          <Pagination totalPages={docs.totalPages} initialPage={1} onChangePage={onChangePage} />
        </div>
      </form>
    </div>
  );
}
