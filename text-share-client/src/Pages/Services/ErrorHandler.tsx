import axios, { AxiosError } from "axios";

export const handleApiError = (error: AxiosError): string[] => {
    if (!axios.isAxiosError(error)) {
        return ["Неизвестная ошибка."];
    }

    if (error.response) {
        const status = error.response.status;

        if (status === 400) {
            const responseData = error.response.data as {
                errors?: Record<string, string[]>;
            };

            if (responseData.errors) {
                const errorMessages: string[] = [];
                for (const [field, messages] of Object.entries(responseData.errors)) {
                    errorMessages.push(...messages.map(msg => `${field}: ${msg}`));
                }
                return errorMessages;
            }
        }

        if (status === 500) {
            return ["Ошибка работы сервера."];
        }
    }

    return ["Ошибка подключения к серверу."];
};