using VCardManager.Core.Interfaces;
using VCardManager.Core.Models;

namespace VCardManager.Core;

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

// Deze klasse implementeert IVCardRepository en slaat contacten op in een .vcf-bestand.
// Ze maakt gebruik van een IFileStore (voor I/O abstractie) zodat dit testbaar blijft.
public class VCardFileRepository : IVCardRepository
{
    private readonly string filePath;      // Pad naar contacts.vcf
    private readonly IFileStore fileStore; // Abstractie van bestandstoegang

    // Constructor krijgt pad en fileStore mee via dependency injection
    public VCardFileRepository(string filePath, IFileStore fileStore)
    {
        this.filePath = filePath;
        this.fileStore = fileStore;
    }

    // Voegt een contact toe door zijn vCard representatie aan het bestand toe te voegen
    public void Add(VCardContact contact)
    {

        if (string.IsNullOrWhiteSpace(contact.FullName))
            throw new ArgumentException("Contact moet een volledige naam hebben.");

        var vCardString = ToVCard(contact);


        fileStore.AppendAllText(filePath, vCardString + Environment.NewLine);


    }

    // Verwijdert een contact door alle andere contacten opnieuw te schrijven (exclusie op naam)
    public void Delete(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Volledige naam mag niet leeg zijn.");

        var contacts = GetAll();
        var updatedContacts = contacts.Where(c => !c.FullName.Equals(fullName, StringComparison.OrdinalIgnoreCase)).ToList();

        var vCardString = updatedContacts.Select(ToVCard);
        fileStore.WriteAllText(filePath, string.Join(Environment.NewLine, vCardString));

    }

    // Leest en parseert alle contacten uit het .vcf-bestand
    public List<VCardContact> GetAll()
    {
        if (fileStore.Exists(filePath))
        {
            var rawContent = fileStore.ReadAllText(filePath);
            return ParseContacts(rawContent);
        }
        else
        {
            return new List<VCardContact>();
        }
    }

    // Zoekt een contact op basis van de volledige naam
    public VCardContact? FindByName(string name)
    {
        return GetAll()
            .FirstOrDefault(c => c.FullName.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    // Exporteert één contact naar een los .vcf-bestand
    public void ExportToFile(VCardContact contact, string exportPath)
    {
        if (contact == null)
            throw new ArgumentNullException(nameof(contact), "Contact mag niet null zijn.");

        if (string.IsNullOrWhiteSpace(exportPath))
            throw new ArgumentException("Export pad mag niet leeg zijn.", nameof(exportPath));

        var vCardString = ToVCard(contact);
        fileStore.WriteAllText(exportPath, vCardString);
    }

    // ⬇️ Helper: Zet een VCardContact om naar een string in vCard-formaat
    private string ToVCard(VCardContact c) => "BEGIN:VCARD\n" +
        $"FN:{c.FirstName} {c.LastName}\n" +
        $"TEL:{c.Phone}\n" +
        $"EMAIL:{c.Email}\n" +
        "END:VCARD";


    // ⬇️ Helper: Parseert vCard-tekst uit bestand naar een lijst van VCardContact objecten
    private List<VCardContact> ParseContacts(string raw)
    {
        var contacts = new List<VCardContact>();
        var cards = raw.Split("BEGIN:VCARD", StringSplitOptions.RemoveEmptyEntries);

        foreach (var card in cards)
        {
            var lines = card.Split("\n", StringSplitOptions.RemoveEmptyEntries);
            var contact = new VCardContact();

            foreach (var line in lines)
            {
                var trimmed = line.Trim();
                if (trimmed.StartsWith("FN:"))
                {
                    var parts = trimmed.Substring(3).Split(' ', 2);
                    contact.FirstName = parts.ElementAtOrDefault(0) ?? "";
                    contact.LastName = parts.ElementAtOrDefault(1) ?? "";
                }
                else if (trimmed.StartsWith("TEL:"))
                    contact.Phone = trimmed.Substring(4).Trim();
                else if (trimmed.StartsWith("EMAIL:"))
                    contact.Email = trimmed.Substring(6).Trim();
            }

            // Enkel toevoegen als naam niet leeg is
            if (!string.IsNullOrWhiteSpace(contact.FullName))
                contacts.Add(contact);
        }

        return contacts;
    }
}

