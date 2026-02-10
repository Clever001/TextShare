using Auth.Dto.Document;
using Auth.Repository.Interface;
using Auth.Service.Interface;
using Microsoft.AspNetCore.Http.HttpResults;
using Shared.ApiError;
using Shared.Result;

namespace Auth.Service.Impl;

public class DocumentService(
    IDocumentRepository documentRepository
) : ServiceBase, IDocumentService {
    public async Task<ApiResult> SaveDocumentMetadata(SaveDocumentRequest req) {
        var (IsValidRequestDto, possibleError) = CheckValidity(req);
        if (!IsValidRequestDto) {
            return ApiResult.Failure(possibleError);
        }

        bool containsSameId = await documentRepository.ContainsById(req.DocumentId);
        if (containsSameId) {
            return ApiResult.Failure(new BadRequestApiError(""
        }
    }

    public async Task<ApiResult> UpdateDefaultRoleForDocument(UpdateDefaultRoleRequest req) {
        var (IsValidRequestDto, possibleError) = CheckValidity(req);
        if (!IsValidRequestDto) {
            return ApiResult.Failure(possibleError);
        }
    }

    public async Task<ApiResult> DeleteDocumentMetadata(DeleteDocumentRequest req) {
        var (IsValidRequestDto, possibleError) = CheckValidity(req);
        if (!IsValidRequestDto) {
            return ApiResult.Failure(possibleError);
        }
    }

    public async Task<ApiResult<UserRoleDto>> GetUserRoleForDocument(UserRoleRequest req) {
        var (IsValidRequestDto, possibleError) = CheckValidity(req);
        if (!IsValidRequestDto) {
            return ApiResult<UserRoleDto>.Failure(possibleError);
        }
    }
}