using Moq;
using VCardManager.Core;
using VCardManager.Core.Interfaces;
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