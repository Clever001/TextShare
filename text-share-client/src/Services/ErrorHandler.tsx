import axios, { AxiosError } from "axios";
import { ExceptionDto, ExceptionInput } from "../Dtos";

export const handleApiError = (error: Error): ExceptionDto => {
    if (!axios.isAxiosError(error)) {
        return {
            httpCode: -1,
            apiCode: "NonAxiosError",
            description: "Ошибка не связана с работой API.",
            details: null
        }
    }

    if (error.response) {
        const status = error.response.status;

        const responseData = error.response.data as ExceptionInput;

        return {
            httpCode: status,
            apiCode: responseData.code,
            description: responseData.description,
            details: responseData.details
        }
    }

    return {
        httpCode: 503,
        apiCode: "ServiceUnavailable",
        description: "",
        details: null
    }
};

export const isExceptionDto = (obj: any): obj is ExceptionDto => {
    return (
        obj != null && 
        typeof obj.httpCode === "number" &&
        typeof obj.apiCode === "string" &&
        typeof obj.description === "string" &&
        (
            (Array.isArray(obj.details) && obj.details.every((item: any) => typeof item === "string")) ||
            obj.details === null
        )
    );
};