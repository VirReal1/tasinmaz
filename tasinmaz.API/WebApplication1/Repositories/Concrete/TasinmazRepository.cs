using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using tasinmaz.API.Models;

namespace tasinmaz.API.Data
{
    public class TasinmazRepository : GenericRepository<Tasinmaz>, ITasinmazRepository
    {
        public TasinmazRepository(DataContext context) : base(context)
        {

        }
    }
}