using VCardManager.Core.Interfaces;

namespace VCardManager.CLI;

// Concreet implementatie van IFileStore die echte bestandsoperaties uitvoert via System.IO.
// Wordt gebruikt in de productieomgeving (niet in tests).
public class FileSystemStore : IFileStore
{
    // Controleert of het opgegeven bestand bestaat
    public bool Exists(string path)
    {
        return File.Exists(path);
    }

    // Leest de volledige inhoud van het bestand als één string
    public string ReadAllText(string path)
    {
        return File.ReadAllText(path);
    }

    // Overschrijft de volledige inhoud van het bestand met nieuwe inhoud
    public void WriteAllText(string path, string contents)
    {
        File.WriteAllText(path, contents);
    }

    // Voegt nieuwe inhoud toe aan het einde van het bestand, zonder te overschrijven
    public void AppendAllText(string path, string contents)
    {
        File.AppendAllText(path, contents);
    }
}