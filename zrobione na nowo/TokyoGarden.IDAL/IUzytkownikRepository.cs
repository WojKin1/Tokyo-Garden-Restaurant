using System.Threading.Tasks;
using TokyoGarden.Model;

namespace TokyoGarden.IDAL
{
    public interface IUzytkownikRepository : IRepository<Uzytkownicy>
    {
        Task<Uzytkownicy?> GetByUsernameAsync(string username);
        Task<Uzytkownicy?> AuthenticateAsync(string username, string password);
        Task<bool> UsernameExistsAsync(string username);
    }
}
