using VCardManager.Core;

namespace VCardManager.CLI;

// Concreet implementatie van IConsole die rechtstreeks gebruikmaakt van System.Console.
// Wordt gebruikt in de productieomgeving (de echte CLI).
public class SystemConsole : IConsole
{
    // Schrijft een regel naar de console met line break
    public void WriteLine(string message) => Console.WriteLine(message);

    // Schrijft een tekst naar de console zonder line break (bv. prompt)
    public void Write(string message) => Console.Write(message);

    // Leest een volledige lijn van de gebruiker.
    // Als null (kan gebeuren in uitzonderlijke gevallen), geef lege string terug.
    public string ReadLine() => Console.ReadLine() ?? string.Empty;
}