using Moq;
using Xunit;
using Model;
using IBL;
using IDAL;
using System.Collections.Generic;

public class ObslugaAdresowTests
{
    [Fact]
    public void TestDummy()
    {
        var dummyRepository = new Mock<IAdresyRepository>();

        var obslugaAdresow = new ObslugaAdresow(dummyRepository.Object);

        Assert.NotNull(obslugaAdresow);
    }

    [Fact]
    public void TestStub()
    {
        var stubRepository = new Mock<IAdresyRepository>();
        stubRepository.Setup(repo => repo.GetAdresy()).Returns(new List<Adresy>
    {
        new Adresy { miasto = "Warszawa" },
        new Adresy { miasto = "Kraków" }
    });

        var obslugaAdresow = new ObslugaAdresow(stubRepository.Object);

        var adresy = obslugaAdresow.PobierzPosortowaneAdresy();
        Assert.Equal(2, adresy.Count());
        Assert.Equal("Kraków", adresy.First().miasto);
    }

    [Fact]
    public void TestMock()
    {
        var mockRepository = new Mock<IAdresyRepository>();
        mockRepository.Setup(repo => repo.GetAdresy()).Returns(new List<Adresy>());

        var obslugaAdresow = new ObslugaAdresow(mockRepository.Object);

        obslugaAdresow.PobierzPosortowaneAdresy();

        mockRepository.Verify(repo => repo.GetAdresy(), Times.Once);
    }

    [Fact]
    public void TestSpy()
    {
        var spyRepository = new Mock<IAdresyRepository>();
        List<Adresy> retrievedAdresy = null;
        spyRepository.Setup(repo => repo.GetAdresy()).Callback(() => retrievedAdresy = new List<Adresy>
    {
        new Adresy { miasto = "Gdañsk" }
    }).Returns(retrievedAdresy);

        var obslugaAdresow = new ObslugaAdresow(spyRepository.Object);

        var adresy = obslugaAdresow.PobierzPosortowaneAdresy();

        Assert.Single(adresy);
        Assert.Equal("Gdañsk", adresy.First().miasto);
    }

}
