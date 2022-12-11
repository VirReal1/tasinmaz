using System.Collections.Generic;
using System.Threading.Tasks;
using tasinmaz.API.Dtos;
using tasinmaz.API.ServiceResponder;
using tasinmaz.API.Services.Abstract;

namespace tasinmaz.API.Services.Concrete
{
    public class UserService : IUserService
    {
        public async Task<ServiceResponse<List<KullaniciForShowDeleteDto>>> GetAllAsync()
        {
            throw new System.NotImplementedException();
        }

        public async Task<ServiceResponse<List<KullaniciForShowDeleteDto>>> GetTasinmazlarAsync(KullaniciForShowDeleteDto kullaniciForInfoDto)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ServiceResponse<KullaniciForShowDeleteDto>> AddTasinmazAsync(KullaniciForAddUpdateDto kullaniciForAddUpdateDto)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ServiceResponse<KullaniciForShowDeleteDto>> UpdateTasinmazAsync(KullaniciForAddUpdateDto kullaniciForAddUpdateDto)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ServiceResponse<KullaniciForShowDeleteDto>> DeleteTasinmazAsync(KullaniciForAddUpdateDto kullaniciForAddUpdateDto)
        {
            throw new System.NotImplementedException();
        }
    }
}