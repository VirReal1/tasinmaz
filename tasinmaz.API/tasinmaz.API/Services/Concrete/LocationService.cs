using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using tasinmaz.API.Data;
using tasinmaz.API.Dtos.Tasinmaz;
using tasinmaz.API.Entities.Abstract;
using tasinmaz.API.Entities.Concrete;
using tasinmaz.API.Models;
using tasinmaz.API.ServiceResponder;
using tasinmaz.API.Services.Abstract;

namespace tasinmaz.API.Services.Concrete
{
    public class LocationService : ILocationService
    {
        ILocationRepository<Il> _ilRepository;
        ILocationRepository<Ilce> _ilceRepository;
        ILocationRepository<Mahalle> _mahalleRepository;

        public LocationService(ILocationRepository<Il> ilRepository, ILocationRepository<Ilce> ilceRepository, ILocationRepository<Mahalle> mahalleRepository)
        {
            _ilRepository = ilRepository;
            _ilceRepository = ilceRepository;
            _mahalleRepository = mahalleRepository;
        }

        public async Task<ServiceResponse<List<Il>>> GetIllerAsync()
        {
            ServiceResponse<List<Il>> response = new ServiceResponse<List<Il>>();

            try
            {
                var illerAllList = await _ilRepository.GetAll();

                if (illerAllList.Count == 0)
                {
                    response.Process = "Iller";
                    response.Message = "Iller veri tabanında bulunamadı.";
                    response.Warning = true;
                    response.Data = null;
                    return response;
                }

                response.Process = "Iller";
                response.Message = "Bütün iller getirildi.";
                response.Data = illerAllList.ToList();
            }
            catch (Exception e)
            {
                response.Process = "Iller";
                response.Message = "Illeri getirirken servis katmanında bir hata oluştu.";
                response.Error = true;
                response.Data = null;
                response.ErrorMessages = new List<string> { Convert.ToString(e.Message) };
            }
            return response;
        }

        public async Task<ServiceResponse<List<Ilce>>> GetIlceByIlIdAsync(string ilAdi)
        {
            ServiceResponse<List<Ilce>> response = new ServiceResponse<List<Ilce>>();

            try
            {
                var il = await _ilRepository.Get(x => x.Adi == ilAdi);
                var ilceList = await _ilceRepository.GetById(x => x.IlId == il.Id);

                if (ilceList == null)
                {
                    response.Process = "Ilçeler";
                    response.Message = "Ile bağlı ilçeler veri tabanında bulunamadı.";
                    response.Warning = true;
                    response.Data = null;
                    return response;
                }
                response.Process = "Ilçeler";
                response.Message = "Ilçeler başarıyla getirildi.";
                response.Data = ilceList.ToList();
            }
            catch (Exception e)
            {
                response.Process = "Ilçeler";
                response.Message = "İlçeleri getirirken servis katmanında bir hata oluştu.";
                response.Error = true;
                response.Data = null;
                response.ErrorMessages = new List<string> { Convert.ToString(e.Message) };
            }

            return response;
        }

        public async Task<ServiceResponse<List<Mahalle>>> GetMahalleByIlceIdAsync(string ilceAdi)
        {
            ServiceResponse<List<Mahalle>> response = new ServiceResponse<List<Mahalle>>();

            try
            {
                var ilce = await _ilceRepository.Get(x => x.Adi == ilceAdi);
                var ilceList = await _mahalleRepository.GetById(x => x.IlceId == ilce.Id);

                if (ilceList == null)
                {
                    response.Process = "Mahalleler";
                    response.Message = "Ilceye bağlı mahalleler veri tabanında bulunamadı.";
                    response.Warning = true;
                    response.Data = null;
                    return response;
                }
                response.Process = "Mahalleler";
                response.Message = "Mahalleler başarıyla getirildi.";
                response.Data = ilceList.ToList();
            }
            catch (Exception e)
            {
                response.Process = "Mahalleler";
                response.Message = "Mahalleleri getirirken servis katmanında bir hata oluştu.";
                response.Error = true;
                response.Data = null;
                response.ErrorMessages = new List<string> { Convert.ToString(e.Message) };
            }

            return response;
        }
    }
}