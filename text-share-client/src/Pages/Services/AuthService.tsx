import axios, { AxiosError, AxiosResponse, AxiosStatic } from "axios";
import { LoginDto, RegisterDto, UserWithTokenDto } from "../../Dtos"
import { handleApiError } from "./ErrorHandler";


export const RegisterAPI = async (register: RegisterDto) : Promise<AxiosResponse<UserWithTokenDto, any> | string[]> => {
    try {
        const url = process.env.REACT_APP_SERVER_URL_PATH + "api/accounts/register/";
        const data = await axios.post<UserWithTokenDto>(url, register, {
            headers: {
                "Content-Type": "application/json",
            }
        });
        return data;
    } catch (error) {
        return handleApiError(error as AxiosError);
    }
}

export const LoginAPI = async (login: LoginDto) : Promise<AxiosResponse<UserWithTokenDto, any> | string[]> => {
    try {
        const url = process.env.REACT_APP_SERVER_URL_PATH + "api/accounts/login";
        const data = await axios.post<UserWithTokenDto>(url, login, {
            headers: {
                "Content-Type": "application/json",
            }
        });
        return data;
    } catch (error) {
        return handleApiError(error as AxiosError);
    }
}
