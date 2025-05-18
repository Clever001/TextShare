import axios, { AxiosResponse } from "axios";
import { CreateTextDto, ExceptionDto, PaginatedResponseDto, PaginationDto, SortDto, TextFilterDto, TextWithContentDto, TextWithContentInput, TextWithoutContentDto, TextWithoutContentInput, UpdateTextDto } from "../../Dtos";
import { handleApiError } from "../ErrorHandler";
import { translateException } from "../TranslatorService";

export const SearchTextsAPI = async (pagination: PaginationDto, sort: SortDto, filter: TextFilterDto, token: string | null) : Promise<PaginatedResponseDto<TextWithoutContentDto> | ExceptionDto> => {
    try {
        var url = process.env.REACT_APP_SERVER_URL_PATH + "api/text" +
            `?pageNumber=${pagination.pageNumber}&pageSize=${pagination.pageSize}` +
            `&sortBy=${sort.sortBy}&sortAscending=${sort.sortAscending}`

        if (filter.ownerName) url += `&ownerName=${encodeURIComponent(filter.ownerName)}`
        if (filter.title) url += `&title=${encodeURIComponent(filter.title)}`
        if (filter.tags) filter.tags.forEach(t => url += `&tags=${encodeURIComponent(t)}`)
        if (filter.syntax) url += `&syntax=${encodeURIComponent(filter.syntax)}`
        if (filter.accessType) url += `&accessType=${encodeURIComponent(filter.accessType)}`
        if (filter.hasPassword != null) url += `&hasPassword=${encodeURIComponent(filter.hasPassword)}`
        var data:PaginatedResponseDto<TextWithoutContentInput> | null= null;

        if (token) {
            data = (await axios.get<PaginatedResponseDto<TextWithoutContentInput>>(url, {
                headers: {
                    "Authorization" : `Bearer ${token}`
                }
            })).data as PaginatedResponseDto<TextWithoutContentInput>;
        } else {
            data = (await axios.get<PaginatedResponseDto<TextWithoutContentInput>>(url)).data as PaginatedResponseDto<TextWithoutContentInput>;
        }

        const convertedData: PaginatedResponseDto<TextWithoutContentDto> = {
                items: data.items.map(ti => {
                    return {
                        id: ti.id,
                        title: ti.title,
                        description: ti.description,
                        syntax: ti.syntax,
                        tags: ti.tags,
                        createdOn: new Date(ti.createdOn),
                        updatedOn: ti.updatedOn ? new Date(ti.updatedOn) : null,
                        ownerName: ti.ownerName,
                        accessType: ti.accessType,
                        hasPassword: ti.hasPassword
                    };
                }),
                totalItems: data.totalItems,
                totalPages: data.totalPages,
                currentPage: data.currentPage,
                pageSize: data.pageSize
            }

        // console.log(data);
        // console.log(convertedData);
        return convertedData
    } catch (error) {
        return translateException(handleApiError(error as Error));
    }
}

export const SearchSocietyTextsAPI = async () : Promise<TextWithoutContentDto[] | ExceptionDto> => {
    try {
        const url = process.env.REACT_APP_SERVER_URL_PATH + "api/text/latests"

        const data = (await axios.get<TextWithoutContentInput[]>(url)).data as TextWithoutContentInput[]
        const convertedData: TextWithoutContentDto[] = data.map(t => {
            return {
                id: t.id,
                title: t.title,
                description: t.description,
                syntax: t.syntax,
                tags: t.tags,
                createdOn: new Date(t.createdOn),
                updatedOn: t.updatedOn ? new Date(t.updatedOn) : null,
                ownerName: t.ownerName,
                accessType: t.accessType,
                hasPassword: t.hasPassword
            }
        })
        
        return convertedData
    } catch (error) {
        return translateException(handleApiError(error as Error));
    }
}

export const SearchTextByIdAPI = async (id: string, token: string | null, password: string | null) : Promise<TextWithContentDto | ExceptionDto> => {
    try {
        var url = process.env.REACT_APP_SERVER_URL_PATH + "api/text/" + id;
        if (password) url += `?requestPassword=${password}`;

        const response = token ? await axios.get<TextWithContentInput>(url, {
                headers: {
                    "Authorization" : `Bearer ${token}`
                }
            }) : await axios.get<TextWithContentInput>(url);
            

        const data = response.data as TextWithContentInput;

        const convertedData: TextWithContentDto = {
            id: data.id,
            title: data.title,
            description: data.description,
            syntax: data.syntax,
            tags: data.tags,
            content: data.content,
            createdOn: new Date(data.createdOn),
            updatedOn: data.updatedOn ? new Date(data.updatedOn) : null,
            ownerName: data.ownerName,
            accessType: data.accessType,
            hasPassword: data.hasPassword
        }

        return convertedData;
    } catch (error) {
        return translateException(handleApiError(error as Error));
    }
}

export const UpdateTextAPI = async (id: string, updateDto: UpdateTextDto, token: string) : Promise<TextWithContentDto | ExceptionDto> => {
    try {
        const url = process.env.REACT_APP_SERVER_URL_PATH + "api/text/" + id;

        const response = await axios.put<TextWithContentInput>(url, updateDto, {
            headers: {
                "Authorization" : `Bearer ${token}`
            }
        });

        const data = response.data as TextWithContentInput;

        const convertedData: TextWithContentDto = {
            id: data.id,
            title: data.title,
            description: data.description,
            syntax: data.syntax,
            tags: data.tags,
            content: data.content,
            createdOn: new Date(data.createdOn),
            updatedOn: data.updatedOn ? new Date(data.updatedOn) : null,
            ownerName: data.ownerName,
            accessType: data.accessType,
            hasPassword: data.hasPassword
        };

        return convertedData;
    } catch (error) {
        return translateException(handleApiError(error as Error))
    }
}

export const CreateTextAPI = async (createDto: CreateTextDto, token: string) : Promise<TextWithoutContentDto | ExceptionDto> => {
    try {
        const url = process.env.REACT_APP_SERVER_URL_PATH + "api/text/";

        const response = await axios.post<TextWithoutContentInput>(url, createDto, {
            headers: {
                "Authorization" : `Bearer ${token}`
            }
        });

        const data = response.data as TextWithoutContentInput;

        const convertedData: TextWithoutContentDto = {
            id: data.id,
            title: data.title,
            description: data.description,
            syntax: data.syntax,
            tags: data.tags,
            createdOn: new Date(data.createdOn),
            updatedOn: data.updatedOn ? new Date(data.updatedOn) : null,
            ownerName: data.ownerName,
            accessType: data.accessType,
            hasPassword: data.hasPassword
        };

        return convertedData;
    } catch (error) {
        return translateException(handleApiError(error as Error))
    }
}

export const DeleteTextAPI = async (id: string, token: string) : Promise<null | ExceptionDto> => {
    try {
        const url = process.env.REACT_APP_SERVER_URL_PATH + "api/text/" + id;

        console.log(url);
        const result = await axios.delete(url, {
            headers: {
                "Authorization" : `Bearer ${token}`
            }
        })

        return null;
    } catch (error) {
        return translateException(handleApiError(error as Error))
    }
}