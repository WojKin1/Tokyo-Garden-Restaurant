using Xunit;
using Moq;
using BL;
using IBL;
using IDAL;
using Model;
using System.Collections.Generic;

namespace TestProject.Moq
{
	public class MoqMockTest
	{
		[Fact]
		public void MockTest_ObslugaAdresow_PoliczAdresy()
		{

			var adresy = new List<Adresy>
			{
				new Adresy { id = 1, miasto = "Warszawa", ulica = "Marsza³kowska", nr_domu = 1, nr_mieszkania = 10 },
				new Adresy { id = 2, miasto = "Kraków", ulica = "Floriañska", nr_domu = 2, nr_mieszkania = 20 }
			};

			var mockRepo = new Mock<IAdresyRepository>(MockBehavior.Strict);
			mockRepo.Setup(repo => repo.GetAdresy()).Returns(adresy);

			var obslugaAdresow = new ObslugaAdresow(mockRepo.Object);

			var result = obslugaAdresow.PoliczAdresy();

			Assert.Equal(2, result);

			mockRepo.Verify(repo => repo.GetAdresy(), Times.Once);

			mockRepo.VerifyNoOtherCalls();
		}
	}
}