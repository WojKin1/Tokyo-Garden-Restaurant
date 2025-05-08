using Xunit;
using BL;
using IBL;
using IDAL;
using Model;
using System.Collections.Generic;
using System.Linq;

namespace TestProject.Custom
{
    public class CustomMockTest
    {
        private class MockUzytkownikRepository : IUzytkownikRepository
        {
            private readonly List<Uzytkownik> _uzytkownicy;
            public bool GetUzytkownikWasCalled { get; private set; } = false;
            public int GetUzytkownikCallCount { get; private set; } = 0;

            public MockUzytkownikRepository(List<Uzytkownik> uzytkownicy)
            {
                _uzytkownicy = uzytkownicy;
            }

            public IEnumerable<Uzytkownik> GetUzytkownik()
            {
                GetUzytkownikWasCalled = true;
                GetUzytkownikCallCount++;
                return _uzytkownicy;
            }

            public void DeleteUzytkownik(int uzytkownikID) => throw new System.NotImplementedException();
            public void Dispose() => throw new System.NotImplementedException();
            public Uzytkownik GetUzytkownikByID(int uzytkownikID) => throw new System.NotImplementedException();
            public void InsertUzytkownik(Uzytkownik uzytkownik) => throw new System.NotImplementedException();
            public void Save() => throw new System.NotImplementedException();
            public void UpdateUzytkownik(Uzytkownik uzytkownik) => throw new System.NotImplementedException();
        }

        [Fact]
        public void MockTest_ObslugaUzytkownikow_PobierzPosortowaneUzytkownikow()
        {
            var uzytkownicy = new List<Uzytkownik>
            {
                new Uzytkownik { id = 1, nazwa_uzytkownika = "jankowalski", nazwisko = "Kowalski", haslo = "pass123" },
                new Uzytkownik { id = 2, nazwa_uzytkownika = "anowak", nazwisko = "Nowak", haslo = "pass456" },
                new Uzytkownik { id = 3, nazwa_uzytkownika = "mwisniewski", nazwisko = "Wiœniewski", haslo = "pass789" }
            };

            var mockUzytkownikRepo = new MockUzytkownikRepository(uzytkownicy);

            var obslugaUzytkownikow = new ObslugaUzytkownikow(mockUzytkownikRepo);

            var result = obslugaUzytkownikow.PobierzPosortowaneUzytkownikow().ToList();

            Assert.True(mockUzytkownikRepo.GetUzytkownikWasCalled);
            Assert.Equal(1, mockUzytkownikRepo.GetUzytkownikCallCount);

            Assert.Equal(3, result.Count);
            Assert.Equal("Kowalski", result[0].nazwisko);
            Assert.Equal("Nowak", result[1].nazwisko);
            Assert.Equal("Wiœniewski", result[2].nazwisko);
        }
    }
}