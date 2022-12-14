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
        IHttpContextAccessor _httpAccessor;
        ILogService _logService;

        public LocationService(ILocationRepository<Il> ilRepository, ILocationRepository<Ilce> ilceRepository, ILocationRepository<Mahalle> mahalleRepository, IHttpContextAccessor httpAccessor, ILogService logService)
        {
            _ilRepository = ilRepository;
            _ilceRepository = ilceRepository;
            _mahalleRepository = mahalleRepository;
            _httpAccessor = httpAccessor;
            _logService = logService;
        }

        public async Task<ServiceResponse<List<Il>>> GetIllerAsync()
        {
            ServiceResponse<List<Il>> response = new ServiceResponse<List<Il>>();
            Log log = new Log();

            try
            {
                var illerAllList = await _ilRepository.GetAll();

                response.Process = "Iller";
                response.Message = "Bütün iller getirildi.";
                response.Data = illerAllList.ToList();

                log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                log.Tarih = DateTime.Now;
                log.Durum = "Başarılı";
                log.Islem = "Getirme";
                log.Aciklama = $"Açıklama: \"{response.Message}.\" - Veri: \"{JsonConvert.SerializeObject(response.Data)}\"";
                await _logService.AddAsync(log);
            }
            catch (Exception e)
            {
                response.Process = "Iller";
                response.Message = "Illeri getirirken servis katmanında bir hata oluştu.";
                response.Error = true;
                response.Data = null;
                response.ErrorMessages = new List<string> { Convert.ToString(e.Message) };

                log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                log.Tarih = DateTime.Now;
                log.Durum = "Başarısız";
                log.Islem = "Getirme";
                log.Aciklama = $"Açıklama: \"{response.Message}\" - Veri: \"Veri yok.\" - Hata Mesajları: \"{JsonConvert.SerializeObject(response.ErrorMessages)}\"";
                await _logService.AddAsync(log);
            }
            return response;
        }

        public async Task<ServiceResponse<List<Ilce>>> GetIlceByIlIdAsync(int ilId, int kullaniciId)
        {
            ServiceResponse<List<Ilce>> response = new ServiceResponse<List<Ilce>>();
            Log log = new Log();

            try
            {
                var ilceList = await _ilceRepository.GetById(x => x.IlId == ilId);

                if (ilceList == null)
                {
                    response.Process = "Ilçeler";
                    response.Message = "Ile bağlı ilçeler veri tabanında bulunamadı.";
                    response.Warning = true;
                    response.Data = null;

                    log.KullaniciId = kullaniciId;
                    log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                    log.Tarih = DateTime.Now;
                    log.Durum = "Başarısız";
                    log.Islem = "Getirme";
                    log.Aciklama = $"Açıklama: \"{response.Message}.\" - Veri: \"Veri yok.\"";
                    await _logService.AddAsync(log);
                    return response;
                }
                response.Process = "Ilçeler";
                response.Message = "Ilçeler başarıyla getirildi.";
                response.Data = ilceList.ToList();

                log.KullaniciId = kullaniciId;
                log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                log.Tarih = DateTime.Now;
                log.Durum = "Başarılı";
                log.Islem = "Arama";
                log.Aciklama = $"Açıklama: \"{response.Message}.\" - Veri: \"{JsonConvert.SerializeObject(response.Data)}\"";
                await _logService.AddAsync(log);
            }
            catch (Exception e)
            {
                response.Process = "Ilçeler";
                response.Message = "İlçeleri getirirken servis katmanında bir hata oluştu.";
                response.Error = true;
                response.Data = null;
                response.ErrorMessages = new List<string> { Convert.ToString(e.Message) };

                log.KullaniciId = kullaniciId;
                log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                log.Tarih = DateTime.Now;
                log.Durum = "Başarısız";
                log.Islem = "Arama";
                log.Aciklama = $"Açıklama: \"{response.Message}\" - Veri: \"Veri yok.\" - Hata Mesajları: \"{JsonConvert.SerializeObject(response.ErrorMessages)}\"";
                await _logService.AddAsync(log);
            }

            return response;
        }

        public async Task<ServiceResponse<List<Mahalle>>> GetMahalleByIlceIdAsync(int ilceId, int kullaniciId)
        {
            ServiceResponse<List<Mahalle>> response = new ServiceResponse<List<Mahalle>>();
            Log log = new Log();

            try
            {
                var ilceList = await _mahalleRepository.GetById(x => x.IlceId == ilceId);

                if (ilceList == null)
                {
                    response.Process = "Mahalleler";
                    response.Message = "Ilceye bağlı mahalleler veri tabanında bulunamadı.";
                    response.Warning = true;
                    response.Data = null;

                    log.KullaniciId = kullaniciId;
                    log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                    log.Tarih = DateTime.Now;
                    log.Durum = "Başarısız";
                    log.Islem = "Getirme";
                    log.Aciklama = $"Açıklama: \"{response.Message}.\" - Veri: \"Veri yok.\"";
                    await _logService.AddAsync(log);
                    return response;
                }
                response.Process = "Mahalleler";
                response.Message = "Mahalleler başarıyla getirildi.";
                response.Data = ilceList.ToList();

                log.KullaniciId = kullaniciId;
                log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                log.Tarih = DateTime.Now;
                log.Durum = "Başarılı";
                log.Islem = "Arama";
                log.Aciklama = $"Açıklama: \"{response.Message}.\" - Veri: \"{JsonConvert.SerializeObject(response.Data)}\"";
                await _logService.AddAsync(log);
            }
            catch (Exception e)
            {
                response.Process = "Mahalleler";
                response.Message = "Mahalleleri getirirken servis katmanında bir hata oluştu.";
                response.Error = true;
                response.Data = null;
                response.ErrorMessages = new List<string> { Convert.ToString(e.Message) };

                log.KullaniciId = kullaniciId;
                log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                log.Tarih = DateTime.Now;
                log.Durum = "Başarısız";
                log.Islem = "Arama";
                log.Aciklama = $"Açıklama: \"{response.Message}\" - Veri: \"Veri yok.\" - Hata Mesajları: \"{JsonConvert.SerializeObject(response.ErrorMessages)}\"";
                await _logService.AddAsync(log);
            }

            return response;
        }
    }
}