import axios from "axios";
import { ExceptionDto, FriendRequestDto } from "../../Dtos"
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