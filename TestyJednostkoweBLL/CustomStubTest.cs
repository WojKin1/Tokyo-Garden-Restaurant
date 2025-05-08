using Xunit;
using BL;
using IBL;
using IDAL;
using Model;
using System.Collections.Generic;
using System.Linq;

namespace TestProject.Custom
{
	public class CustomStubTest
	{
		private class StubAlergenyRepository : IAlergenyRepository
		{
			private readonly List<Alergeny> _alergeny;

			public StubAlergenyRepository(List<Alergeny> alergeny)
			{
				_alergeny = alergeny;
			}

			public IEnumerable<Alergeny> GetAlergeny() => _alergeny;

			public void DeleteAlergen(int alergenId) => throw new System.NotImplementedException();
			public void Dispose() => throw new System.NotImplementedException();
			public Alergeny GetAlergenyByID(int alergenId) => throw new System.NotImplementedException();
			public void InsertAlergen(Alergeny alergen) => throw new System.NotImplementedException();
			public void Save() => throw new System.NotImplementedException();
			public void UpdateAlergen(Alergeny alergen) => throw new System.NotImplementedException();
		}

		[Fact]
		public void StubTest_ObslugaAlergenow_PobierzPosortowaneAlergeny()
		{
			var alergenyList = new List<Alergeny>
			{
				new Alergeny { id = 1, nazwa_alergenu = "Orzechy", opis_alergenu = "Alergeny z orzechów" },
				new Alergeny { id = 2, nazwa_alergenu = "Gluten", opis_alergenu = "Alergeny glutenowe" },
				new Alergeny { id = 3, nazwa_alergenu = "Bia³ko mleka", opis_alergenu = "Alergeny mleczne" }
			};

			var stubAlergenyRepo = new StubAlergenyRepository(alergenyList);

			var obslugaAlergenow = new ObslugaAlergenow(stubAlergenyRepo);

			var result = obslugaAlergenow.PobierzPosortowaneAlergeny().ToList();

			Assert.Equal(3, result.Count);
			Assert.Equal("Bia³ko mleka", result[0].nazwa_alergenu);
			Assert.Equal("Gluten", result[1].nazwa_alergenu);
			Assert.Equal("Orzechy", result[2].nazwa_alergenu);
		}
	}
}