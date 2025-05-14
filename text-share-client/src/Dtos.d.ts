// Auth
export interface RegisterDto {
    userName: string,
    email: string,
    password: string
}

export interface UserWithTokenDto {
    id: string,
    userName: string,
    email: string,
    token: string
}

export interface LoginDto {
    userNameOrEmail: string,
    password: string
}

// Exception

export interface ExceptionDto {
    code: string,
    description: string,
    details: string[] | null
}

// Text

export interface TextWithoutContentInput {
    id: string,
    title: string,
    description: string,
    syntax: string,
    tags: string[],
    createdOn: string,
    updatedOn: string | null,
    ownerName: string,
    accessType: string,
    hasPassword: boolean
}

export interface TextWithoutContentDto {
    id: string,
    title: string,
    description: string,
    syntax: string,
    tags: string[],
    createdOn: Date,
    updatedOn: Date | null,
    ownerName: string,
    accessType: string,
    hasPassword: boolean
}

export interface PaginatedResponseDto<T> {
    items: T[],
    totalItems: number,
    totalPages: number,
    currentPage: number,
    pageSize: number
}

export interface TextWithContentInput {
    id: string,
    title: string,
    description: string,
    syntax: string,
    tags: string[],
    content: string,
    createdOn: string,
    updatedOn: string | null,
    ownerName: string,
    accessType: string,
    hasPassword: boolean
}

export interface TextWithContentDto {
    id: string,
    title: string,
    description: string,
    syntax: string,
    tags: string[],
    content: string,
    createdOn: Date,
    updatedOn: Date | null,
    ownerName: string,
    accessType: string,
    hasPassword: boolean
}

// QueryOptions

export interface PaginationDto {
    pageNumber: number,
    pageSize: number
}

export interface SortDto {
    sortBy: string,
    sortAscending: boolean
}

export interface TextFilterDto {
    ownerName: string | null,
    title: string | null,
    tags: string[] | null,
    syntax: string | null,
    accessType: string | null,
    hasPassword: bool | null
}