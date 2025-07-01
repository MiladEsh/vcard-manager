namespace VCardManager.Core.Interfaces;

// Deze interface abstraheert bestandsoperaties.
// Dit laat toe om echte bestandstoegang (via System.IO) te vervangen door een mock of in-memory implementatie tijdens testing.
public interface IFileStore
{
    // Controleert of het opgegeven pad al bestaat als bestand
    bool Exists(string path);

    // Leest de volledige inhoud van een bestand als één string
    string ReadAllText(string path);

    // Schrijft volledige tekst naar een bestand (overschrijft bestaande inhoud)
    void WriteAllText(string path, string contents);

    // Voegt tekst toe aan het einde van een bestaand bestand
    void AppendAllText(string path, string contents);
}