using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace tasinmaz.API.Data
{
    public interface IGenericRepository<T>
    {
        Task<T> GetAsync(Expression<Func<T, bool>> filter);
        Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>> filter = null);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> filter);
        Task<bool> AddAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(T entity);
    }
}