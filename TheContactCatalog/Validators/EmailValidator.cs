namespace TheContactCatalog.Validators;

public class EmailValidator
{
    public bool IsValidEmail(string email)
    {
        try
        {
            var emailAddress = System.Text.RegularExpressions.Regex.IsMatch(
                email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return emailAddress;
        }
        catch { return false; }
    }
}