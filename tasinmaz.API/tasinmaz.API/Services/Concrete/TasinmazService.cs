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
        IUserRepository _userRepository;

        public TasinmazService(ITasinmazRepository tasinmazRepository, IHttpContextAccessor httpAccessor, IMapper mapper, ILogService logService, IUserRepository userRepository)
        {
            _tasinmazRepository = tasinmazRepository;
            _httpAccessor = httpAccessor;
            _mapper = mapper;
            _logService = logService;
            _userRepository = userRepository;
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
                (tasinmazDto.Id == default || x.Id.ToString().ToLower().Contains(tasinmazDto.Id.ToString().ToLower())) &&
                (tasinmazDto.Adi == null || x.Adi.ToLower().Contains(tasinmazDto.Adi.ToLower())) &&
                (tasinmazDto.IlAdi == null || x.IlAdi.ToLower().Contains(tasinmazDto.IlAdi.ToLower())) &&
                (tasinmazDto.IlceAdi == null || x.IlceAdi.ToLower().Contains(tasinmazDto.IlceAdi.ToLower())) &&
                (tasinmazDto.MahalleAdi == null || x.MahalleAdi.ToLower().Contains(tasinmazDto.MahalleAdi.ToLower())) &&
                (tasinmazDto.Ada == null || x.Ada.ToLower().Contains(tasinmazDto.Ada.ToLower())) &&
                (tasinmazDto.Parsel == null || x.Parsel.ToLower().Contains(tasinmazDto.Parsel.ToLower())) &&
                (tasinmazDto.Nitelik == null || x.Nitelik.ToLower().Contains(tasinmazDto.Nitelik.ToLower())) &&
                (tasinmazDto.KoordinatBilgileri == null || x.KoordinatBilgileri.ToLower().Contains(tasinmazDto.KoordinatBilgileri.ToLower())) &&
                (tasinmazDto.KullaniciId == default || x.Kullanici.Id.ToString().ToLower().Contains(tasinmazDto.KullaniciId.ToString().ToLower())));

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
                var tasinmazExists = await _tasinmazRepository.Exists(x => x.Adi == tasinmazDto.Adi);
                if (tasinmazExists)
                {
                    _response.Message = "Taşınmaz exists.";
                    _response.Success = false;
                    _response.Data = null;

                    _log.Kullanici = await _userRepository.Get(tasinmazDto.KullaniciId);
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

                    _log.Kullanici = await _userRepository.Get(tasinmazDto.KullaniciId);
                    _log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                    _log.Tarih = DateTime.Now;
                    _log.Durum = "Başarısız";
                    _log.Islem = "Yeni Kayıt";
                    _log.Aciklama = $"Açıklama: \"Something went wrong in repository layer when adding taşınmaz {tasinmazDto}.\" - Veri: \"No Data\"";
                    await _logService.AddAsync(_log);
                    return _response;
                }
                _response.Success = true;
                _response.Data = _mapper.Map<TasinmazDto>(createdTasinmaz);
                _response.Message = "Tasinmaz created";

                _log.KullaniciId = tasinmazDto.KullaniciId;
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

                _log.Kullanici = await _userRepository.Get(tasinmazDto.KullaniciId);
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
                    _log.Aciklama = $"Açıklama: \"Something went wrong in repository layer when adding taşınmaz {tasinmazDto}.\" - Veri: \"No Data\"";
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
                    _log.Aciklama = $"Açıklama: \"Something went wrong in repository layer when adding taşınmaz {tasinmazDto}.\" - Veri: \"No Data\"";
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