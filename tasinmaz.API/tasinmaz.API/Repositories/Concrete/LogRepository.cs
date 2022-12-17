using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using tasinmaz.API.Dtos.Log;
using tasinmaz.API.Entities.Concrete;

namespace tasinmaz.API.Data
{
    public class LogRepository : ILogRepository
    {
        DataContext _context;
        public LogRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<Log>> GetAllAsync(Expression<Func<Log, bool>> filter = null)
        {
            return filter == null ? await _context.Loglar.ToListAsync() : await _context.Loglar.Where(filter).ToListAsync();
        }

        public async Task<bool> AddAsync(Log log)
        {
            await _context.Loglar.AddAsync(log);
            return await SaveChanges();
        }

        private async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}