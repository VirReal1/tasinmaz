using System.Collections.Generic;
using System.Threading.Tasks;
using tasinmaz.API.Dtos.Log;
using tasinmaz.API.Entities.Concrete;
using tasinmaz.API.ServiceResponder;

namespace tasinmaz.API.Services.Abstract
{
    public interface ILogService
    {
        Task<ServiceResponse<List<LogDto>>> GetAllAsync();
        Task<ServiceResponse<List<LogDto>>> GetLoglarAsync(LogDto logDto);
        Task<ServiceResponse<LogDto>> AddAsync(Log log);

    }
}