using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using tasinmaz.API.Dtos;
using tasinmaz.API.Models;
using tasinmaz.API.Models.Concrete;

namespace tasinmaz.API.Data
{
    public interface IUserRepository
    {
        Task<Kullanici> Get(Expression<Func<Kullanici, bool>> filter);
        Task<ICollection<Kullanici>> GetAllAsync(Expression<Func<Kullanici, bool>> filter = null);
        Task<bool> Exists(Expression<Func<Kullanici, bool>> filter);
        Task<KullaniciToken> LoginUserAsync(KullaniciForLoginDto kullaniciForLoginDto);
        Task<bool> AddAsync(KullaniciForUpdateDto kullaniciForAddUpdateDto);
        Task<bool> UpdateAsync(KullaniciForUpdateDto kullaniciForAddUpdateDto);
        Task<bool> DeleteAsync(Kullanici kullanici);
    }
}