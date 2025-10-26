using TheContactCatalog.Models;

namespace TheContactCatalog.Repositories;

public interface IContactRepository
{
    void Add(Contact contact);
    IEnumerable<Contact> GetAll();
    IEnumerable<Contact> SearchByNameOrEmail(string name);
    IEnumerable<Contact> FilterByTag(string tag);
}