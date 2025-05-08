using Xunit;
using Moq;
using BL;
using IDAL;
using Model;
using System.Collections.Generic;

public class MockMoqTest
{
    [Fact]
    public void PoliczAdresy_Moq_VerifyWywolanie()
    {
        var mock = new Mock<IAdresyRepository>();
        mock.Setup(r => r.GetAdresy()).Returns(new List<Adresy>());
        var obsluga = new ObslugaAdresow(mock.Object);
        obsluga.PoliczAdresy();
        mock.Verify(r => r.GetAdresy(), Times.Once);
    }
}
