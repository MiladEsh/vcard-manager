using VCardManager.Core.Models;

namespace VCardManager.Core.Interfaces;

// Deze interface beschrijft de laag voor permanente opslag van contacten.
// De implementatie (bv. VCardFileRepository) werkt met bestanden (vcf), maar deze interface laat toe om dit later makkelijk te vervangen.
public interface IVCardRepository
{
    // Haalt alle contacten op uit de opslag (bijv. contacts.vcf)
    List<VCardContact> GetAll();

    // Voegt een nieuw contact toe aan de opslag
    void Add(VCardContact contact);

    // Verwijdert een contact op basis van volledige naam (case-insensitive)
    void Delete(string fullName);

    // Zoekt een contact op basis van volledige naam
    // Retourneert null als het contact niet bestaat
    VCardContact? FindByName(string name);

    // Exporteert een enkel contact naar een apart bestand (.vcf)
    void ExportToFile(VCardContact contact, string filePath);
}