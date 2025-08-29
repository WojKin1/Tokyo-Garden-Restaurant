using System.Collections.Generic;
using System.Threading.Tasks;
using TokyoGarden.IBL;
using TokyoGarden.IDAL;
using TokyoGarden.Model;

namespace TokyoGarden.BL
{
    public class UzytkownikService : IUzytkownikService
    {
        private readonly IUzytkownikRepository _repo;
        public UzytkownikService(IUzytkownikRepository repo) { _repo = repo; }

        public Task<IEnumerable<Uzytkownicy>> GetAllAsync() => _repo.GetAllAsync();
        public Task<Uzytkownicy?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
        public Task AddAsync(Uzytkownicy user) => _repo.AddAsync(user);
        public Task UpdateAsync(Uzytkownicy user) => _repo.UpdateAsync(user);
        public Task DeleteAsync(int id) => _repo.DeleteAsync(id);

        public Task<Uzytkownicy?> AuthenticateAsync(string username, string password) => _repo.AuthenticateAsync(username, password);
        public Task<bool> UsernameExistsAsync(string username) => _repo.UsernameExistsAsync(username);
        public Task<Uzytkownicy?> GetByUsernameAsync(string username) => _repo.GetByUsernameAsync(username);
    }
}
