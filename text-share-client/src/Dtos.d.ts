export interface RegisterDto {
    userName : string,
    email : string,
    password : string
}

export interface UserWithTokenDto {
    userName : string,
    email : string,
    token : string
}

export interface LoginDto {
    userNameOrEmail : string,
    password : string
}