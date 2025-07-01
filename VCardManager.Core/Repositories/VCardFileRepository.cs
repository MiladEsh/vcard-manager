using VCardManager.Core.Interfaces;
using VCardManager.Core.Models;

namespace VCardManager.Core.Repositories;

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
        var vcard = ToVCard(contact);
        fileStore.AppendAllText(filePath, vcard + Environment.NewLine);
    }

    // Verwijdert een contact door alle andere contacten opnieuw te schrijven (exclusie op naam)
    public void Delete(string fullName)
    {
        var contacts = GetAll()
            .Where(c => !string.Equals(c.FullName, fullName, StringComparison.OrdinalIgnoreCase))
            .ToList();

        var allText = string.Join(Environment.NewLine, contacts.Select(ToVCard));
        fileStore.WriteAllText(filePath, allText + Environment.NewLine);
    }

    // Leest en parseert alle contacten uit het .vcf-bestand
    public List<VCardContact> GetAll()
    {
        if (!fileStore.Exists(filePath))
            return new List<VCardContact>();

        var content = fileStore.ReadAllText(filePath);
        return ParseContacts(content);
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
        var vcard = ToVCard(contact);
        fileStore.WriteAllText(exportPath, vcard);
    }

    // ⬇️ Helper: Zet een VCardContact om naar een string in vCard-formaat
    private string ToVCard(VCardContact c) => 
$@"BEGIN:VCARD
FN:{c.FirstName} {c.LastName}
TEL:{c.Phone}
EMAIL:{c.Email}
END:VCARD";

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