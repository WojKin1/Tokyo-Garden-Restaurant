using Model;
using System.Collections.Generic;
using System;

namespace IDAL
{
    public interface IKategoriaRepository : IDisposable
    {
        IEnumerable<Kategoria> GetKategorie();
        Kategoria GetKategoriaByID(int id);
        void InsertKategoria(Kategoria kategoria);
        void DeleteKategoria(int id);
        void UpdateKategoria(Kategoria kategoria);
        void Save();
    }
}
