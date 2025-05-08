using Xunit;
using Moq;
using BL;
using IDAL;

public class DummyMoqTest
{
    [Fact]
    public void Konstruktor_Akceptuje_DummyMoq()
    {
        var dummy = new Mock<IAdresyRepository>().Object;
        var obsluga = new ObslugaAdresow(dummy);
        Assert.NotNull(obsluga);
    }
}
