using System.Threading.Tasks;
using tasinmaz.API.Models;

namespace tasinmaz.API.Data
{
    public interface IAuthRepository
    {
        Task<Kullanici> Register(Kullanici kullanici, string password);
        Task<Kullanici> Login(string email, string password);
        Task<bool> UserExists(string email);
    }
}