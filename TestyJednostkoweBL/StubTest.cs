using Xunit;
using BL;
using BL.Tests.Doubles;
using System.Linq;

public class StubTest
{
    [Fact]
    public void PobierzPosortowaneAdresy_Zwraca_Posortowane()
    {
        var stub = new StubAdresyRepository();
        var obsluga = new ObslugaAdresow(stub);

        var result = obsluga.PobierzPosortowaneAdresy().ToList();

        Assert.Equal("Kraków", result[0].miasto);
        Assert.Equal("Warszawa", result[1].miasto);
    }
}
