using Auth.Dto.Account;
using Auth.Model;
using Auth.Other;
using Auth.Repository.Interface;
using Auth.Service.Interface;
using Auth.Validator;
using Shared;
using Shared.ApiError;

namespace Auth.Service.Impl;

public class UserService (
    IUserRepository userRepository,
    ITokenService tokenService,
    ILogger<UserService> logger
) : IUserService {

    public async Task<Result<UserWithTokenDto>> RegisterUser(RegisterUserDto registerDto) {
        try {
            var user = new User {
                UserName = registerDto.Name,
                Email = registerDto.Email
            };
 
            var createResult = await userRepository.CreateUser(user, registerDto.Password);
            if (createResult.Succeeded) {
                var appendResult = await userRepository.AddUserToRole(user, "User");
                if (appendResult.Succeeded) {
                    var token = tokenService.CreateToken(user);
                    return Result<UserWithTokenDto>.Success(new UserWithTokenDto(user, token));
                }

                await userRepository.DeleteUser(user);

                throw BusinessLogicException.CreateFromIdentityErrors(
                    "Error apeared while adding user to role.",
                    appendResult.Errors
                );
            }

            return Result<UserWithTokenDto>.Failure(new BadRequestApiError(
                "Cannot create user with provided information.",
                createResult.Errors.Select(e => e.Description).ToList()
            ));
        }
        catch (Exception ex) {
            logger.LogError("Error on user register: \n {Exeption}", ex.ToString());
            throw new BusinessLogicException("Exception throwed while registering user.", ex);
        }
    }

    public async Task<Result<UserWithTokenDto>> LoginUser(LoginUserDto loginDto) {
        User? user = await userRepository.FindByName(loginDto.NameOrEmail);
        user ??= await userRepository.FindByEmail(loginDto.NameOrEmail);

        if (user == null) return Result<UserWithTokenDto>.Failure(new UnauthorizedApiError());

        bool passwordIsValid = await userRepository.IsValidPassword(user, loginDto.Password);
        if (!passwordIsValid) return Result<UserWithTokenDto>.Failure(new UnauthorizedApiError());

        var token = tokenService.CreateToken(user);
        return Result<UserWithTokenDto>.Success(new UserWithTokenDto(user, token));
    }

    public async Task<Result<UserWithTokenDto>> UpdateUser(UpdateUserDto updateDto) {
        var user = await userRepository.FindById(updateDto.InitialUserId);
        if (user == null) return Result<UserWithTokenDto>.Failure(new NotFoundApiError());

        if (updateDto.NewName != null && updateDto.NewName != user.UserName) {
            user.UserName = updateDto.NewName;
        }

        if (updateDto.NewEmail != null && updateDto.NewEmail != user.Email) {
            user.Email = updateDto.NewEmail;
        }

        var updateResult = await userRepository.UpdateUser(user);
        if (!updateResult.Succeeded) {
            return Result<UserWithTokenDto>.Failure(new BadRequestApiError(
                "Cannot update user with provided information.",
                updateResult.Errors.Select(e => e.Description).ToList()
            ));
        }

        var token = tokenService.CreateToken(user);

        return Result<UserWithTokenDto>.Success(new UserWithTokenDto(user, token));
    }

    public async Task<Result<PaginatedResponse<User>>> GetUsers(
        UsersPagedFilterDto usersFilter
    ) {
        string? searchName = usersFilter.UserName;

        var paginationValidator = new PaginationPageValidator(usersFilter);
        paginationValidator.PerformValidityCheck();
        if (!paginationValidator.IsValid) {
            return Result<PaginatedResponse<User>>.Failure(
                new BadRequestApiError(
                    "Validation error while getting users info",
                    paginationValidator.ValidationErrors
                )
            );
        }
        
        var queryFilterBuilder = new QueryFilterBuilder<User, string>();
        queryFilterBuilder.WithPagination(usersFilter);
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
        return Result<PaginatedResponse<User>>.Success(new PaginatedResponse<User>(
            itemsSelection: usersCollection,
            totalItemsCount: countOfAllUsers,
            currentPage: queryFilter.PageNumber,
            pageSize: queryFilter.PageSize
        ));
    }
}