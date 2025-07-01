namespace VCardManager.Core;

// Interface die de standaardconsole abstract maakt, zodat je deze kan testen of vervangen.
public interface IConsole
{
    // Schrijft een volledige regel naar de console, gevolgd door een line break
    void WriteLine(string message);

    // Schrijft een tekst naar de console zonder line break (voor bv. prompts)
    void Write(string message);

    // Leest een volledige lijn van de console (gebruikerstypen)
    string ReadLine();
}