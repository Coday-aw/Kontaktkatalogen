using TheContactCatalog.Validators;

namespace TheContactCatalog.Tests;

public class EmailValidatorTests
{
    [Theory]
    [InlineData("codaygmail.com")]
    [InlineData("null")]
    [InlineData("coday awesome.com")]
    
    public void IsValidEmail_MissingAtSign_ReturnsFalse(string email)
    {
        var validator = new EmailValidator();
        var result = validator.IsValidEmail(email);
         
        Assert.False(result);
    }

    [Theory]
    [InlineData("test@gmail.com")]
    [InlineData("bella@gmail.com")]
    [InlineData("test123@gmail.com")]
    public void IsValidEmail_ValidEmail_ReturnsTrue(string email)
    {
        var validator = new EmailValidator();
        var result = validator.IsValidEmail(email);
        
        Assert.True(result);
    }
}