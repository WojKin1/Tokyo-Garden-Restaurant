using Xunit;
using BL;
using BL.Tests.Doubles;

public class MockTest
{
    [Fact]
    public void PoliczAdresy_Wywoluje_GetAdresy_Raz()
    {
        var mock = new MockAdresyRepository();
        var obsluga = new ObslugaAdresow(mock);
        obsluga.PoliczAdresy();
        Assert.Equal(1, mock.GetAdresyCallCount);
    }
}
