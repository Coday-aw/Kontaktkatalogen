using TheContactCatalog.Exceptions;
using TheContactCatalog.Repositories;
using TheContactCatalog.Services;
using TheContactCatalog.Models;
using Moq;
using Microsoft.Extensions.Logging;
using System.Linq;
namespace TheContactCatalog.Tests;

public class ContactServiceTests
{
    [Fact]
    public void AddContact_DuplicateEmail_Throws_DuplicateEmailException()
    {
        //Arrange
        var mockRepo = new Mock<IContactRepository>();

        mockRepo.Setup(r => r.Add(It.IsAny<Contact>()))
            .Throws(new DuplicateEmailException("test@gmail.com"));

    
        var service = new ContactService(mockRepo.Object, Mock.Of<ILogger<ContactService>>());

        var existingContact = new Contact("C001", "Anna", "test@gmail.com", new List<string>());
        
        // Act & Assert
        Assert.Throws<DuplicateEmailException>(() => service.AddContact(existingContact));
        mockRepo.Verify(r => r.Add(It.IsAny<Contact>()), Times.Once);
    }

    [Fact]
    public void AddContact_InvalidEmail_Throws_InvalidEmailException()
    {
        //Arrange
        var mockRepo = new Mock<IContactRepository>();
        mockRepo.Setup(r => r.Add(It.IsAny<Contact>()))
            .Throws(new InvalidEmailException("testgmail.com"));
        
       
        var service = new ContactService(mockRepo.Object, Mock.Of<ILogger<ContactService>>());
        
        var invalidContact = new Contact("C001", "Anna", "testgmail.com", new List<string>());
        
        //Act & Assert
        Assert.Throws<InvalidEmailException>(() => service.AddContact(invalidContact));
        mockRepo.Verify(r => r.Add(It.IsAny<Contact>()), Times.Once);
    }
    
    [Fact]
    public void Filter_By_Tags_Returns_Only_Matching_Contacts()
    {
        // Arrange
        var mockRepo = new Mock<IContactRepository>();

        mockRepo.Setup(r => r.FilterByTag("friend")).Returns(new List<Contact>
        {
            new Contact("C001", "Anna", "test@gmail.com", new List<string> { "friend", "football" }),
        });

      
        var service = new ContactService(mockRepo.Object, Mock.Of<ILogger<ContactService>>());

        //Act
        var result = service.FilterContactByTag("friend").ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal("Anna", result[0].Name);
        mockRepo.Verify(r => r.FilterByTag(It.Is<string>(s => s == "friend")), Times.Once);
    }
    
    [Fact]
    public void GetAllContacts_Returns_All_Contacts()
    {
        //Arrange
        var mockRepo = new Mock<IContactRepository>();

        mockRepo.Setup(r => r.GetAll()).Returns(new List<Contact>
        {
            new Contact("C001", "Anna", "test@gmail.com", new List<string> { "friend", "football" }),
            new Contact("B001", "Bella", "bella@gmail.com", new List<string> { "gym", "bestfriend" }),
        });
        
        var service = new ContactService(mockRepo.Object, Mock.Of<ILogger<ContactService>>());
        
        //Act
        var result = service.GetAllContacts().ToList();
        
        //Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, c => c.Id == "B001" && c.Name == "Bella");
        Assert.Contains(result, c => c.Id == "C001" && c.Name == "Anna");
        mockRepo.Verify(r => r.GetAll(), Times.Once);
    }
    
    [Theory]
    [InlineData("Anna")]
    [InlineData("test@gmail.com")]
    public void GetContact_By_Name_Or_Email_Returns_Contact(string search)
    {
        var mockRepo = new Mock<IContactRepository>();
        mockRepo.Setup(r => r.SearchByNameOrEmail(search)).Returns(new List<Contact>
        {
            new Contact("C001", "Anna", "test@gmail.com", new List<string> { "friend", "football" }),
        });
        
        
        var service = new ContactService(mockRepo.Object, Mock.Of<ILogger<ContactService>>());
        
        var result = service.SearchByNameOrEmail(search).ToList();

        Assert.Single(result);
        Assert.Equal("Anna", result[0].Name);
        Assert.Equal("test@gmail.com", result[0].Email);
        mockRepo.Verify(r => r.SearchByNameOrEmail(It.Is<string>(s => s == search)), Times.Once);
    }
}