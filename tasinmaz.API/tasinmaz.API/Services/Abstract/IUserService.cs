using System.Collections.Generic;
using System.Threading.Tasks;
using tasinmaz.API.Dtos;
using tasinmaz.API.ServiceResponder;

namespace tasinmaz.API.Services.Abstract
{
    public interface IUserService
    {
        Task<ServiceResponse<List<KullaniciForShowDto>>> GetAllAsync(int logKullaniciId);
        Task<ServiceResponse<List<KullaniciForShowDto>>> GetUsersAsync(KullaniciForShowDto kullaniciForShowDto);
        Task<ServiceResponse<string>> LoginUserAsync(KullaniciForLoginDto kullaniciForLoginDto);
        Task<ServiceResponse<KullaniciForShowDto>> AddUserAsync(KullaniciForAddDto kullaniciForAddDto);
        Task<ServiceResponse<KullaniciForShowDto>> UpdateUserAsync(KullaniciForUpdateDto kullaniciForUpdateDto);
        Task<ServiceResponse<KullaniciForShowDto>> DeleteUserAsync(int logKullaniciId, int kullaniciId);
    }
}