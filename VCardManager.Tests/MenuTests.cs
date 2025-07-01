using VCardManager.Core;
using VCardManager.Core.Interfaces;

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

        // Controleert of het menu werd getoond
        Assert.Contains(spy.Output, line => line.Contains("VCard Manager"));

        // Controleren NIET op "exit" in de output, want dat wordt niet geprint
    }

    private class DummyService : IVCardService
    {
        public void AddContact(Core.Models.VCardContact contact) { }
        public bool DeleteContact(string fullName) => false;
        public bool ExportContact(string fullName, string filePath) => false;
        public List<Core.Models.VCardContact> GetAllContacts() => new();
        public Core.Models.VCardContact? SearchContact(string name) => null;
    }
}