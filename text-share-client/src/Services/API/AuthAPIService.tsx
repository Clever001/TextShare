import axios, { AxiosError, AxiosResponse, AxiosStatic } from "axios";
import { ExceptionDto, LoginDto, RegisterDto, UserWithTokenDto } from "../../Dtos"
import { handleApiError } from "../ErrorHandler";
import { translateException } from "../TranslatorService";


export const RegisterAPI = async (register: RegisterDto) : Promise<UserWithTokenDto | ExceptionDto> => {
    try {
        const url = process.env.REACT_APP_SERVER_URL_PATH + "api/accounts/register/";
        const response = await axios.post<UserWithTokenDto>(url, register, {
            headers: {
                "Content-Type": "application/json",
            }
        });
        return response.data;
    } catch (error) {
        return translateException(handleApiError(error as Error));
    }
}

export const LoginAPI = async (login: LoginDto) : Promise<UserWithTokenDto | ExceptionDto> => {
    try {
        const url = process.env.REACT_APP_SERVER_URL_PATH + "api/accounts/login";
        const response = await axios.post<UserWithTokenDto>(url, login, {
            headers: {
                "Content-Type": "application/json",
            }
        });
        return response.data;
    } catch (error) {
        return translateException(handleApiError(error as Error));
    }
}
