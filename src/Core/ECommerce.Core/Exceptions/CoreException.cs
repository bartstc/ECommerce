namespace ECommerce.Core.Exceptions;

public abstract record CoreException(string Message)
{
    public sealed record Error(string Message) : CoreException(Message);

    public sealed record BusinessRuleError(string Message) : CoreException(Message);
}