using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using System.Threading.Tasks;
using tasinmaz.API.Dtos.Log;
using tasinmaz.API.Entities.Concrete;

namespace tasinmaz.API.Data
{
    public interface ILogRepository
    {
        Task<bool> AddAsync(Log log);
        Task<List<Log>> GetAllAsync(Expression<Func<Log, bool>> filter = null);
    }
}