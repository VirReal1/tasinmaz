using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using tasinmaz.API.Data;
using tasinmaz.API.Dtos.Log;
using tasinmaz.API.Dtos.Tasinmaz;
using tasinmaz.API.Entities.Concrete;
using tasinmaz.API.Models;
using tasinmaz.API.ServiceResponder;
using tasinmaz.API.Services.Abstract;

namespace tasinmaz.API.Services.Concrete
{
    public class TasinmazService : ITasinmazService
    {
        ITasinmazRepository _tasinmazRepository;
        IHttpContextAccessor _httpAccessor;
        IMapper _mapper;
        ILogService _logService;

        public TasinmazService(ITasinmazRepository tasinmazRepository, IHttpContextAccessor httpAccessor, IMapper mapper, ILogService logService, IUserRepository userRepository)
        {
            _tasinmazRepository = tasinmazRepository;
            _httpAccessor = httpAccessor;
            _mapper = mapper;
            _logService = logService;
        }

        public async Task<ServiceResponse<List<TasinmazDto>>> GetAllAsync(int logKullaniciId)
        {
            ServiceResponse<List<TasinmazDto>> response = new ServiceResponse<List<TasinmazDto>>();
            Log log = new Log();

            try
            {
                ICollection<Tasinmaz> tasinmazlarAllList;

                if (logKullaniciId != 0)
                {
                    tasinmazlarAllList = await _tasinmazRepository.GetAllAsync(x => x.KullaniciId == logKullaniciId);
                }
                else
                {
                    tasinmazlarAllList = await _tasinmazRepository.GetAllAsync();
                }

                var tasinmazlarDtoAllList = new List<TasinmazDto>();

                foreach (var item in tasinmazlarAllList)
                {
                    tasinmazlarDtoAllList.Add(_mapper.Map<TasinmazDto>(item));
                }

                response.Process = "Taşınmazlar";
                response.Message = "Bütün taşınmazlar getirildi.";
                response.Data = tasinmazlarDtoAllList;

                log.KullaniciId = logKullaniciId;
                log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                log.Tarih = DateTime.Now;
                log.Durum = "Başarılı";
                log.Islem = "Getirme";
                log.Aciklama = $"Açıklama: \"{response.Message}.\" - Veri: \"{JsonConvert.SerializeObject(response.Data)}\"";
                await _logService.AddLogAsync(log);
            }
            catch (Exception e)
            {
                response.Process = "Taşınmazlar";
                response.Message = "Taşınmazları getirirken servis katmanında bir hata oluştu.";
                response.Error = true;
                response.Data = null;
                response.ErrorMessages = new List<string> { Convert.ToString(e.Message) };

                log.KullaniciId = logKullaniciId;
                log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                log.Tarih = DateTime.Now;
                log.Durum = "Başarısız";
                log.Islem = "Getirme";
                log.Aciklama = $"Açıklama: \"{response.Message}\" - Veri: \"Veri yok.\" - Hata Mesajları: \"{JsonConvert.SerializeObject(response.ErrorMessages)}\"";
                await _logService.AddLogAsync(log);
            }
            return response;
        }
        public async Task<ServiceResponse<List<TasinmazDto>>> GetTasinmazlarAsync(TasinmazDto tasinmazDto)
        {
            ServiceResponse<List<TasinmazDto>> response = new ServiceResponse<List<TasinmazDto>>();
            Log log = new Log();

            try
            {
                var tasinmazlarList = await _tasinmazRepository.GetAllAsync(x =>
                    (tasinmazDto.Id == default || x.Id.ToString().ToLower().Contains(tasinmazDto.Id.ToString().ToLower())) &&
                    (tasinmazDto.Adi == null || x.Adi.ToLower().Contains(tasinmazDto.Adi.ToLower())) &&
                    (tasinmazDto.IlAdi == null || x.IlAdi.ToLower().Contains(tasinmazDto.IlAdi.ToLower())) &&
                    (tasinmazDto.IlceAdi == null || x.IlceAdi.ToLower().Contains(tasinmazDto.IlceAdi.ToLower())) &&
                    (tasinmazDto.MahalleAdi == null || x.MahalleAdi.ToLower().Contains(tasinmazDto.MahalleAdi.ToLower())) &&
                    (tasinmazDto.Ada == null || x.Ada.ToLower().Contains(tasinmazDto.Ada.ToLower())) &&
                    (tasinmazDto.Parsel == null || x.Parsel.ToLower().Contains(tasinmazDto.Parsel.ToLower())) &&
                    (tasinmazDto.Nitelik == null || x.Nitelik.ToLower().Contains(tasinmazDto.Nitelik.ToLower())) &&
                    (tasinmazDto.KoordinatBilgileri == null || x.KoordinatBilgileri.ToLower().Contains(tasinmazDto.KoordinatBilgileri.ToLower())) &&
                    (tasinmazDto.LogKullaniciId == default || x.KullaniciId.ToString().ToLower().Contains(tasinmazDto.LogKullaniciId.ToString().ToLower())));

                if (tasinmazlarList.Count == 0)
                {
                    response.Process = "Taşınmazlar";
                    response.Message = "Arama parametreleri veri tabanıyla eşleşmedi.";
                    response.Warning = true;
                    response.Data = null;

                    log.KullaniciId = tasinmazDto.LogKullaniciId;
                    log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                    log.Tarih = DateTime.Now;
                    log.Durum = "Başarısız";
                    log.Islem = "Arama";
                    log.Aciklama = $"Açıklama: \"{response.Message}.\" - Veri: \"Veri yok.\"";
                    await _logService.AddLogAsync(log);
                    return response;
                }
                response.Process = "Taşınmazlar";
                response.Message = "Taşınmazlar başarıyla filtrelendi.";
                response.Data = _mapper.Map<List<TasinmazDto>>(tasinmazlarList);

                log.KullaniciId = tasinmazDto.LogKullaniciId;
                log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                log.Tarih = DateTime.Now;
                log.Durum = "Başarılı";
                log.Islem = "Arama";
                log.Aciklama = $"Açıklama: \"{response.Message}.\" - Veri: \"{JsonConvert.SerializeObject(response.Data)}\"";
                await _logService.AddLogAsync(log);
            }
            catch (Exception e)
            {
                response.Process = "Taşınmazlar";
                response.Message = "Taşınmazları filtrelerken servis katmanında bir hata oluştu.";
                response.Error = true;
                response.Data = null;
                response.ErrorMessages = new List<string> { Convert.ToString(e.Message) };

                log.KullaniciId = tasinmazDto.LogKullaniciId;
                log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                log.Tarih = DateTime.Now;
                log.Durum = "Başarısız";
                log.Islem = "Arama";
                log.Aciklama = $"Açıklama: \"{response.Message}\" - Veri: \"Veri yok.\" - Hata Mesajları: \"{JsonConvert.SerializeObject(response.ErrorMessages)}\"";
                await _logService.AddLogAsync(log);
            }
            return response;
        }

