using TheContactCatalog.Exceptions;
using TheContactCatalog.Models;
using TheContactCatalog.Repositories;
using Microsoft.Extensions.Logging;
using System.Text;

namespace TheContactCatalog.Services;

public class ContactService
{
    private readonly IContactRepository _contactRepository;
    private ILogger <ContactService> _logger;
    
    public ContactService(IContactRepository contactRepository, ILogger<ContactService> logger)
    {
        _contactRepository = contactRepository;
        _logger = logger;
    }
    
    // Add method from contact repository is implemented here with try catch 
    public void AddContact(Contact contact)
    {
        try
        {
            _contactRepository.Add(contact);
            _logger.LogInformation("Contact added to the catalog!");
        }
        catch (InvalidEmailException e)
        {
            _logger.LogWarning(e.Message);
            throw;
        }
        catch (DuplicateEmailException e)
        {
            _logger.LogWarning(e.Message);
            throw;
        } catch(Exception ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }
    }
    
    // Method to get all contacts 
    public IEnumerable<Contact> GetAllContacts()
    {
        return _contactRepository.GetAll();
    }

    //method to get contact by search and print 
    public IEnumerable<Contact> SearchByNameOrEmail(string name)
    {
        return _contactRepository.SearchByNameOrEmail(name);
    }

    //method to get conatct by tag and print 
    public IEnumerable<Contact> FilterContactByTag(string tag)
    {
        return _contactRepository.FilterByTag(tag);
    }
    
    // converts contacts to CSV
    public string ToCsv(IEnumerable<Contact> contacts)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Id,Name,Email,Tags");
        foreach (var c in contacts)
        {
            var tags = string.Join('|', c.Tags);
            sb.AppendLine($"{c.Id},{c.Name},{c.Email},{tags}");
        }
        return sb.ToString();
    }

    // Method to export contacts 
    public string ExportAllContactsToCsv()
    {
        var allContacts = _contactRepository.GetAll();
        return ToCsv(allContacts);
    }

    // method to import from CSV, returns 2 int (total successful and failed imports)
    public (int success, int failed) ImportFromCsv(string csvFilePath)
    {
        int success = 0;
        int failed = 0;
        var lines = File.ReadAllLines(csvFilePath).Skip(1);
        foreach (var line in lines)
        {
            var columns = line.Split(',');
            try
            {
                var contact = new Contact
                (
                     columns[0], 
                     columns[1],
                    columns[2],
                     columns[3].Split("|", StringSplitOptions.RemoveEmptyEntries
                                                 | StringSplitOptions.TrimEntries).ToList()
                );
                _contactRepository.Add(contact);
                success++;
            }
            catch (Exception)
            {
                failed++;
            }
        }
        _logger.LogInformation("{Success} contacts imported, {Failed} failed!", success, failed);
        return (success, failed);
    }




}