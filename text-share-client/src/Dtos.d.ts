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

export interface UserWithoutTokenDto {
    id: string,
    userName: string
}

// Exception

export interface ExceptionInput {
    code: string,
    description: string,
    details: string[] | null
}

export interface ExceptionDto {
    httpCode: number,
    apiCode: string,
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

export interface UpdateTextDto {
    title: string | null,
    description: string | null,
    content: string | null,
    syntax: string | null,
    tags: string[] | null,
    accessType: string | null,
    password: string | null,
    updatePassword: bool
}

export interface CreateTextDto {
    title: string,
    description: string,
    content: string,
    syntax: string,
    tags: string[],
    accessType: string,
    password: string | null
}

// FriendRequest

export interface FriendRequestDto {
    senderName: string,
    recipientName: string,
    isAccepted: boolean | null
}

export interface ProcessFriendRequestDto {
    acceptRequest: booleacn
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
    hasPassword: boolean | null
}

export interface TextFilterWithoutNameDto {
    title: string | null,
    tags: string[] | null,
    syntax: string | null,
    accessType: string | null,
    hasPassword: bool | null
}