        public async Task<ServiceResponse<TasinmazDto>> AddTasinmazAsync(TasinmazDto tasinmazDto)
        {
            ServiceResponse<TasinmazDto> response = new ServiceResponse<TasinmazDto>();
            Log log = new Log();

            try
            {
                var tasinmazExists = await _tasinmazRepository.ExistsAsync(x => x.Adi == tasinmazDto.Adi);
                if (tasinmazExists)
                {
                    response.Process = "Taşınmazlar";
                    response.Message = "Taşınmaz veri tabanında mevcut.";
                    response.Warning = true;
                    response.Data = null;

                    log.KullaniciId = tasinmazDto.LogKullaniciId;
                    log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                    log.Tarih = DateTime.Now;
                    log.Durum = "Başarısız";
                    log.Islem = "Yeni Kayıt";
                    log.Aciklama = $"Açıklama: \"{response.Message}.\" - Veri: \"Veri yok.\"";
                    await _logService.AddLogAsync(log);
                    return response;
                }

                var createdTasinmaz = _mapper.Map<Tasinmaz>(tasinmazDto);

                if (!await _tasinmazRepository.AddAsync(createdTasinmaz))
                {
                    response.Process = "Taşınmazlar";
                    response.Message = "Taşınmaz veri tabanına eklenirken bir hata oluştu.";
                    response.Error = true;
                    response.Data = null;

                    log.KullaniciId = tasinmazDto.LogKullaniciId;
                    log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                    log.Tarih = DateTime.Now;
                    log.Durum = "Başarısız";
                    log.Islem = "Yeni Kayıt";
                    log.Aciklama = $"Açıklama: \"Taşınmaz: \"{tasinmazDto.Adi}\" veri tabanına eklenirken bir hata oluştu.\" - Veri: \"Veri yok.\"";
                    await _logService.AddLogAsync(log);
                    return response;
                }

                response.Process = "Taşınmazlar";
                response.Message = "Taşınmaz başarıyla eklendi.";
                response.Data = _mapper.Map<TasinmazDto>(createdTasinmaz);

                log.KullaniciId = tasinmazDto.LogKullaniciId;
                log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                log.Tarih = DateTime.Now;
                log.Durum = "Başarılı";
                log.Islem = "Yeni Kayıt";
                log.Aciklama = $"Açıklama: \"{response.Message}\" - Veri: \"{JsonConvert.SerializeObject(response.Data)}\"";
                await _logService.AddLogAsync(log);
            }
            catch (Exception e)
            {
                response.Process = "Taşınmazlar";
                response.Message = "Taşınmaz servis katmanında eklenirken bir hata oluştu.";
                response.Error = true;
                response.Data = null;
                response.ErrorMessages = new List<string> { Convert.ToString(e.Message) };

                log.KullaniciId = tasinmazDto.LogKullaniciId;
                log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                log.Tarih = DateTime.Now;
                log.Durum = "Başarısız";
                log.Islem = "Yeni Kayıt";
                log.Aciklama = $"Açıklama: \"Taşınmaz: \"{tasinmazDto.Adi}\" servis katmanında eklenirken bir hata oluştu.\" - Veri: \"Veri yok.\" - Hata Mesajları: \"{JsonConvert.SerializeObject(response.ErrorMessages)}\"";
                await _logService.AddLogAsync(log);
            }
            return response;
        }

