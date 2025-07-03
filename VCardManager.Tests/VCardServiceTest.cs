using Moq;
using VCardManager.Core;
using VCardManager.Core.Models;

namespace VCardManager.Tests;


public class VCardServiceTest
{
    private readonly Mock<IVCardRepository> _repositoryMock;
    private readonly VCardService _service;

    public VCardServiceTest()
    {
        _repositoryMock = new Mock<IVCardRepository>();
        _service = new VCardService(_repositoryMock.Object);
    }

    [Fact]
    public void AddContact_ShouldCallRepositoryAdd()
    {
        var contact = new VCardContact { FirstName = "Milad", LastName = "Eshaghzey" };

        _service.AddContact(contact);

        _repositoryMock.Verify(r => r.Add(contact), Times.Once);
    }

    [Fact]
    public void DeleteContact_ShouldCallRepositoryDelete_WhenContactExists()
    {
        var fullName = "Milad Eshaghzey";
        _repositoryMock.Setup(r => r.FindByName(fullName)).Returns(new VCardContact());

        var result = _service.DeleteContact(fullName);

        _repositoryMock.Verify(r => r.Delete(fullName), Times.Once);
        Assert.True(result);
    }

    [Fact]
    public void DeleteContact_ShouldReturnFalse_WhenContactDoesNotExist()
    {
        var fullName = "Milad Eshaghzey";
        _repositoryMock.Setup(r => r.FindByName(fullName)).Returns((VCardContact?)null);

        var result = _service.DeleteContact(fullName);

        _repositoryMock.Verify(r => r.Delete(fullName), Times.Never);
        Assert.False(result);
    }
}