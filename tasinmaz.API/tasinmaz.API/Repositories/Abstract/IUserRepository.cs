using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using tasinmaz.API.Dtos;
using tasinmaz.API.Models;

namespace tasinmaz.API.Data
{
    public interface IUserRepository
    {
        Task<Kullanici> Get(Expression<Func<Kullanici, bool>> filter);
        Task<ICollection<Kullanici>> GetAllAsync(Expression<Func<Kullanici, bool>> filter = null);
        Task<bool> Exists(Expression<Func<Kullanici, bool>> filter);
        Task<string[]> LoginUserAsync(KullaniciForLoginDto kullaniciForLoginDto);
        Task<bool> AddAsync(KullaniciForAddDto kullaniciForAddDto);
        Task<bool> UpdateAsync(KullaniciForUpdateDto kullaniciForUpdateDto);
        Task<bool> DeleteAsync(Expression<Func<Kullanici, bool>> filter);
    }
}