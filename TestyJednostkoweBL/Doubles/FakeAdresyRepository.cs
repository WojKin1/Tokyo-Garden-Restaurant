using IDAL;
using Model;
using System.Collections.Generic;
using System.Linq;

namespace BL.Tests.Doubles
{
    public class FakeAdresyRepository : IAdresyRepository
    {
        private readonly List<Adresy> _adresy = new List<Adresy>();

        public IEnumerable<Adresy> GetAdresy() => _adresy;
        public Adresy GetAdresyByID(int adresId) => _adresy.FirstOrDefault(a => a.id == adresId);
        public void InsertAdres(Adresy adres) => _adresy.Add(adres);
        public void DeleteAdres(int adresId) => _adresy.RemoveAll(a => a.id == adresId);
        public void UpdateAdres(Adresy adres) { }
        public void Save() { }
        public void Dispose() { }
    }
}
