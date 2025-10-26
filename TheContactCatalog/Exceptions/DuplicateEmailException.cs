namespace TheContactCatalog.Exceptions;

public class DuplicateEmailException : Exception
{
    public DuplicateEmailException(string email) : base($"Email: {email} is taken, please try another one.") { } 
}