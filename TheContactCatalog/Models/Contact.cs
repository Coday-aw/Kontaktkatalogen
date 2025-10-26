namespace TheContactCatalog.Models;

public class Contact
{
    public string? Id { get; set; } 
    public string? Name { get; set; } 
    public string? Email { get; set; }
    public List<string> Tags { get; set; } 

    public Contact(string id, string name, string email, List<string> tags)
    {
        Id = id;
        Name = name;
        Email = email;
        Tags = tags;
    }
}