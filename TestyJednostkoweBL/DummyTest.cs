using Xunit;
using BL;
using BL.Tests.Doubles;

public class DummyTest
{
    [Fact]
    public void Konstruktor_Akceptuje_Dummy()
    {
        var dummy = new DummyAdresyRepository();
        var obsluga = new ObslugaAdresow(dummy);
        Assert.NotNull(obsluga);
    }
}
