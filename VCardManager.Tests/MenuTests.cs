using Moq;
using VCardManager.Core;
using VCardManager.Core.Models;

namespace VCardManager.Tests;

public class MenuTests
{
    [Fact]
    public void Run_ExitCommand_ShouldStopImmediately()
    {
        // Arrange
        var spy = new ConsoleSpy();
        spy.AddInput("exit");

        var dummyService = new DummyService();
        var menu = new Menu(dummyService, spy);

        // Act
        menu.Run();

        // Assert: controleert of het menu werd getoond
        Assert.Contains(spy.Output, line => line.Contains("VCard Manager"));

        // We controleren niet op "exit" in output, want dat wordt niet geprint
    }

    [Fact]
    public void Run_NieuwContactToevoegen_RoeptAddContactAan()
    {
        // Arrange
        var mockService = new Mock<IVCardService>();
        var spy = new ConsoleSpy();

        // Simuleer invoer voor nieuw contact
        spy.AddInput("2");                  // optie: nieuw contact
        spy.AddInput("Jan");                // voornaam
        spy.AddInput("Jansen");             // achternaam
        spy.AddInput("0470000000");         // telefoon
        spy.AddInput("jan@example.com");    // e-mail
        spy.AddInput("6");                  // stoppen

        var menu = new Menu(mockService.Object, spy);

        // Act
        menu.Run();

        // Assert: AddContact moet zijn aangeroepen
        mockService.Verify(s => s.AddContact(It.IsAny<VCardContact>()), Times.Once);
    }

    [Fact]
    public void Run_ZoekContact_RoeptSearchContactAan()
    {
        // Arrange
        var mockService = new Mock<IVCardService>();
        var spy = new ConsoleSpy();

        // Simuleer invoer voor zoeken
        spy.AddInput("3");                  // optie: zoek contact
        spy.AddInput("Jan Jansen");         // naam om te zoeken
        spy.AddInput("6");                  // stoppen

        var menu = new Menu(mockService.Object, spy);

        // Act
        menu.Run();

        // Assert: SearchContact moet zijn aangeroepen met de juiste naam
        mockService.Verify(s => s.SearchContact("Jan Jansen"), Times.Once);
    }
    
    [Fact]
    public void Run_VerwijderContact_RoeptDeleteContactAan()
    {
        // Arrange
        var mockService = new Mock<IVCardService>();
        var spy = new ConsoleSpy();
        // Simuleer invoer voor verwijderen
        spy.AddInput("4");                  // optie: verwijder contact
        spy.AddInput("Jan Jansen");         // naam om te verwijderen
        spy.AddInput("6");                  // stoppen      
        var menu = new Menu(mockService.Object, spy);
        // Act
        menu.Run();
        // Assert: DeleteContact moet zijn aangeroepen met de juiste naam
        mockService.Verify(s => s.DeleteContact("Jan Jansen"), Times.Once);
    }

    [Fact]
    public void Run_ExporteerContact_RoeptExportContactAan()
    {
        // Arrange
        var mockService = new Mock<IVCardService>();
        var spy = new ConsoleSpy();
        // Simuleer invoer voor exporteren
        spy.AddInput("5");                  // optie: exporteer contact
        spy.AddInput("Jan Jansen");         // naam om te exporteren
        spy.AddInput("data/jan_jansen.vcf"); // export pad
        spy.AddInput("6");                  // stoppen
        var menu = new Menu(mockService.Object, spy);
        // Act
        menu.Run();
        // Assert: ExportContact moet zijn aangeroepen met de juiste naam en pad
        mockService.Verify(s => s.ExportContact("Jan Jansen", "data/jan_jansen.vcf"), Times.Once);
    }



    // Dummy voor de eerste test
    private class DummyService : IVCardService
    {
        public void AddContact(VCardContact contact) { }
        public bool DeleteContact(string fullName) => false;
        public bool ExportContact(string fullName, string filePath) => false;
        public List<VCardContact> GetAllContacts() => new();
        public VCardContact? SearchContact(string name) => null;
    }
}