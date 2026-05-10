import { useCallback, useEffect, useMemo, useRef, useState } from "react";
import DocsTable from "../../4-Widgets/DocsTable/DocsTable";
import Pagination from "../../4-Widgets/Pagination/Pagination";
import Search from "../../4-Widgets/Search/Search";
import ValueInput from "../../4-Widgets/ValueInput/ValueInput";
import type { PaginatedResponseDtoOfShortDocumentDto } from "../../6-Shared/ApiClient";
import "./SearchDocF.css";
import { assignFormInputFromSearch, generateDocumentApi, getApiErrors, toDate, toNumberOrDefault } from "../../6-Shared/utils";
import { useSearchParams } from "react-router-dom";

type Props = {
  isProfilePage: boolean,
  userId: string
}

type SearchQuery = {
  pageNumber: number;
  title?: string | undefined;
  tags?: Array<string> | undefined;
  fromDate?: string | undefined;
  toDate?: string | undefined;
  ownerName?: string | undefined;
};

const PAGE_SIZE = 20
const START_PAGE = 1

export default function SearchDocF(props: Props) {
  const docApi = useMemo(generateDocumentApi, [])
  const [searchParams, setSearchParams] = useSearchParams();
  const [docs, setDocs] = useState<PaginatedResponseDtoOfShortDocumentDto>({
    items: [],
    totalItems: 0,
    totalPages: 0,
    currentPage: 1,
    pageSize: 5
  });
  const [errors, setErrors] = useState<string[]>([])
  const searchQueryRef = useRef<SearchQuery>({
    pageNumber: START_PAGE
  })
  const formRef = useRef<HTMLFormElement>(null);

  const onSearchClick = (e: React.SubmitEvent<HTMLFormElement>) => {
    e.preventDefault()

    const formData = new FormData(e.currentTarget)
    searchQueryRef.current = {
      pageNumber: START_PAGE,
      title: formData.get("title")?.toString() ?? undefined,
      tags: (formData.get("tags")?.toString() ?? "").split(" ").filter(e => e.length > 0),
      fromDate: formData.get("start-release-date")?.toString(),
      toDate: formData.get("end-release-date")?.toString(),
      ownerName: undefined
    }
    if (!props.isProfilePage) {
      searchQueryRef.current.ownerName = formData.get("creator")?.toString()
    }
    
    performSearch()
  }

  const onChangePage = (newPage: number) => {
    searchQueryRef.current.pageNumber = newPage
    performSearch()
  }

  useEffect(() => {
    searchQueryRef.current = {
      pageNumber: toNumberOrDefault(searchParams.get("pageNumber"), START_PAGE),
      title: searchParams.get("title") ?? undefined,
      tags: searchParams.get("tags")?.split(" ").filter(i => i.length > 0),
      fromDate: toDate(searchParams.get("fromDate"))?.toISOString(),
      toDate: toDate(searchParams.get("toDate"))?.toISOString(),
    }
    if (!props.isProfilePage) {
      searchQueryRef.current.ownerName = searchParams.get("ownerName") ?? undefined
    }

    const form = formRef.current
    if (form) {
      assignFormInputFromSearch(form, searchParams, "title", "title")
      assignFormInputFromSearch(form, searchParams, "tags", "tags")
      assignFormInputFromSearch(form, searchParams, "start-release-date", "fromDate")
      assignFormInputFromSearch(form, searchParams, "end-release-date", "toDate")
      if (!props.isProfilePage) {
        assignFormInputFromSearch(form, searchParams, "creator", "ownerName")
      }
    }
    performSearch()
  }, [props.userId])

  const performSearch = useCallback(async () => {
    const localSearchQuery = searchQueryRef.current

    const newParams: Record<string, string> = {}
    newParams.pageNumber = localSearchQuery.pageNumber.toString()
    if (localSearchQuery.title) {
      newParams.title = localSearchQuery.title
    }
    if (localSearchQuery.tags) {
      newParams.tags = localSearchQuery.tags.join(" ")
    }
    if (localSearchQuery.fromDate) {
      newParams.fromDate = localSearchQuery.fromDate
    }
    if (localSearchQuery.toDate) {
      newParams.toDate = localSearchQuery.toDate
    }
    if (!props.isProfilePage && localSearchQuery.ownerName) {
      newParams.ownerName = localSearchQuery.ownerName
    }
    setSearchParams(newParams)

    try {
      if (props.isProfilePage) {
        const { data } = await docApi.searchDocuments(
          undefined, undefined, localSearchQuery.pageNumber,
          PAGE_SIZE, localSearchQuery.title, localSearchQuery.tags,
          localSearchQuery.fromDate, localSearchQuery.toDate, undefined,
          props.userId
        )
        setDocs(data)
      } else {
        const { data } = await docApi.searchDocuments(
          undefined, undefined, localSearchQuery.pageNumber,
          PAGE_SIZE, localSearchQuery.title, localSearchQuery.tags,
          localSearchQuery.fromDate, localSearchQuery.toDate, localSearchQuery.ownerName,
          undefined
        )
        setDocs(data)
      }
      setErrors([])
    } catch (err) {
      const errors = getApiErrors(err)
      setErrors(errors)
    }
  }, [props.userId])

  return (
    <div className="doc-search-feature">
      <form className="doc-search-form" onSubmit={onSearchClick} ref={formRef}>
        <Search 
          widgetType="doc-search" 
          formSearchName="title" 
          hint="Название документа" 
        />
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
          {!props.isProfilePage && (
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
        <DocsTable docinfos={docs.items} showCreators={!props.isProfilePage} />
        <div className="pagination-container">
          <Pagination
            totalPages={docs.totalPages}
            curPage={docs.currentPage}
            onChangePage={onChangePage}
          />
        </div>
      </form>
    </div>
  );
}
