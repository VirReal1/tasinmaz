using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;
using tasinmaz.API.Models;

namespace tasinmaz.API.Data
{
    public interface ILocationRepository<T>
    {
        Task<T> Get(Expression<Func<T, bool>> filter);
        Task<ICollection<T>> GetAll();
        Task<ICollection<T>> GetById(Expression<Func<T, bool>> filter);
        Task<bool> Exists(Expression<Func<T, bool>> filter);
        Task<bool> AddAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(T entity);

    }
}