using Auth.Other;

namespace Auth.Dto;

public interface ICheckable {
    public DtoChecker.DtoCheckResult CheckValidity();
}