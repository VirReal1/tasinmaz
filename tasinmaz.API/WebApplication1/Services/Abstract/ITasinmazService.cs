using System.Collections.Generic;
using System.Threading.Tasks;
using tasinmaz.API.Dtos.Tasinmaz;
using tasinmaz.API.Entities.Concrete;
using tasinmaz.API.ServiceResponder;

namespace tasinmaz.API.Services.Abstract
{
    public interface ITasinmazService
    {
        Task<ServiceResponse<List<TasinmazDto>>> GetAllAsync();
        Task<ServiceResponse<List<TasinmazDto>>> GetTasinmazlarAsync(TasinmazDto tasinmazDto);
        Task<ServiceResponse<TasinmazDto>> AddTasinmazAsync(TasinmazDto tasinmazDto);
        Task<ServiceResponse<TasinmazDto>> UpdateTasinmazAsync(TasinmazDto tasinmazDto);
        Task<ServiceResponse<TasinmazDto>> DeleteTasinmazAsync(TasinmazDto tasinmazDto);



    }
}