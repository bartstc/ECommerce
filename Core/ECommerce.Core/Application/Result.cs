using ECommerce.Core.Exceptions;

namespace ECommerce.Core.Application;

public class Result<T>
{
    public bool IsSuccess { get; set; }
    public T Value { get; set; }
    public ErrorResponse Error { get; set; }

    public static Result<T> Success(T value) => new Result<T>
    {
        IsSuccess = true,
        Value = value
    };

    public static Result<T> Failure(Exception error) => new Result<T>
    {
        IsSuccess = false,
        Error = new ErrorResponse(error)
    };

    public static Result<T> FromException(Exception ex) => ex switch
    {
        BusinessRuleException bre => Failure(bre),
        ApplicationLogicException ale => Failure(ale),
        _ => Failure(new Exception("An error occurred while processing the request"))
    };
}

public class ErrorResponse
{
    public string Message { get; set; }
    private string ExceptionType { get; set; }

    public ErrorResponse(Exception ex)
    {
        Message = ex.Message;
        ExceptionType = ex.GetType().Name;
    }

    public bool TypeOf<T>() where T : Exception
    {
        return ExceptionType == typeof(T).Name;
    }
}