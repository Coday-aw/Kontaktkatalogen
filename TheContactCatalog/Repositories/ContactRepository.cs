using TheContactCatalog.Exceptions;
using TheContactCatalog.Models;
using TheContactCatalog.Validators;
namespace TheContactCatalog.Repositories;

public class ContactRepository : IContactRepository
{
    private readonly Dictionary<string, Contact> _byId = new();
    private readonly HashSet<string> _emails = new(StringComparer.OrdinalIgnoreCase);
    private readonly EmailValidator _emailValidator = new();

    //Add a new contact
    public void Add(Contact contact)
    {
        if (!_emailValidator.IsValidEmail(contact.Email)) 
            throw new InvalidEmailException(contact.Email);
        
        if(!_emails.Add(contact.Email))
            throw new DuplicateEmailException(contact.Email);
        
        _byId.Add(contact.Id, contact);
    }
    
    //get all contacts from the dictionary 
    public IEnumerable<Contact> GetAll() => _byId.Values;

    //get contacts by name search or email
    public IEnumerable<Contact> SearchByNameOrEmail(string name) =>
        _byId
            .Values.Where(c => c.Name.Contains(name, StringComparison.OrdinalIgnoreCase) || c.Email.Contains(name, StringComparison.OrdinalIgnoreCase));

    //get contacts by tag
    public IEnumerable<Contact> FilterByTag(string tag) =>
        _byId.Values.Where(c => c.Tags.Contains(tag, StringComparer.OrdinalIgnoreCase)).OrderBy(c => c.Name);
}