using Xunit;
using Moq;
using BL;
using IDAL;
using Model;
using System.Collections.Generic;

public class SpyMoqTest
{
    [Fact]
    public void PobierzPosortowaneAdresy_Moq_VerifyWywolanie()
    {
        var spy = new Mock<IAdresyRepository>();
        spy.Setup(r => r.GetAdresy()).Returns(new List<Adresy>());
        var obsluga = new ObslugaAdresow(spy.Object);
        obsluga.PobierzPosortowaneAdresy();
        spy.Verify(r => r.GetAdresy(), Times.Once);
    }
}
