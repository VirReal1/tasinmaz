using System.Collections.Generic;
using System.Threading.Tasks;
using tasinmaz.API.Models;

namespace tasinmaz.API.Data
{
    public interface IUserRepository
    {
        Task<Kullanici> Register(Kullanici kullanici, string password);
        Task<Kullanici> Login(string email, string password);
        Task<bool> UserExists(string email);
        Task<Kullanici> Remove(int kullaniciId);
        Task<Kullanici> Edit(Kullanici kullanici);
        Task<List<Kullanici>> GetUsers();
        Task<Kullanici> Get(int kullaniciId);
    }
}