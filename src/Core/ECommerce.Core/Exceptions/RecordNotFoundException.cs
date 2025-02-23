namespace ECommerce.Core.Exceptions;

public class RecordNotFoundException(string message) : Exception(message) { }