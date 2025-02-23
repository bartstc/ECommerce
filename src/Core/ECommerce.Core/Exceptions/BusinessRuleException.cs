using ECommerce.Core.Domain;

namespace ECommerce.Core.Exceptions;

public class BusinessRuleException : Exception
{
    public IBusinessRuleBase Rule { get; set; }

    public BusinessRuleException(IBusinessRuleBase rule) : base(rule.Message)
    {
        Rule = rule;
    }

    public override string ToString()
    {
        return $"{Rule.GetType().FullName}: {Rule.Message}";
    }
}
