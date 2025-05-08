using IDAL;
using Model;
using System.Collections.Generic;

namespace BL.Tests.Doubles
{
    public class SpyAdresyRepository : IAdresyRepository
    {
        public bool InsertAdresCalled { get; private set; } = false;

        public IEnumerable<Adresy> GetAdresy() => new List<Adresy>();
        public Adresy GetAdresyByID(int adresId) => null;
        public void InsertAdres(Adresy adres) { InsertAdresCalled = true; }
        public void DeleteAdres(int adresId) { }
        public void UpdateAdres(Adresy adres) { }
        public void Save() { }
        public void Dispose() { }
    }
}
