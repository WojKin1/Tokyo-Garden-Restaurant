using System.Collections.Generic;
using System.Threading.Tasks;
using TokyoGarden.Model;

namespace TokyoGarden.IBL
{
    public interface IUzytkownikService
    {
        Task<IEnumerable<Uzytkownicy>> GetAllAsync();
        Task<Uzytkownicy?> GetByIdAsync(int id);
        Task AddAsync(Uzytkownicy user);
        Task UpdateAsync(Uzytkownicy user);
        Task DeleteAsync(int id);

        Task<Uzytkownicy?> AuthenticateAsync(string username, string password);
        Task<bool> UsernameExistsAsync(string username);
        Task<Uzytkownicy?> GetByUsernameAsync(string username);
    }
}
