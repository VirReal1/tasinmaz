using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using tasinmaz.API.Models;

namespace tasinmaz.API.Data
{
    public class UserRepository : IUserRepository
    {
        private DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public List<Kullanici> GetUsers()
        {
            var users = _context.Kullanicilar.ToList();
            return users;
        }
        public async Task<Kullanici> Login(string email, string password)
        {
            var kullanici = await _context.Kullanicilar.FirstOrDefaultAsync(x => x.Email == email);
            if (kullanici == null)
            {
                return null;
            }

            if (!VerifyPasswordHash(password, kullanici.PasswordHash, kullanici.PasswordSalt))
            {
                return null;
            }

            return kullanici;
        }

        private bool VerifyPasswordHash(string password, byte[] kullaniciPasswordHash, byte[] kullaniciPasswordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(kullaniciPasswordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != kullaniciPasswordHash[i])
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public async Task<Kullanici> Register(Kullanici kullanici, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            kullanici.PasswordHash = passwordHash;
            kullanici.PasswordSalt = passwordSalt;
            await _context.Kullanicilar.AddAsync(kullanici);
            await _context.SaveChangesAsync();
            return kullanici;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> UserExists(string email)
        {
            if (await _context.Kullanicilar.AnyAsync(x => x.Email == email))
            {
                return true;
            }

            return false;
        }

        public Task<Kullanici> Remove(int kullaniciId)
        {
            throw new System.NotImplementedException();
        }

        public Task<Kullanici> Edit(Kullanici kullanici)
        {
            throw new System.NotImplementedException();
        }
    }
}