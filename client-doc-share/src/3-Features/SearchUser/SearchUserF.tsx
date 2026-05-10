import { useCallback, useEffect, useMemo, useRef, useState } from "react";
import Pagination from "../../4-Widgets/Pagination/Pagination";
import Search from "../../4-Widgets/Search/Search";
import SectionTitle from "../../4-Widgets/SectionTitle/SectionTitle";
import UserTable from "../../4-Widgets/UserTable/UserTable";
import "./SearchUserF.css";
import type { PaginatedResponseDtoOfUserWithoutTokenDto, UserWithoutTokenDto } from "../../6-Shared/ApiClient";
import { generateAccountApi, getApiErrors, toNumberOrDefault } from "../../6-Shared/utils";
import { useSearchParams } from "react-router-dom";

type Props = {};

type SearchQuery = {
  pageNumber: number,
  userName: string
}

const PAGE_SIZE = 20
const START_PAGE = 1

export default function SearchUserF(props: Props) {
  const accApi = useMemo(generateAccountApi, [])
  const [searchParams, setSearchParams] = useSearchParams()
  const [users, setUsers] = useState<PaginatedResponseDtoOfUserWithoutTokenDto>({
    items: [],
    totalItems: 0,
    totalPages: 1,
    currentPage: 1,
    pageSize: PAGE_SIZE
  })
  const [errors, setErrors] = useState<string[]>([])
  const searchQueryRef = useRef<SearchQuery>({
    pageNumber: START_PAGE,
    userName: ""
  })
  const [userName, setUserName] = useState<string>("")

  const onUserNameChange = useCallback((e: React.ChangeEvent<HTMLInputElement, HTMLInputElement>) => {
    setUserName(e.target.value)
  }, [])

  const onSearchClick = useCallback(async () => {
    console.log(userName)
    searchQueryRef.current = {
      pageNumber: START_PAGE,
      userName: userName
    }
    console.log(searchQueryRef.current)
    performSearch()
  }, [userName])

  const onChangePage = (newPage: number) => {
    searchQueryRef.current.pageNumber = newPage
    performSearch()
  }

  useEffect(() => {
    searchQueryRef.current = {
      pageNumber: toNumberOrDefault(searchParams.get("pageNumber"), START_PAGE),
      userName: searchParams.get("userName") ?? ""
    }
    
    setUserName(searchQueryRef.current.userName)
    performSearch()
  }, [])

  const performSearch = useCallback(async () => {
    const localSearchQuery = searchQueryRef.current

    console.log(localSearchQuery)


    const newParams: Record<string, string> = {}
    newParams.pageNumber = localSearchQuery.pageNumber.toString()
    newParams.userName = localSearchQuery.userName
    setSearchParams(newParams)

    try {
      console.log(localSearchQuery)
      const {data} = await accApi.searchAccounts(
        localSearchQuery.pageNumber, PAGE_SIZE, localSearchQuery.userName
      )
      setUsers(data)
      setErrors([])
    } catch (err) {
      const errors = getApiErrors(err)
      setErrors(errors)
    }
  }, [])

  return (
    <div className="search-user-feature">
      <SectionTitle title="Поиск пользователей на сайте" />
      <Search 
        widgetType="user-search"
        hint="Имя пользователя"
        buttonText="Искать"
        value={userName}
        onValueChange={onUserNameChange}
        suggestions={[]}
        onActionBtnClick={onSearchClick}
      />
      {errors.length > 0 &&
        <div className="errors">
          {errors.map(e => <p>{e}</p>)}
        </div>
      }
      <SectionTitle title="Результат поиска" />
      <UserTable usernames={users.items.map(u => u.userName)} />
      <div className="pagination-container">
        <Pagination 
          totalPages={users.totalPages} 
          curPage={users.currentPage} 
          onChangePage={onChangePage} 
        />
      </div>
    </div>
  );
}
