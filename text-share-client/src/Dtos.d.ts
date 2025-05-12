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

export interface ExceptionDto {
    code: string,
    description: string,
    details: string[] | null
}