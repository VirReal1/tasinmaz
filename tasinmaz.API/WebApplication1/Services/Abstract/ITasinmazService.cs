using System.Collections.Generic;
using System.Threading.Tasks;
using tasinmaz.API.Dtos.Tasinmaz;
using tasinmaz.API.Entities.Concrete;
using tasinmaz.API.Models;

namespace tasinmaz.API.Services.Abstract
{
    public interface ITasinmazService
    {
        Task<LogKaydi<List<TasinmazDto>>> GetTasinmazlarAsync(TasinmazDto tasinmazDto);
        Task<LogKaydi<TasinmazDto>> AddTasinmazAsync(TasinmazDto tasinmazDto);
        Task<LogKaydi<TasinmazDto>> UpdateTasinmazAsync(TasinmazDto tasinmazDto);
        Task<LogKaydi<TasinmazDto>> DeleteTasinmazAsync(TasinmazDto tasinmazDto);



    }
}