using ECommerce.Core.Exceptions;

namespace ECommerce.Core.Application;

public class Result<T>
{
    public bool IsSuccess { get; set; }
    public T Value { get; set; }
    public ApplicationLogicException Error { get; set; }

    public static Result<T> Success(T value) => new Result<T>
    {
        IsSuccess = true,
        Value = value
    };

    public static Result<T> Failure(ApplicationLogicException error) => new Result<T>
    {
        IsSuccess = false,
        Error = error
    };
}
