import axios, { AxiosError } from "axios";
import { ExceptionDto } from "../Dtos";

export const handleApiError = (error: Error): string[] => {
    if (!axios.isAxiosError(error)) {
        return ["Неизвестная ошибка."];
    }

    if (error.response) {
        const status = error.response.status;

        if (status === 400) {
            const responseData = error.response.data as ExceptionDto;

            if (responseData.details) {
                return responseData.details;
            }

            return [responseData.description];
        }

        if (status === 401) {
            const responseData = error.response.data as ExceptionDto;

            return [responseData.description];
        }

        if (status == 403) {
            return ["Токен невалиден."]
        }

        if (status === 500) {
            return ["Ошибка работы сервера."];
        }
    }

    return ["Ошибка подключения к серверу."];
};