using System.Text.RegularExpressions;
using VCardManager.Core.Models;

namespace VCardManager.Core;

public interface IMenu
{
    void Run();
}



// De Menu-klasse is verantwoordelijk voor alle gebruikersinteractie in de console.
public class Menu : IMenu
{
    private readonly IVCardService _service; // logica-laag, toegang tot contacten
    private readonly IConsole _console;     // abstractielaag voor System.Console

    public Menu(IVCardService service, IConsole console)
    {
        _service = service;
        _console = console;
    }

    // De hoofdloop van de applicatie: toont menu en voert commando's uit
    public void Run()
    {
        while (true)
        {
            // Toon het menu aan de gebruiker
            _console.WriteLine("\n== VCard Manager ==");
            _console.WriteLine("1. Toon alle contacten");
            _console.WriteLine("2. Voeg nieuw contact toe");
            _console.WriteLine("3. Zoek contact op naam");
            _console.WriteLine("4. Verwijder contact");
            _console.WriteLine("5. Exporteer contact");
            _console.WriteLine("6. Stoppen");
            _console.Write("> ");

            var input = _console.ReadLine();

            // Als gebruiker 'exit' typt, stoppen we de applicatie
            if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
                return;

            // Verwerk de keuze van de gebruiker
            switch (input)
            {
                case "1":
                    ToonAlleContacten();
                    break;
                case "2":
                    VoegNieuwContactToe();
                    break;
                case "3":
                    ZoekContact();
                    break;
                case "4":
                    VerwijderContact();
                    break;
                case "5":
                    ExporteerContact();
                    break;
                case "6":
                    _console.WriteLine("Tot ziens!");
                    return;
                default:
                    _console.WriteLine("Ongeldige keuze.");
                    break;
            }
        }
    }

    // Haalt alle contacten op via de service en toont ze
    private void ToonAlleContacten()
    {
        var contacten = _service.GetAllContacts();
        if (contacten.Count == 0)
        {
            _console.WriteLine("Geen contacten gevonden.");
            return;
        }

        // Toon elk contact in één regel
        foreach (var c in contacten)
        {
            _console.WriteLine($"{c.FullName}: {c.Email}, {c.Phone}");
        }
    }

    // Vraagt gegevens voor een nieuw contact op en voegt het toe
    private void VoegNieuwContactToe()
    {
        _console.Write("Voornaam: ");
        var first = _console.ReadLine();

        _console.Write("Achternaam: ");
        var last = _console.ReadLine();

        string phone;
        while (true)
        {
            _console.Write("Telefoonnummer: ");
            phone = _console.ReadLine();

            var geldig = Regex.IsMatch(phone, @"^0?4\d{2}([./]?\d{2}){3}$");
            if (geldig)
                break;

            _console.WriteLine("Ongeldig telefoonnummer. Probeer opnieuw.");

        }

        _console.Write("E-mailadres: ");
        var email = _console.ReadLine();

        // Maak het contact aan met ingevulde gegevens
        var contact = new VCardContact
        {
            FirstName = first,
            LastName = last,
            Phone = phone,
            Email = email
        };

        _service.AddContact(contact);
        _console.WriteLine("Contact toegevoegd.");
    }

    // Vraagt een naam op en zoekt het bijbehorende contact
    private void ZoekContact()
    {
        _console.Write("Naam (volledig): ");
        var naam = _console.ReadLine();

        var result = _service.SearchContact(naam);
        if (result == null)
        {
            _console.WriteLine("Contact niet gevonden.");
        }
        else
        {
            _console.WriteLine($"Gevonden: {result.FullName} | Tel: {result.Phone} | Email: {result.Email}");
        }
    }

    // Vraagt een naam op en probeert dat contact te verwijderen
    private void VerwijderContact()
    {
        _console.Write("Naam (volledig): ");
        var naam = _console.ReadLine();

        var success = _service.DeleteContact(naam);
        _console.WriteLine(success ? "Contact verwijderd." : "Contact niet gevonden.");
    }

    // Vraagt een naam en een pad op en probeert het contact te exporteren naar .vcf
    private void ExporteerContact()
    {
        _console.Write("Naam (volledig): ");
        var naam = _console.ReadLine();

        _console.Write("Bestandsnaam (bijv. jan.vcf): ");
        var pad = _console.ReadLine();

        var success = _service.ExportContact(naam, pad);
        _console.WriteLine(success ? "Contact geëxporteerd." : "Contact niet gevonden.");
    }
}