using IDAL;
using Model;
using System.Collections.Generic;

namespace BL.Tests.Doubles
{
    public class DummyAdresyRepository : IAdresyRepository
    {
        public IEnumerable<Adresy> GetAdresy() => null;
        public Adresy GetAdresyByID(int adresId) => null;
        public void InsertAdres(Adresy adres) { }
        public void DeleteAdres(int adresId) { }
        public void UpdateAdres(Adresy adres) { }
        public void Save() { }
        public void Dispose() { }
    }
}
