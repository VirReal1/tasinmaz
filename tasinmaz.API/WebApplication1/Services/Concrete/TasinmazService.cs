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

        public TasinmazService(ITasinmazRepository tasinmazRepository, IHttpContextAccessor httpAccessor, IMapper mapper, ILogService logService)
        {
            _tasinmazRepository = tasinmazRepository;
            _httpAccessor = httpAccessor;
            _mapper = mapper;
            _logService = logService;
        }

        public async Task<ServiceResponse<List<TasinmazDto>>> GetAllAsync()
        {
            ServiceResponse<List<TasinmazDto>> _response = new ServiceResponse<List<TasinmazDto>>();
            Log _log = new Log();

            var tasinmazlarAllList = await _tasinmazRepository.GetAllAsync();
            var tasinmazlarDtoAllList = new List<TasinmazDto>();

            foreach (var item in tasinmazlarAllList)
            {
                tasinmazlarDtoAllList.Add(_mapper.Map<TasinmazDto>(item));
            }

            try
            {
                _response.Message = "Got all Taşınmazlar.";
                _response.Success = true;
                _response.Data = tasinmazlarDtoAllList;

                _log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                _log.Tarih = DateTime.Now;
                _log.Durum = "Başarılı";
                _log.Islem = "Getirme";
                _log.Aciklama = $"Açıklama: \"{_response.Message}.\" - Veri: \"{JsonConvert.SerializeObject(_response.Data)}\"";
                await _logService.AddAsync(_log);
            }
            catch (Exception e)
            {
                _response.Message = "Error occured.";
                _response.Success = false;
                _response.Data = tasinmazlarDtoAllList;
                _response.ErrorMessages = new List<string> { Convert.ToString(e.Message) };

                _log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                _log.Tarih = DateTime.Now;
                _log.Durum = "Başarısız";
                _log.Islem = "Getirme";
                _log.Aciklama = $"Açıklama: \"{_response.Message}\" - Veri: \"No Data\" - Hata Mesajları: \"{JsonConvert.SerializeObject(_response.ErrorMessages)}\"";
                await _logService.AddAsync(_log);
            }
            return _response;
        }
        public async Task<ServiceResponse<List<TasinmazDto>>> GetTasinmazlarAsync(TasinmazDto tasinmazDto)
        {
            ServiceResponse<List<TasinmazDto>> _response = new ServiceResponse<List<TasinmazDto>>();
            Log _log = new Log();

            var tasinmazlarAllList = await _tasinmazRepository.GetAllAsync();
            var tasinmazlarDtoAllList = new List<TasinmazDto>();
            var tasinmazlarList = await _tasinmazRepository.GetAllAsync(x =>
                (x.Adi.Contains(tasinmazDto.Adi, StringComparison.InvariantCultureIgnoreCase) || tasinmazDto.Adi == null) &&
                (x.IlAdi.Contains(tasinmazDto.IlAdi, StringComparison.InvariantCultureIgnoreCase) || tasinmazDto.IlAdi == null) &&
                (x.IlceAdi.Contains(tasinmazDto.IlceAdi, StringComparison.InvariantCultureIgnoreCase) || tasinmazDto.IlceAdi == null) &&
                (x.MahalleAdi.Contains(tasinmazDto.MahalleAdi, StringComparison.InvariantCultureIgnoreCase) || tasinmazDto.MahalleAdi == null) &&
                (x.Ada.Contains(tasinmazDto.Ada, StringComparison.InvariantCultureIgnoreCase) || tasinmazDto.Ada == null) &&
                (x.Parsel.Contains(tasinmazDto.Parsel, StringComparison.InvariantCultureIgnoreCase) || tasinmazDto.Parsel == null) &&
                (x.Nitelik.Contains(tasinmazDto.Nitelik, StringComparison.InvariantCultureIgnoreCase) || tasinmazDto.Nitelik == null) &&
                (x.KoordinatBilgileri.Contains(tasinmazDto.KoordinatBilgileri, StringComparison.InvariantCultureIgnoreCase) || tasinmazDto.KoordinatBilgileri == null) &&
                (x.Kullanici.Id.ToString().Contains(tasinmazDto.KullaniciId.ToString(), StringComparison.CurrentCultureIgnoreCase) || tasinmazDto.KullaniciId == default));

            foreach (var item in tasinmazlarAllList)
            {
                tasinmazlarDtoAllList.Add(_mapper.Map<TasinmazDto>(item));
            }

            try
            {
                if (tasinmazDto == null)
                {
                    _response.Message = "No Parameter";
                    _response.Success = false;
                    _response.Data = tasinmazlarDtoAllList;

                    _log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                    _log.Tarih = DateTime.Now;
                    _log.Durum = "Başarısız";
                    _log.Islem = "Arama";
                    _log.Aciklama = $"Açıklama: \"{_response.Message}.\" - Veri: \"No Data\"";
                    await _logService.AddAsync(_log);
                    return _response;
                }

                if (tasinmazlarList == null)
                {
                    _response.Message = "Parameters does not match with the database.";
                    _response.Success = false;
                    _response.Data = tasinmazlarDtoAllList;

                    _log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                    _log.Tarih = DateTime.Now;
                    _log.Durum = "Başarısız";
                    _log.Islem = "Arama";
                    _log.Aciklama = $"Açıklama: \"{_response.Message}.\" - Veri: \"No Data\"";
                    await _logService.AddAsync(_log);
                    return _response;
                }
                _response.Message = "Taşınmazlar searched.";
                _response.Success = false;
                _response.Data = _mapper.Map<List<TasinmazDto>>(tasinmazlarList);

                _log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                _log.Tarih = DateTime.Now;
                _log.Durum = "Başarılı";
                _log.Islem = "Arama";
                _log.Aciklama = $"Açıklama: \"{_response.Message}.\" - Veri: \"{JsonConvert.SerializeObject(_response.Data)}\"";
                await _logService.AddAsync(_log);
            }
            catch (Exception e)
            {
                _response.Message = "Error occured.";
                _response.Success = false;
                _response.Data = tasinmazlarDtoAllList;
                _response.ErrorMessages = new List<string> { Convert.ToString(e.Message) };

                _log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                _log.Tarih = DateTime.Now;
                _log.Durum = "Başarısız";
                _log.Islem = "Arama";
                _log.Aciklama = $"Açıklama: \"{_response.Message}\" - Veri: \"No Data\" - Hata Mesajları: \"{JsonConvert.SerializeObject(_response.ErrorMessages)}\"";
                await _logService.AddAsync(_log);
            }
            return _response;
        }

        public async Task<ServiceResponse<TasinmazDto>> AddTasinmazAsync(TasinmazDto tasinmazDto)
        {
            ServiceResponse<TasinmazDto> _response = new ServiceResponse<TasinmazDto>();
            Log _log = new Log();

            try
            {
                if (await _tasinmazRepository.Exists(x => x.Adi == tasinmazDto.Adi))
                {
                    _response.Message = "Taşınmaz exists.";
                    _response.Success = false;
                    _response.Data = null;

                    _log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                    _log.Tarih = DateTime.Now;
                    _log.Durum = "Başarısız";
                    _log.Islem = "Yeni Kayıt";
                    _log.Aciklama = $"Açıklama: \"{_response.Message}.\" - Veri: \"No Data\"";
                    await _logService.AddAsync(_log);
                    return _response;
                }

                var createdTasinmaz = _mapper.Map<Tasinmaz>(tasinmazDto);

                if (!await _tasinmazRepository.AddAsync(createdTasinmaz))
                {
                    _response.Error = "Repository error.";
                    _response.Success = false;
                    _response.Data = null;

                    _log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                    _log.Tarih = DateTime.Now;
                    _log.Durum = "Başarısız";
                    _log.Islem = "Yeni Kayıt";
                    _log.Aciklama = $"Açıklama: \"Something went wrong in respository layer when adding taşınmaz {tasinmazDto}.\" - Veri: \"No Data\"";
                    await _logService.AddAsync(_log);
                    return _response;
                }
                _response.Success = true;
                _response.Data = _mapper.Map<TasinmazDto>(createdTasinmaz);
                _response.Message = "Tasinmaz created";

                _log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                _log.Tarih = DateTime.Now;
                _log.Durum = "Başarılı";
                _log.Islem = "Yeni Kayıt";
                _log.Aciklama = $"Açıklama: \"{_response.Message}\" - Veri: \"{JsonConvert.SerializeObject(_response.Data)}\"";
                await _logService.AddAsync(_log);
            }
            catch (Exception e)
            {
                _response.Success = false;
                _response.Data = null;
                _response.Error = "Error occured.";
                _response.ErrorMessages = new List<string> { Convert.ToString(e.Message) };

                _log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                _log.Tarih = DateTime.Now;
                _log.Durum = "Başarısız";
                _log.Islem = "Yeni Kayıt";
                _log.Aciklama = $"Açıklama: \"Something went wrong in service layer when adding taşınmaz {tasinmazDto}.\" - Veri: \"No Data\" - Hata Mesajları: \"{JsonConvert.SerializeObject(_response.ErrorMessages)}\"";
                await _logService.AddAsync(_log);
            }
            return _response;
        }

        public async Task<ServiceResponse<TasinmazDto>> UpdateTasinmazAsync(TasinmazDto tasinmazDto)
        {
            ServiceResponse<TasinmazDto> _response = new ServiceResponse<TasinmazDto>();
            Log _log = new Log();

            try
            {
                var existingTasinmaz = await _tasinmazRepository.Get(x => x.Id == tasinmazDto.Id);

                if (existingTasinmaz == null)
                {
                    _response.Message = "Not found.";
                    _response.Success = false;
                    _response.Data = null;

                    _log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                    _log.Tarih = DateTime.Now;
                    _log.Durum = "Başarısız";
                    _log.Islem = "Güncelleme";
                    _log.Aciklama = $"Açıklama: \"{_response.Message}.\" - Veri: \"No Data\"";
                    await _logService.AddAsync(_log);
                    return _response;
                }

                existingTasinmaz = _mapper.Map<Tasinmaz>(tasinmazDto);

                if (!await _tasinmazRepository.UpdateAsync(existingTasinmaz))
                {
                    _response.Error = "Repository error.";
                    _response.Success = false;
                    _response.Data = null;

                    _log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                    _log.Tarih = DateTime.Now;
                    _log.Durum = "Başarısız";
                    _log.Islem = "Güncelleme";
                    _log.Aciklama = $"Açıklama: \"Something went wrong in respository layer when adding taşınmaz {tasinmazDto}.\" - Veri: \"No Data\"";
                    await _logService.AddAsync(_log);
                    return _response;
                }
                _response.Success = true;
                _response.Data = _mapper.Map<TasinmazDto>(existingTasinmaz);
                _response.Message = "Taşınmaz updated";

                _log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                _log.Tarih = DateTime.Now;
                _log.Durum = "Başarılı";
                _log.Islem = "Güncelleme";
                _log.Aciklama = $"Açıklama: \"{_response.Message}\" - Veri: \"{JsonConvert.SerializeObject(_response.Data)}\"";
                await _logService.AddAsync(_log);
            }
            catch (Exception e)
            {
                _response.Success = false;
                _response.Data = null;
                _response.Error = "Error occured.";
                _response.ErrorMessages = new List<string> { Convert.ToString(e.Message) };

                _log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                _log.Tarih = DateTime.Now;
                _log.Durum = "Başarısız";
                _log.Islem = "Güncelleme";
                _log.Aciklama = $"Açıklama: \"Something went wrong in service layer when adding taşınmaz {tasinmazDto}.\" - Veri: \"No Data\" - Hata Mesajları: \"{JsonConvert.SerializeObject(_response.ErrorMessages)}\"";
                await _logService.AddAsync(_log);
            }
            return _response;
        }

        public async Task<ServiceResponse<TasinmazDto>> DeleteTasinmazAsync(TasinmazDto tasinmazDto)
        {
            ServiceResponse<TasinmazDto> _response = new ServiceResponse<TasinmazDto>();
            Log _log = new Log();

            try
            {
                if (!await _tasinmazRepository.Exists(x => x.Id == tasinmazDto.Id))
                {
                    _response.Message = "Not found.";
                    _response.Success = false;
                    _response.Data = null;

                    _log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                    _log.Tarih = DateTime.Now;
                    _log.Durum = "Başarısız";
                    _log.Islem = "Silme";
                    _log.Aciklama = $"Açıklama: \"{_response.Message}.\" - Veri: \"No Data\"";
                    await _logService.AddAsync(_log);
                    return _response;
                }

                if (!await _tasinmazRepository.DeleteAsync(_mapper.Map<Tasinmaz>(tasinmazDto)))
                {
                    _response.Error = "Repository error.";
                    _response.Success = false;
                    _response.Data = null;

                    _log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                    _log.Tarih = DateTime.Now;
                    _log.Durum = "Başarısız";
                    _log.Islem = "Silme";
                    _log.Aciklama = $"Açıklama: \"Something went wrong in respository layer when adding taşınmaz {tasinmazDto}.\" - Veri: \"No Data\"";
                    await _logService.AddAsync(_log);
                    return _response;
                }
                _response.Success = true;
                _response.Data = null;
                _response.Message = "Taşınmaz deleted.";

                _log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                _log.Tarih = DateTime.Now;
                _log.Durum = "Başarılı";
                _log.Islem = "Silme";
                _log.Aciklama = $"Açıklama: \"{_response.Message}\" - Veri: \"{JsonConvert.SerializeObject(_response.Data)}\"";
                await _logService.AddAsync(_log);
            }
            catch (Exception e)
            {
                _response.Success = false;
                _response.Data = null;
                _response.Error = "Error occured.";
                _response.ErrorMessages = new List<string> { Convert.ToString(e.Message) };

                _log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                _log.Tarih = DateTime.Now;
                _log.Durum = "Başarısız";
                _log.Islem = "Silme";
                _log.Aciklama = $"Açıklama: \"Something went wrong in service layer when adding taşınmaz {tasinmazDto}.\" - Veri: \"No Data\" - Hata Mesajları: \"{JsonConvert.SerializeObject(_response.ErrorMessages)}\"";
                await _logService.AddAsync(_log);
            }
            return _response;
        }
    }
}