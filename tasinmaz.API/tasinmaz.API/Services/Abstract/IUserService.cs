using System.Collections.Generic;
using System.Threading.Tasks;
using tasinmaz.API.Dtos;
using tasinmaz.API.Models.Concrete;
using tasinmaz.API.ServiceResponder;

namespace tasinmaz.API.Services.Abstract
{
    public interface IUserService
    {
        Task<ServiceResponse<List<KullaniciForShowDeleteDto>>> GetAllAsync();
        Task<ServiceResponse<List<KullaniciForShowDeleteDto>>> GetUsersAsync(KullaniciForShowDeleteDto kullaniciForShowDeleteDto);
        Task<ServiceResponse<KullaniciToken>> LoginUserAsync(KullaniciForLoginDto kullaniciForLoginDto);
        Task<ServiceResponse<KullaniciForShowDeleteDto>> AddUserAsync(KullaniciForAddUpdateDto kullaniciForAddUpdateDto);
        Task<ServiceResponse<KullaniciForShowDeleteDto>> UpdateUserAsync(KullaniciForAddUpdateDto kullaniciForAddUpdateDto);
        Task<ServiceResponse<KullaniciForShowDeleteDto>> DeleteUserAsync(KullaniciForShowDeleteDto kullaniciForShowDeleteDto);
    }
}