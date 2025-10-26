
using TheContactCatalog.Exceptions;
using TheContactCatalog.Models;
using TheContactCatalog.Repositories;
using TheContactCatalog.Services;
using Microsoft.Extensions.Logging;
namespace TheContactCatalog;

public class CatalogApp ()
{
    public void MenuDisplay()
    { 
      
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        });
        var contactLogger = loggerFactory.CreateLogger<ContactService>();
        var repository = new ContactRepository();
        var service = new ContactService(repository, contactLogger);
        
        
        
         Console.WriteLine("=== Contact Catalog ===");
         while (true)
         { 
             Console.WriteLine("\n1) Add new contact\n2) List contacts\n3) Search by name or email\n4) Filter by tag\n5) Export CSV\n6) Import CSV\n0) Exit");
            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    HandleAddContact(service);
                   Console.WriteLine("\nPress Enter to continue...");
                   Console.ReadLine();
                    break;
                case "2":
                  var contacts =  service.GetAllContacts();
                  PrintContacts(contacts);
                    break;
                case "3":
                    Console.Write("Search by Name or Email: ");
                    string? searchName = Console.ReadLine();
                    
                    if (string.IsNullOrWhiteSpace(searchName))
                    {
                        Console.WriteLine("Search contact by name is empty");
                        break;
                    }

                    var SearchContacts = service.SearchByNameOrEmail(searchName);
                    if (!SearchContacts.Any())
                    {
                        Console.WriteLine("No contacts found");
                        break;
                    }
                    PrintContacts(SearchContacts);
                    break;
                case "4":
                    Console.Write("Tag: ");
                    string? tagName = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(tagName))
                    {
                       Console.WriteLine("Tag cannot be empty, try again");
                       break;
                    }
                    var filterContacts = service.FilterContactByTag(tagName);
                    PrintContacts(filterContacts);
                    break;
                case "5":
                    Console.WriteLine("Export File Name: ");
                    string? exportFileName = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(exportFileName))
                    {
                        Console.WriteLine("Export file name can not be empty");
                        break;
                    }

                    var contactsCount = service.GetAllContacts();
                    if (contactsCount.Count() < 1)
                    {
                        Console.WriteLine("You have no contacts to export");
                        break;
                    }
                    try
                    {
                        string exportDesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                        string exportFilePath = Path.Combine(exportDesktopPath, exportFileName);
                        File.WriteAllText(exportFilePath, service.ExportAllContactsToCsv());
                        Console.WriteLine($"[Export complete!] {contactsCount.Count()} contacts exported");
                    }
                    catch
                    {
                        Console.WriteLine("Error Exporting CSV, please try again");
                    }
                    Console.WriteLine("\nPress Enter to continue...");
                    Console.ReadLine();
                    break;
                case "6":
                    Console.WriteLine("Write file name (File must be on desktop): ");
                    string? fileName = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(fileName))
                    {
                        Console.WriteLine("File name can not be empty");
                        break;
                    }
                    string importDesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    string importFilePath = Path.Combine(importDesktopPath, fileName);
                    if (!File.Exists(importFilePath))
                    {
                        Console.WriteLine($"File {importFilePath} does not exist, try again");
                        break;
                    }
                    service.ImportFromCsv(importFilePath);
                    Console.WriteLine("\nPress Enter to continue...");
                    Console.ReadLine();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid choice, please try again");
                    continue;
            }
         }
    }
    
    
    //Helper method for printing contacts 
    private static void PrintContacts(IEnumerable<Contact> contacts)
    {
        if (!contacts.Any())
        {
            Console.WriteLine("No contacts found");
            return;
        }
        foreach (var contact in contacts)
        {
            Console.WriteLine($"-({contact.Id}) {contact.Name} <{contact.Email}> [{string.Join(", ", contact.Tags)}]");
        }
        Console.WriteLine("\nPress Enter to continue...");
        Console.ReadLine();
    }

    private static void HandleAddContact(ContactService service)
    {
        Console.Write("Id: ");
        string? id = Console.ReadLine();
        
        Console.Write("Name: ");
        string? name = Console.ReadLine();
        
        Console.Write("Email: ");
        string? email = Console.ReadLine();
        
        Console.Write("Tags (separated by comma): ");
        string? tagsInput = Console.ReadLine();

        var tags = tagsInput?.Split(",", 
                       StringSplitOptions.RemoveEmptyEntries |
                       StringSplitOptions.TrimEntries).ToList() ??
                   new List<string>();

        try
        {
            var newContact = new Contact(id, name, email, tags);
            service.AddContact(newContact);
        }
        catch (InvalidEmailException e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }
        catch (DuplicateEmailException e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Unknown Error: {e.Message}");
        }
    }
}