        public async Task<ServiceResponse<TasinmazDto>> UpdateTasinmazAsync(TasinmazDto tasinmazDto)
        {
            ServiceResponse<TasinmazDto> response = new ServiceResponse<TasinmazDto>();
            Log log = new Log();

            try
            {
                var existingTasinmaz = await _tasinmazRepository.GetAsync(x => x.Id == tasinmazDto.Id);

                if (existingTasinmaz == null)
                {
                    response.Process = "Taşınmazlar";
                    response.Message = "Taşınmaz veri tabanında bulunamadı.";
                    response.Warning = true;
                    response.Data = null;

                    log.KullaniciId = tasinmazDto.LogKullaniciId;
                    log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                    log.Tarih = DateTime.Now;
                    log.Durum = "Başarısız";
                    log.Islem = "Güncelleme";
                    log.Aciklama = $"Açıklama: \"{response.Message}.\" - Veri: \"Veri yok.\"";
                    await _logService.AddLogAsync(log);
                    return response;
                }

                existingTasinmaz = _mapper.Map<Tasinmaz>(tasinmazDto);

                if (!await _tasinmazRepository.UpdateAsync(existingTasinmaz))
                {
                    response.Process = "Taşınmazlar";
                    response.Message = "Taşınmaz veri tabanına güncellenirken bir hata oluştu.";
                    response.Error = true;
                    response.Data = null;

                    log.KullaniciId = tasinmazDto.LogKullaniciId;
                    log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                    log.Tarih = DateTime.Now;
                    log.Durum = "Başarısız";
                    log.Islem = "Güncelleme";
                    log.Aciklama = $"Açıklama: \"Taşınmaz: \"{tasinmazDto.Adi}\" veri tabanına güncellenirken bir hata oluştu.\" - Veri: \"Veri yok.\"";
                    await _logService.AddLogAsync(log);
                    return response;
                }
                response.Process = "Taşınmazlar";
                response.Message = "Taşınmaz başarıyla güncellendi.";
                response.Data = _mapper.Map<TasinmazDto>(existingTasinmaz);

                log.KullaniciId = tasinmazDto.LogKullaniciId;
                log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                log.Tarih = DateTime.Now;
                log.Durum = "Başarılı";
                log.Islem = "Güncelleme";
                log.Aciklama = $"Açıklama: \"{response.Message}\" - Veri: \"{JsonConvert.SerializeObject(response.Data)}\"";
                await _logService.AddLogAsync(log);
            }
            catch (Exception e)
            {
                response.Process = "Taşınmazlar";
                response.Message = "Taşınmaz servis katmanında güncellenirken bir hata oluştu.";
                response.Error = true;
                response.Data = null;
                response.ErrorMessages = new List<string> { Convert.ToString(e.Message) };

                log.KullaniciId = tasinmazDto.LogKullaniciId;
                log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                log.Tarih = DateTime.Now;
                log.Durum = "Başarısız";
                log.Islem = "Güncelleme";
                log.Aciklama = $"Açıklama: \"Taşınmaz: \"{tasinmazDto.Adi}\" servis katmanında güncellenirken bir hata oluştu.\" - Veri: \"Veri yok.\" - Hata Mesajları: \"{JsonConvert.SerializeObject(response.ErrorMessages)}\"";
                await _logService.AddLogAsync(log);
            }
            return response;
        }

