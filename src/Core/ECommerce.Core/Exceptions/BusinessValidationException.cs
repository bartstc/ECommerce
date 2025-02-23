namespace ECommerce.Core.Exceptions;

public class BusinessValidationException(string message) : Exception(message) { }