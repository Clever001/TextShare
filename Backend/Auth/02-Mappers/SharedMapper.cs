using Auth.Grpc;
using Auth.Models;
using Auth.Other;
using Shared;
using Shared.Exceptions;

namespace Auth.Mappers;

public class SharedMapper {
    public GrpcException ToException(IApiException exception) {
        var grpcException = new GrpcException {
            Code = exception.Code,
            CodeNumber = exception.CodeNumber,
            Description = exception.Description,
        };
        var details = exception.Details;
        if (details != null && details.Count != 0) {
            grpcException.Details.AddRange(details);
        }
        return grpcException;
    }

    public PaginationCommand ToPaginationCommand(PaginationPage value) {
        return new PaginationCommand {
            PageNumber = value.PageNumber,
            PageSize = value.PageSize
        };
    }

    public PaginationResponse ToPaginationResponse<T>(PaginatedResponse<T> serviceResponse) {
        return new PaginationResponse {
            TotalItems = serviceResponse.TotalItems,
            TotalPages = serviceResponse.TotalPages,
            CurrentPage = serviceResponse.CurrentPage,
            PageSize = serviceResponse.PageSize
        };
    }

    public UserWithToken ToUserWithToken(ValueTuple<AppUser, string> resultValue) {
        var (user, jwtToken) = resultValue;
        return new UserWithToken {
            Id = user.Id,
            Name = user.UserName,
            Email = user.Email,
            Token = jwtToken
        };
    }

    public EmptyResult ToEmptyResult(Result result) {
        if (result.IsSuccess) {
            return new EmptyResult();
        } else {
            return new EmptyResult {
                Exception = ToException(result.Exception)
            };
        }
    }

    public BooleanResult ToBooleanResult(Result<bool> result) {
        if (result.IsSuccess) {
            return new BooleanResult {
                AreFriends = result.Value
            };
        } else {
            return new BooleanResult {
                Exception = ToException(result.Exception)
            };
        }
    }
}