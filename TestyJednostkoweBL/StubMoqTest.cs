using Xunit;
using Moq;
using BL;
using Model;
using System.Collections.Generic;
using System.Linq;

public class StubMoqTest
{
    [Fact]
    public void PobierzPosortowaneAdresy_StubMoq()
    {
        var stub = new Mock<IDAL.IAdresyRepository>();
        stub.Setup(r => r.GetAdresy()).Returns(new List<Adresy>
        {
            new Adresy { miasto = "Poznañ" }
        });

        var obsluga = new ObslugaAdresow(stub.Object);

        var result = obsluga.PobierzPosortowaneAdresy().First();
        Assert.Equal("Poznañ", result.miasto);
    }
}
