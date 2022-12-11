using System.Collections.Generic;
using System.Threading.Tasks;
using tasinmaz.API.Dtos;
using tasinmaz.API.ServiceResponder;

namespace tasinmaz.API.Services.Abstract
{
    public interface IUserService
    {
        Task<ServiceResponse<List<KullaniciForShowDeleteDto>>> GetAllAsync();
        Task<ServiceResponse<List<KullaniciForShowDeleteDto>>> GetTasinmazlarAsync(KullaniciForShowDeleteDto kullaniciForInfoDto);
        Task<ServiceResponse<KullaniciForShowDeleteDto>> AddTasinmazAsync(KullaniciForAddUpdateDto kullaniciForAddUpdateDto);
        Task<ServiceResponse<KullaniciForShowDeleteDto>> UpdateTasinmazAsync(KullaniciForAddUpdateDto kullaniciForAddUpdateDto);
        Task<ServiceResponse<KullaniciForShowDeleteDto>> DeleteTasinmazAsync(KullaniciForAddUpdateDto kullaniciForAddUpdateDto);
    }
}