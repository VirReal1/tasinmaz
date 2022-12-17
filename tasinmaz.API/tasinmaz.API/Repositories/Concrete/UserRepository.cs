using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using tasinmaz.API.Dtos;
using tasinmaz.API.Models;
using tasinmaz.API.Models.Concrete;

namespace tasinmaz.API.Data
{
    public class UserRepository : IUserRepository
    {
        DataContext _context;
        IMapper _mapper;
        IConfiguration _configuration;
        IHttpContextAccessor _httpContextAccessor;

        public UserRepository(DataContext context, IMapper mapper, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Kullanici> Get(Expression<Func<Kullanici, bool>> filter)
        {
            return await _context.Kullanicilar.FirstOrDefaultAsync(filter);
        }

        public async Task<ICollection<Kullanici>> GetAllAsync(Expression<Func<Kullanici, bool>> filter = null)
        {
            return filter == null ? await _context.Kullanicilar.ToListAsync() : await _context.Kullanicilar.Where(filter).ToListAsync();
        }
        public async Task<bool> Exists(Expression<Func<Kullanici, bool>> filter)
        {
            return await _context.Kullanicilar.AnyAsync(filter);
        }

        public async Task<KullaniciToken> LoginUserAsync(KullaniciForLoginDto kullaniciForLoginDto)
        {
            var kullanici = await _context.Kullanicilar.FirstOrDefaultAsync(x => x.Email == kullaniciForLoginDto.Email);
            
            if (kullanici == null)
            {
                return null;
            }

            if (!VerifyPasswordHash(kullaniciForLoginDto.Password, kullanici.PasswordHash, kullanici.PasswordSalt))
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("AppSettings:Token").Value);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, kullanici.Id.ToString()),
                    new Claim(ClaimTypes.Email, kullanici.Email),
                    new Claim("role", kullanici.UserRole)
                }),
                Expires = DateTime.Now.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature),
                Issuer = _configuration["Jwt: Issuer"],
                Audience = _configuration["Jwt: Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        public async Task<bool> AddAsync(KullaniciForUpdateDto kullaniciForAddUpdateDto)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(kullaniciForAddUpdateDto.Password, out passwordHash, out passwordSalt);
            kullaniciForAddUpdateDto.PasswordHash = passwordHash;
            kullaniciForAddUpdateDto.PasswordSalt = passwordSalt;

            await _context.Kullanicilar.AddAsync(_mapper.Map<Kullanici>(kullaniciForAddUpdateDto));
            return await SaveChanges();
        }
        public async Task<bool> UpdateAsync(KullaniciForUpdateDto kullaniciForAddUpdateDto)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(kullaniciForAddUpdateDto.Password, out passwordHash, out passwordSalt);
            kullaniciForAddUpdateDto.PasswordHash = passwordHash;
            kullaniciForAddUpdateDto.PasswordSalt = passwordSalt;

            _context.ChangeTracker.Clear();
            _context.Kullanicilar.Update(_mapper.Map<Kullanici>(kullaniciForAddUpdateDto));
            return await SaveChanges();
        }

        public async Task<bool> DeleteAsync(Kullanici kullanici)
        {
            _context.Kullanicilar.Remove(kullanici);
            return await SaveChanges();
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

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}