        public async Task<ServiceResponse<TasinmazDto>> DeleteTasinmazAsync(int logKullaniciId, int kullaniciId)
        {
            ServiceResponse<TasinmazDto> response = new ServiceResponse<TasinmazDto>();
            Log log = new Log();

            try
            {
                if (!await _tasinmazRepository.ExistsAsync(x => x.Id == kullaniciId))
                {
                    response.Process = "Taşınmazlar";
                    response.Message = "Taşınmaz veri tabanında bulunamadı.";
                    response.Warning = true;
                    response.Data = null;

                    log.KullaniciId = logKullaniciId;
                    log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                    log.Tarih = DateTime.Now;
                    log.Durum = "Başarısız";
                    log.Islem = "Silme";
                    log.Aciklama = $"Açıklama: \"{response.Message}.\" - Veri: \"Veri yok.\"";
                    await _logService.AddLogAsync(log);
                    return response;
                }

                if (!await _tasinmazRepository.DeleteAsync(x=>x.Id == kullaniciId))
                {
                    response.Process = "Taşınmazlar";
                    response.Message = "Taşınmaz veri tabanından silinirken bir hata oluştu.";
                    response.Error = true;
                    response.Data = null;

                    log.KullaniciId = logKullaniciId;
                    log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                    log.Tarih = DateTime.Now;
                    log.Durum = "Başarısız";
                    log.Islem = "Silme";
                    log.Aciklama = $"Açıklama: \"Taşınmaz veri tabanından silinirken bir hata oluştu.\" - Veri: \"Veri yok.\"";
                    await _logService.AddLogAsync(log);
                    return response;
                }
                response.Process = "Taşınmazlar";
                response.Message = "Taşınmaz başarıyla silindi.";
                response.Data = null;

                log.KullaniciId = logKullaniciId;
                log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                log.Tarih = DateTime.Now;
                log.Durum = "Başarılı";
                log.Islem = "Silme";
                log.Aciklama = $"Açıklama: \"{response.Message}\" - Veri: \"{JsonConvert.SerializeObject(response.Data)}\"";
                await _logService.AddLogAsync(log);
            }
            catch (Exception e)
            {
                response.Process = "Taşınmazlar";
                response.Message = "Taşınmaz servis katmanından silinirken bir hata oluştu.";
                response.Error = true;
                response.Data = null;
                response.ErrorMessages = new List<string> { Convert.ToString(e.Message) };

                log.KullaniciId = logKullaniciId;
                log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                log.Tarih = DateTime.Now;
                log.Durum = "Başarısız";
                log.Islem = "Silme";
                log.Aciklama = $"Açıklama: \"Taşınmaz servis katmanından silinirken bir hata oluştu.\" - Veri: \"Veri yok.\" - Hata Mesajları: \"{JsonConvert.SerializeObject(response.ErrorMessages)}\"";
                await _logService.AddLogAsync(log);
            }
            return response;
        }
    }
}