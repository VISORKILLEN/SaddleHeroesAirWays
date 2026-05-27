namespace SaddleHeroesAirWays.API.Services
{
    public enum ServiceResultStatus
    {
        Success,
        NotFound,
        ValidationError
    }
    public record ServiceResult(ServiceResultStatus Status, string? ErrorMessage = null)
    {
        public bool Success => Status == ServiceResultStatus.Success;

        public static ServiceResult Ok() => new(ServiceResultStatus.Success);
        public static ServiceResult NotFound(string errorMessage) => new(ServiceResultStatus.NotFound, errorMessage);
        public static ServiceResult ValidationError(string errorMessage) => new(ServiceResultStatus.ValidationError, errorMessage);
    }

    public record ServiceResult<T>(ServiceResultStatus Status, T? Data = default, string? ErrorMessage = null)
    {
        public bool Success => Status == ServiceResultStatus.Success;

        public static ServiceResult<T> Ok(T data) => new(ServiceResultStatus.Success, data);
        public static ServiceResult<T> NotFound(string errorMessage) => new(ServiceResultStatus.NotFound, default, errorMessage);
        public static ServiceResult<T> ValidationError(string errorMessage) => new(ServiceResultStatus.ValidationError, default, errorMessage);
    }
}
