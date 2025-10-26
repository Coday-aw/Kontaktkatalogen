## Contact Catalog

Kontaktkatalogen är en konsolbaserad kontaktkatalog byggd i C# (.NET 8) med fokus på datastrukturer, LINQ och testbarhet.
kontaktkatalogen har flera fetuares som användaren kan använda sig av. 

## Features
Lägg till, lista, sök och filtrera kontakter  
CSV-export: Exportera alla kontakter till CSV-fil på Desktop  
CSV-import: Importera kontakter från CSV-fil med felsummering  
Validering: Email-validering med egna exceptions  
Dubblettskydd: Automatisk kontroll av dubbla email-adresser  
Taggning: Varje kontakt kan ha flera taggar  
Loggning: Microsoft.Extensions.Logging för händelseloggning  
Unit tests: Omfattande tester med xUnit och Moq  

## Designval
Design valen har jag förklarat i pull requesten.   


## Projektstruktur  
ContactCatalog/
├── ContactCatalog/                 # Huvudprojekt
│   ├── Models/
│   │   └── Contact.cs             # Kontakt-modell
│   ├── Repositories/
│   │   ├── IContactRepository.cs  # Repository-interface
│   │   └── ContactRepository.cs   # In-memory implementation
│   ├── Services/
│   │   └── ContactService.cs      # Affärslogik
│   ├── Exceptions/
│   │   ├── InvalidEmailException.cs
│   │   └── DuplicateEmailException.cs
│   ├── Validators/
│   │   └── EmailValidator.cs      # Email-validering
│   └── CatalogApp.cs              # Huvudprogram & meny
│
├── ContactCatalog.Tests/           # Test-projekt
│   ├── EmailValidatorTests.cs     # Validator-tester
│   └── ContactServiceTests.cs     # Service-tester med Moq
│
└── ContactCatalog.sln              # Solution-fil

## Installation & Körning

git clone
cd ContactCatalog

cd ContactCatalog
dotnet run

cd ContactCatalog.Tests
dotnet test



<img width="438" height="751" alt="Skärmavbild 2025-10-26 kl  11 43 01" src="https://github.com/user-attachments/assets/589bb406-0d7c-435e-a0e8-7ffab4e407c9" />

