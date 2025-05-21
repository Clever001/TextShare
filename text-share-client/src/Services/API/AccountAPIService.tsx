import axios from "axios";
import { ExceptionDto, FriendRequestDto, PaginatedResponseDto, PaginationDto, ProcessFriendRequestDto, UserWithoutTokenDto } from "../../Dtos"
import { translateException } from "../TranslatorService";
import { handleApiError } from "../ErrorHandler";



export const AreFriendsAPI = async (token: string, otherUserName: string) : Promise<boolean | ExceptionDto> => {
    try {
        const url = process.env.REACT_APP_SERVER_URL_PATH + "api/friends/areFriends/" + otherUserName;

        const data = await axios.get<string>(url, {
            headers: {
                "Authorization": `Bearer ${token}`
            }
        })

        const response = data.data as string;

        if (response === "Are friends")
            return true;
        if (response === "Are not friends")
            return false;

        return {
            httpCode: -1,
            apiCode: "AnswerDecomposeException",
            description: "Ответ был получен, но не получилось его правильно распознать клиентом.",
            details: null
        }
    } catch (error) {
        return translateException(handleApiError(error as Error));
    }
}

export const GetRequestFromMeAPI = async (token: string, recipientName: string) : Promise<FriendRequestDto | ExceptionDto> => {
    try {
        const url = process.env.REACT_APP_SERVER_URL_PATH + "api/friendRequests/fromMe/" + recipientName;

        const data = await axios.get<FriendRequestDto>(url, {
            headers: {
                "Authorization": `Bearer ${token}`
            }
        });

        return data.data as FriendRequestDto;
    } catch (error) {
        return translateException(handleApiError(error as Error));
    }
}

export const GetRequestToMeAPI = async (token: string, senderName: string) : Promise<FriendRequestDto | ExceptionDto> => {
    try {
        const url = process.env.REACT_APP_SERVER_URL_PATH + "api/friendRequests/ToMe/" + senderName;

        const data = await axios.get<FriendRequestDto>(url, {
            headers: {
                "Authorization": `Bearer ${token}`
            }
        });

        return data.data as FriendRequestDto;
    } catch (error) {
        return translateException(handleApiError(error as Error));
    }
}

export const SendFriendRequestAPI = async (token: string, recipientName: string): Promise<FriendRequestDto | ExceptionDto> => {
    try {
        const url = process.env.REACT_APP_SERVER_URL_PATH + "api/friendRequests/" + recipientName;

        const data = await axios.post<FriendRequestDto>(url, {}, {
            headers: {
                "Authorization": `Bearer ${token}`
            }
        });

        return data.data as FriendRequestDto;
    } catch (error) {
        return translateException(handleApiError(error as Error));
    }
}

export const DeleteFriendRequestAPI = async (token: string, recipientName: string): Promise<null | ExceptionDto> => {
    try {
        const url = process.env.REACT_APP_SERVER_URL_PATH + "api/friendRequests/" + recipientName;

        const data = await axios.delete(url, {
            headers: {
                "Authorization": `Bearer ${token}`
            }
        });

        return null;
    } catch (error) {
        return translateException(handleApiError(error as Error));
    }
}

export const ProcessFriendRequestAPI = async (token: string, senderName: string, dto: ProcessFriendRequestDto): Promise<FriendRequestDto | ExceptionDto> => {
    try {
        const url = process.env.REACT_APP_SERVER_URL_PATH + "api/friendRequests/" + senderName;

        const data = await axios.put<FriendRequestDto>(url, dto, {
            headers: {
                "Authorization": `Bearer ${token}`
            }
        });

        return data.data as FriendRequestDto;
    } catch (error) {
        return translateException(handleApiError(error as Error));
    }
}

export const DeleteFriendAPI = async (token: string, userName: string): Promise<null | ExceptionDto> => {
    try {
        const url = process.env.REACT_APP_SERVER_URL_PATH + "api/friends/" + userName;

        const data = await axios.delete(url, {
            headers: {
                "Authorization": `Bearer ${token}`
            }
        });

        return null;
    } catch (error) {
        return translateException(handleApiError(error as Error));
    }
}

export const GetUsersAPI = async (pagination: PaginationDto, searchName: string | null) : Promise<PaginatedResponseDto<UserWithoutTokenDto> | ExceptionDto> => {
    try {
        var url = process.env.REACT_APP_SERVER_URL_PATH + "api/accounts";
        url += `?pageNumber=${pagination.pageNumber}`;
        url += `&pageSize=${pagination.pageSize}`;
        if (searchName) url += `&userName=${searchName}`;

        const data = await axios.get<PaginatedResponseDto<UserWithoutTokenDto>>(url);

        return data.data as PaginatedResponseDto<UserWithoutTokenDto>;
    } catch (error) {
        return translateException(handleApiError(error as Error));
    }
}

export const GetFriendsAPI = async (pagination: PaginationDto, isAscending: boolean, searchName: string | null, token: string) : Promise<PaginatedResponseDto<UserWithoutTokenDto> | ExceptionDto> => {
    try {
        var url = process.env.REACT_APP_SERVER_URL_PATH + "api/friends";
        url += `?pageNumber=${pagination.pageNumber}`;
        url += `&pageSize=${pagination.pageSize}`;
        url += `&isAscending=${isAscending}`;
        if (searchName) url += `&friendName=${searchName}`;

        const data = await axios.get<PaginatedResponseDto<UserWithoutTokenDto>>(url, {
            headers: {
                "Authorization": `Bearer ${token}`
            }
        });

        return data.data as PaginatedResponseDto<UserWithoutTokenDto>;
    } catch (error) {
        return translateException(handleApiError(error as Error));
    }
}

export const GetSentFriendRequestsAPI = async(pagination: PaginationDto, isAscending: boolean, searchName: string | null, token: string) : Promise<PaginatedResponseDto<FriendRequestDto> | ExceptionDto> => {
    try {
        var url = process.env.REACT_APP_SERVER_URL_PATH + "api/friendRequests/fromMe";
        url += `?pageNumber=${pagination.pageNumber}`;
        url += `&pageSize=${pagination.pageSize}`;
        url += `&isAscending=${isAscending}`;
        if (searchName) url += `&recipientName=${searchName}`;

        const data = await axios.get<PaginatedResponseDto<FriendRequestDto>>(url, {
            headers: {
                "Authorization": `Bearer ${token}`
            }
        });

        return data.data as PaginatedResponseDto<FriendRequestDto>;
    } catch (error) {
        return translateException(handleApiError(error as Error));
    }
}

export const GetRecievedFriendRequestsAPI = async(pagination: PaginationDto, isAscending: boolean, searchName: string | null, token: string) : Promise<PaginatedResponseDto<FriendRequestDto> | ExceptionDto> => {
    try {
        var url = process.env.REACT_APP_SERVER_URL_PATH + "api/friendRequests/toMe";
        url += `?pageNumber=${pagination.pageNumber}`;
        url += `&pageSize=${pagination.pageSize}`;
        url += `&isAscending=${isAscending}`;
        if (searchName) url += `&senderName=${searchName}`;

        const data = await axios.get<PaginatedResponseDto<FriendRequestDto>>(url, {
            headers: {
                "Authorization": `Bearer ${token}`
            }
        });

        return data.data as PaginatedResponseDto<FriendRequestDto>;
    } catch (error) {
        return translateException(handleApiError(error as Error));
    }
}