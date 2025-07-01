using VCardManager.Core.Models;

namespace VCardManager.Core.Interfaces;

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