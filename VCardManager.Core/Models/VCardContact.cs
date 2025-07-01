namespace VCardManager.Core.Models;

// Deze klasse stelt één contactpersoon voor in het systeem.
// Ze bevat de gegevens die nodig zijn om een vCard aan te maken of weer te geven.
public class VCardContact
{
    // Voornaam van de persoon
    public string FirstName { get; set; } = string.Empty;

    // Achternaam van de persoon
    public string LastName { get; set; } = string.Empty;

    // E-mailadres van de persoon
    public string Email { get; set; } = string.Empty;

    // Telefoonnummer van de persoon
    public string Phone { get; set; } = string.Empty;

    // Afgeleide eigenschap die de volledige naam teruggeeft
    public string FullName => $"{FirstName} {LastName}";
}