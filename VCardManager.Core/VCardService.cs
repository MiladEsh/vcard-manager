
using VCardManager.Core.Models;

namespace VCardManager.Core;

// Deze interface definieert de logische functionaliteiten voor het beheren van vCard-contacten.
// Ze wordt geïmplementeerd door VCardService en gebruikt door het menu (UI-laag).
public interface IVCardService
{
    // Geeft een lijst terug van alle opgeslagen contacten
    List<VCardContact> GetAllContacts();

    // Voegt een nieuw contact toe aan de contactenlijst
    void AddContact(VCardContact contact);

    // Verwijdert een contact op basis van de volledige naam (voor- + achternaam)
    // Retourneert true als het contact bestond en verwijderd werd
    bool DeleteContact(string fullName);

    // Zoekt één contact op basis van de naam
    // Retourneert null als het contact niet werd gevonden
    VCardContact? SearchContact(string name);

    // Exporteert een contact naar een apart .vcf-bestand
    // Retourneert true als het contact werd gevonden en succesvol geëxporteerd
    bool ExportContact(string fullName, string filePath);
}

// Deze klasse vormt de logische laag (service-laag) en implementeert IVCardService.
// Ze coördineert acties zoals toevoegen, zoeken, verwijderen en exporteren van contacten,
// door gebruik te maken van een repository (data-opslag).
public class VCardService : IVCardService
{
    private readonly IVCardRepository repository;

    // Dependency injection: de service krijgt een repository mee via de constructor.
    public VCardService(IVCardRepository repository)
    {
        this.repository = repository;
    }

    // Voegt een nieuw contact toe aan de opslag
    public void AddContact(VCardContact contact)
    {
        repository.Add(contact);
    }

    // Verwijdert een contact op basis van volledige naam.
    // Geeft true terug als het contact bestond en verwijderd is.
    public bool DeleteContact(string fullName)
    {
        var existing = repository.FindByName(fullName);
        if (existing == null) return false;

        repository.Delete(fullName);
        return true;
    }

    // Exporteert een contact naar een apart .vcf-bestand.
    // Retourneert true als het contact werd gevonden en geëxporteerd.
    public bool ExportContact(string fullName, string filePath)
    {
        var contact = repository.FindByName(fullName);
        if (contact == null) return false;

        repository.ExportToFile(contact, filePath);
        return true;
    }

    // Haalt een lijst op van alle opgeslagen contacten.
    public List<VCardContact> GetAllContacts()
    {
        return repository.GetAll();
    }

    // Zoekt een contact op basis van de volledige naam.
    public VCardContact? SearchContact(string name)
    {
        return repository.FindByName(name);
    }
}