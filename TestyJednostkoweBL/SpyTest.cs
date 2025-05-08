using Xunit;
using BL;
using BL.Tests.Doubles;
using Model;

public class SpyTest
{
    [Fact]
    public void InsertAdres_Ustawia_Flage()
    {
        var spy = new SpyAdresyRepository();
        spy.InsertAdres(new Adresy());
        Assert.True(spy.InsertAdresCalled);
    }
}
