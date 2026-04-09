using Auth.Dto.Account;
using Auth.Model;
using Auth.Other;
using Auth.Repository.Interface;
using Auth.Service.Interface;
using Shared.ApiError;
using Shared.Result;
using Auth.CustomException;

namespace Auth.Service.Impl;

public class UserService (
    IUserRepository userRepository,
    ITokenService tokenService,
    ILogger<UserService> logger
) : ServiceBase, IUserService {

    public async Task<ApiResult<UserWithTokenDto>> RegisterUser(RegisterUserRequest req) {
        var (IsValidRequestDto, possibleError) = CheckValidity(req);
        if (!IsValidRequestDto) {
            return ApiResult<UserWithTokenDto>.Failure(possibleError);
        }

        try {
            var user = new User {
                UserName = req.Name,
                Email = req.Email
            };
 
            var createResult = await userRepository.CreateUser(user, req.Password);
            if (createResult.IsSuccess) {
                var token = tokenService.CreateToken(user);
                return ApiResult<UserWithTokenDto>.Success(new UserWithTokenDto(user, token));
            } else {
                return ApiResult<UserWithTokenDto>.Failure(new BadRequestApiError(
                    "Cannot create user with provided information.",
                    createResult.ErrorDetails
                ));
            }

        }
        catch (Exception ex) {
            logger.LogError("Error on user register: \n {Exeption}", ex.ToString());
            throw new BusinessLogicException("Exception throwed while registering user.", ex);
        }
    }

    public async Task<ApiResult<UserWithTokenDto>> LoginUser(LoginUserRequest req) {
        var (IsValidRequestDto, possibleError) = CheckValidity(req);
        if (!IsValidRequestDto) {
            return ApiResult<UserWithTokenDto>.Failure(possibleError);
        }

        User? user = await userRepository.FindByName(req.NameOrEmail);
        user ??= await userRepository.FindByEmail(req.NameOrEmail);

        if (user == null) return ApiResult<UserWithTokenDto>.Failure(new UnauthorizedApiError());

        bool passwordIsValid = await userRepository.IsValidPassword(user, req.Password);
        if (!passwordIsValid) return ApiResult<UserWithTokenDto>.Failure(new UnauthorizedApiError());

        var token = tokenService.CreateToken(user);
        return ApiResult<UserWithTokenDto>.Success(new UserWithTokenDto(user, token));
    }

    public async Task<ApiResult<UserWithTokenDto>> UpdateUser(UpdateUserRequest req) {
        var (IsValidRequestDto, possibleError) = CheckValidity(req);
        if (!IsValidRequestDto) {
            return ApiResult<UserWithTokenDto>.Failure(possibleError);
        }

        var user = await userRepository.FindById(req.InitialUserId);
        if (user == null) return ApiResult<UserWithTokenDto>.Failure(new NotFoundApiError());

        if (req.NewName != null && req.NewName != user.UserName) {
            user.UserName = req.NewName;
        }

        if (req.NewEmail != null && req.NewEmail != user.Email) {
            user.Email = req.NewEmail;
        }

        var updateResult = await userRepository.UpdateUser(user);
        if (!updateResult.IsSuccess) {
            return ApiResult<UserWithTokenDto>.Failure(new BadRequestApiError(
                "Cannot update user with provided information.",
                updateResult.ErrorDetails
            ));
        }

        var token = tokenService.CreateToken(user);

        return ApiResult<UserWithTokenDto>.Success(new UserWithTokenDto(user, token));
    }

    public async Task<ApiResult<PaginatedResponse<User>>> GetUsers(
        UsersPagedFilter filter
    ) {
        var (IsValidRequestDto, possibleError) = CheckValidity(filter);
        if (!IsValidRequestDto) {
            return ApiResult<PaginatedResponse<User>>.Failure(possibleError);
        }

        string? searchName = filter.UserName;
        
        var queryFilterBuilder = new QueryFilterBuilder<User, string>();
        queryFilterBuilder.WithPagination(filter);
        queryFilterBuilder.WithSort(
            keyOrder: user => user.UserName!, 
            isAscending: true
        );
        if (searchName != null) {
            queryFilterBuilder.WithWherePredicate(
                user => user.NormalizedUserName!.Contains(searchName.ToUpperInvariant())
            );
        }

        var queryFilter = queryFilterBuilder.Build();
        var (countOfAllUsers, usersCollection) = await userRepository.GetUsersCollection(
            queryFilter
        );
        return ApiResult<PaginatedResponse<User>>.Success(new PaginatedResponse<User>(
            itemsSelection: usersCollection,
            totalItemsCount: countOfAllUsers,
            currentPage: queryFilter.PageNumber,
            pageSize: queryFilter.PageSize
        ));
    }
}