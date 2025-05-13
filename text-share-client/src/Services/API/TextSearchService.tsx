import axios, { AxiosResponse } from "axios";
import { PaginatedResponseDto, PaginationDto, SortDto, TextFilterDto, TextWithoutContentDto, TextWithoutContentInput } from "../../Dtos";
import { translateExceptions } from "../TranslatorService";
import { handleApiError } from "../ErrorHandler";

export const SearchTextsAPI = async (pagination: PaginationDto, sort: SortDto, filter: TextFilterDto, token: string | null) : Promise<PaginatedResponseDto<TextWithoutContentDto> | string[]> => {
    try {
        var url = process.env.REACT_APP_SERVER_URL_PATH + "api/text" +
            `?pageNumber=${pagination.pageNumber}&pageSize=${pagination.pageSize}` +
            `&sortBy=${sort.sortBy}&sortAscending=${sort.sortAscending}`

        if (filter.ownerName) url += `&ownerName=${encodeURIComponent(filter.ownerName)}`
        if (filter.title) url += `&title=${encodeURIComponent(filter.title)}`
        if (filter.tags) filter.tags.forEach(t => url += `&tags=${encodeURIComponent(t)}`)
        if (filter.syntax) url += `&syntax=${encodeURIComponent(filter.syntax)}`
        if (filter.accessType) url += `&accessType=${encodeURIComponent(filter.accessType)}`
        if (filter.hasPassword) url += `&hasPassword=${encodeURIComponent(filter.hasPassword)}`
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
        return translateExceptions(handleApiError(error as Error));
    }
}