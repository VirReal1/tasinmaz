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

        public async Task<ServiceResponse<List<TasinmazDto>>> GetAllAsync()
        {
            ServiceResponse<List<TasinmazDto>> response = new ServiceResponse<List<TasinmazDto>>();
            Log log = new Log();

            var tasinmazlarAllList = await _tasinmazRepository.GetAllAsync();
            var tasinmazlarDtoAllList = new List<TasinmazDto>();

            foreach (var item in tasinmazlarAllList)
            {
                tasinmazlarDtoAllList.Add(_mapper.Map<TasinmazDto>(item));
            }

            try
            {
                response.Process = "Taşınmazlar";
                response.Message = "Got all Taşınmazlar.";
                response.Success = true;
                response.Data = tasinmazlarDtoAllList;

                log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                log.Tarih = DateTime.Now;
                log.Durum = "Başarılı";
                log.Islem = "Getirme";
                log.Aciklama = $"Açıklama: \"{response.Message}.\" - Veri: \"{JsonConvert.SerializeObject(response.Data)}\"";
                await _logService.AddAsync(log);
            }
            catch (Exception e)
            {
                response.Process = "Taşınmazlar";
                response.Message = "Error occured.";
                response.Success = false;
                response.Data = tasinmazlarDtoAllList;
                response.ErrorMessages = new List<string> { Convert.ToString(e.Message) };

                log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                log.Tarih = DateTime.Now;
                log.Durum = "Başarısız";
                log.Islem = "Getirme";
                log.Aciklama = $"Açıklama: \"{response.Message}\" - Veri: \"No Data\" - Hata Mesajları: \"{JsonConvert.SerializeObject(response.ErrorMessages)}\"";
                await _logService.AddAsync(log);
            }
            return response;
        }
        public async Task<ServiceResponse<List<TasinmazDto>>> GetTasinmazlarAsync(TasinmazDto tasinmazDto)
        {
            ServiceResponse<List<TasinmazDto>> response = new ServiceResponse<List<TasinmazDto>>();
            Log log = new Log();

            var tasinmazlarAllList = await _tasinmazRepository.GetAllAsync();
            var tasinmazlarDtoAllList = new List<TasinmazDto>();
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
                (tasinmazDto.KullaniciId == default || x.KullaniciId.ToString().ToLower().Contains(tasinmazDto.KullaniciId.ToString().ToLower())));

            foreach (var item in tasinmazlarAllList)
            {
                tasinmazlarDtoAllList.Add(_mapper.Map<TasinmazDto>(item));
            }

            try
            {
                if (tasinmazDto == null)
                {
                    response.Process = "Taşınmazlar";
                    response.Message = "No Parameter";
                    response.Success = false;
                    response.Data = tasinmazlarDtoAllList;

                    log.KullaniciId = tasinmazDto.KullaniciId;
                    log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                    log.Tarih = DateTime.Now;
                    log.Durum = "Başarısız";
                    log.Islem = "Arama";
                    log.Aciklama = $"Açıklama: \"{response.Message}.\" - Veri: \"No Data\"";
                    await _logService.AddAsync(log);
                    return response;
                }

                if (tasinmazlarList == null)
                {
                    response.Process = "Taşınmazlar";
                    response.Message = "Parameters does not match with the database.";
                    response.Success = false;
                    response.Data = tasinmazlarDtoAllList;

                    log.KullaniciId = tasinmazDto.KullaniciId;
                    log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                    log.Tarih = DateTime.Now;
                    log.Durum = "Başarısız";
                    log.Islem = "Arama";
                    log.Aciklama = $"Açıklama: \"{response.Message}.\" - Veri: \"No Data\"";
                    await _logService.AddAsync(log);
                    return response;
                }
                response.Process = "Taşınmazlar";
                response.Message = "Taşınmazlar searched.";
                response.Success = false;
                response.Data = _mapper.Map<List<TasinmazDto>>(tasinmazlarList);

                log.KullaniciId = tasinmazDto.KullaniciId;
                log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                log.Tarih = DateTime.Now;
                log.Durum = "Başarılı";
                log.Islem = "Arama";
                log.Aciklama = $"Açıklama: \"{response.Message}.\" - Veri: \"{JsonConvert.SerializeObject(response.Data)}\"";
                await _logService.AddAsync(log);
            }
            catch (Exception e)
            {
                response.Process = "Taşınmazlar";
                response.Message = "Error occured.";
                response.Success = false;
                response.Data = tasinmazlarDtoAllList;
                response.ErrorMessages = new List<string> { Convert.ToString(e.Message) };

                log.KullaniciId = tasinmazDto.KullaniciId;
                log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                log.Tarih = DateTime.Now;
                log.Durum = "Başarısız";
                log.Islem = "Arama";
                log.Aciklama = $"Açıklama: \"{response.Message}\" - Veri: \"No Data\" - Hata Mesajları: \"{JsonConvert.SerializeObject(response.ErrorMessages)}\"";
                await _logService.AddAsync(log);
            }
            return response;
        }

        public async Task<ServiceResponse<TasinmazDto>> AddTasinmazAsync(TasinmazDto tasinmazDto)
        {
            ServiceResponse<TasinmazDto> response = new ServiceResponse<TasinmazDto>();
            Log log = new Log();

            try
            {
                var tasinmazExists = await _tasinmazRepository.Exists(x => x.Adi == tasinmazDto.Adi);
                if (tasinmazExists)
                {
                    response.Process = "Taşınmazlar";
                    response.Message = "Taşınmaz exists.";
                    response.Success = false;
                    response.Data = null;

                    log.KullaniciId = tasinmazDto.KullaniciId;
                    log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                    log.Tarih = DateTime.Now;
                    log.Durum = "Başarısız";
                    log.Islem = "Yeni Kayıt";
                    log.Aciklama = $"Açıklama: \"{response.Message}.\" - Veri: \"No Data\"";
                    await _logService.AddAsync(log);
                    return response;
                }

                var createdTasinmaz = _mapper.Map<Tasinmaz>(tasinmazDto);

                if (!await _tasinmazRepository.AddAsync(createdTasinmaz))
                {
                    response.Process = "Taşınmazlar";
                    response.Message = "Repository error.";
                    response.Success = false;
                    response.Data = null;

                    log.KullaniciId = tasinmazDto.KullaniciId;
                    log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                    log.Tarih = DateTime.Now;
                    log.Durum = "Başarısız";
                    log.Islem = "Yeni Kayıt";
                    log.Aciklama = $"Açıklama: \"Something went wrong in repository layer when adding taşınmaz {tasinmazDto}.\" - Veri: \"No Data\"";
                    await _logService.AddAsync(log);
                    return response;
                }
                response.Process = "Taşınmazlar";
                response.Message = "Tasinmaz created";
                response.Success = true;
                response.Data = _mapper.Map<TasinmazDto>(createdTasinmaz);

                log.KullaniciId = tasinmazDto.KullaniciId;
                log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                log.Tarih = DateTime.Now;
                log.Durum = "Başarılı";
                log.Islem = "Yeni Kayıt";
                log.Aciklama = $"Açıklama: \"{response.Message}\" - Veri: \"{JsonConvert.SerializeObject(response.Data)}\"";
                await _logService.AddAsync(log);
            }
            catch (Exception e)
            {
                response.Process = "Taşınmazlar";
                response.Message = "Error occured.";
                response.Success = false;
                response.Data = null;
                response.ErrorMessages = new List<string> { Convert.ToString(e.Message) };

                log.KullaniciId = tasinmazDto.KullaniciId;
                log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                log.Tarih = DateTime.Now;
                log.Durum = "Başarısız";
                log.Islem = "Yeni Kayıt";
                log.Aciklama = $"Açıklama: \"Something went wrong in service layer when adding taşınmaz {tasinmazDto}.\" - Veri: \"No Data\" - Hata Mesajları: \"{JsonConvert.SerializeObject(response.ErrorMessages)}\"";
                await _logService.AddAsync(log);
            }
            return response;
        }

        public async Task<ServiceResponse<TasinmazDto>> UpdateTasinmazAsync(TasinmazDto tasinmazDto)
        {
            ServiceResponse<TasinmazDto> response = new ServiceResponse<TasinmazDto>();
            Log log = new Log();

            try
            {
                var existingTasinmaz = await _tasinmazRepository.Get(x => x.Id == tasinmazDto.Id);

                if (existingTasinmaz == null)
                {
                    response.Process = "Taşınmazlar";
                    response.Message = "Not found.";
                    response.Success = false;
                    response.Data = null;

                    log.KullaniciId = tasinmazDto.KullaniciId;
                    log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                    log.Tarih = DateTime.Now;
                    log.Durum = "Başarısız";
                    log.Islem = "Güncelleme";
                    log.Aciklama = $"Açıklama: \"{response.Message}.\" - Veri: \"No Data\"";
                    await _logService.AddAsync(log);
                    return response;
                }

                existingTasinmaz = _mapper.Map<Tasinmaz>(tasinmazDto);

                if (!await _tasinmazRepository.UpdateAsync(existingTasinmaz))
                {
                    response.Process = "Taşınmazlar";
                    response.Message = "Repository error.";
                    response.Success = false;
                    response.Data = null;

                    log.KullaniciId = tasinmazDto.KullaniciId;
                    log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                    log.Tarih = DateTime.Now;
                    log.Durum = "Başarısız";
                    log.Islem = "Güncelleme";
                    log.Aciklama = $"Açıklama: \"Something went wrong in repository layer when updating taşınmaz {tasinmazDto}.\" - Veri: \"No Data\"";
                    await _logService.AddAsync(log);
                    return response;
                }
                response.Process = "Taşınmazlar";
                response.Message = "Taşınmaz updated";
                response.Success = true;
                response.Data = _mapper.Map<TasinmazDto>(existingTasinmaz);

                log.KullaniciId = tasinmazDto.KullaniciId;
                log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                log.Tarih = DateTime.Now;
                log.Durum = "Başarılı";
                log.Islem = "Güncelleme";
                log.Aciklama = $"Açıklama: \"{response.Message}\" - Veri: \"{JsonConvert.SerializeObject(response.Data)}\"";
                await _logService.AddAsync(log);
            }
            catch (Exception e)
            {
                response.Process = "Taşınmazlar";
                response.Message = "Error occured.";
                response.Success = false;
                response.Data = null;
                response.ErrorMessages = new List<string> { Convert.ToString(e.Message) };

                log.KullaniciId = tasinmazDto.KullaniciId;
                log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                log.Tarih = DateTime.Now;
                log.Durum = "Başarısız";
                log.Islem = "Güncelleme";
                log.Aciklama = $"Açıklama: \"Something went wrong in service layer when updating taşınmaz {tasinmazDto}.\" - Veri: \"No Data\" - Hata Mesajları: \"{JsonConvert.SerializeObject(response.ErrorMessages)}\"";
                await _logService.AddAsync(log);
            }
            return response;
        }

        public async Task<ServiceResponse<TasinmazDto>> DeleteTasinmazAsync(TasinmazDto tasinmazDto)
        {
            ServiceResponse<TasinmazDto> response = new ServiceResponse<TasinmazDto>();
            Log log = new Log();

            try
            {
                if (!await _tasinmazRepository.Exists(x => x.Id == tasinmazDto.Id))
                {
                    response.Process = "Taşınmazlar";
                    response.Message = "Not found.";
                    response.Success = false;
                    response.Data = null;

                    log.KullaniciId = tasinmazDto.KullaniciId;
                    log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                    log.Tarih = DateTime.Now;
                    log.Durum = "Başarısız";
                    log.Islem = "Silme";
                    log.Aciklama = $"Açıklama: \"{response.Message}.\" - Veri: \"No Data\"";
                    await _logService.AddAsync(log);
                    return response;
                }

                if (!await _tasinmazRepository.DeleteAsync(_mapper.Map<Tasinmaz>(tasinmazDto)))
                {
                    response.Process = "Taşınmazlar";
                    response.Message = "Repository error.";
                    response.Success = false;
                    response.Data = null;

                    log.KullaniciId = tasinmazDto.KullaniciId;
                    log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                    log.Tarih = DateTime.Now;
                    log.Durum = "Başarısız";
                    log.Islem = "Silme";
                    log.Aciklama = $"Açıklama: \"Something went wrong in repository layer when deleting taşınmaz {tasinmazDto}.\" - Veri: \"No Data\"";
                    await _logService.AddAsync(log);
                    return response;
                }
                response.Process = "Taşınmazlar";
                response.Success = true;
                response.Data = null;
                response.Message = "Taşınmaz deleted.";

                log.KullaniciId = tasinmazDto.KullaniciId;
                log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                log.Tarih = DateTime.Now;
                log.Durum = "Başarılı";
                log.Islem = "Silme";
                log.Aciklama = $"Açıklama: \"{response.Message}\" - Veri: \"{JsonConvert.SerializeObject(response.Data)}\"";
                await _logService.AddAsync(log);
            }
            catch (Exception e)
            {
                response.Process = "Taşınmazlar";
                response.Success = false;
                response.Data = null;
                response.Message = "Error occured.";
                response.ErrorMessages = new List<string> { Convert.ToString(e.Message) };

                log.KullaniciId = tasinmazDto.KullaniciId;
                log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                log.Tarih = DateTime.Now;
                log.Durum = "Başarısız";
                log.Islem = "Silme";
                log.Aciklama = $"Açıklama: \"Something went wrong in service layer when deleting taşınmaz {tasinmazDto}.\" - Veri: \"No Data\" - Hata Mesajları: \"{JsonConvert.SerializeObject(response.ErrorMessages)}\"";
                await _logService.AddAsync(log);
            }
            return response;
        }
    }
}