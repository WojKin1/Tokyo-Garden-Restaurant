using Xunit;
using BL;
using Model;
using BL.Tests.Doubles;

public class FakeTest
{
    [Fact]
    public void PoliczAdresy_Zwraca_PoprawnaLiczbe()
    {
        var fake = new FakeAdresyRepository();
        fake.InsertAdres(new Adresy());
        fake.InsertAdres(new Adresy());
        var obsluga = new ObslugaAdresow(fake);
        var result = obsluga.PoliczAdresy();
        Assert.Equal(2, result);
    }
}
