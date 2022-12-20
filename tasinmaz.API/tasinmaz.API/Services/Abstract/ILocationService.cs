using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using tasinmaz.API.Entities.Abstract;
using tasinmaz.API.Models;
using tasinmaz.API.ServiceResponder;

namespace tasinmaz.API.Services.Abstract
{
    public interface ILocationService
    {
        Task<ServiceResponse<List<Il>>> GetIllerAsync();
        Task<ServiceResponse<List<Ilce>>> GetIlceByIlIdAsync(string ilAdi);
        Task<ServiceResponse<List<Mahalle>>> GetMahalleByIlceIdAsync(string ilceAdi);
    }
}