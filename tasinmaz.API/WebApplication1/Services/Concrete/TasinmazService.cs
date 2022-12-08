using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using tasinmaz.API.Data;
using tasinmaz.API.Dtos.Tasinmaz;
using tasinmaz.API.Entities.Concrete;
using tasinmaz.API.Models;
using tasinmaz.API.Services.Abstract;

namespace tasinmaz.API.Services.Concrete
{
    public class TasinmazService : ITasinmazService
    {
        IGenericRepository<TasinmazDto> _tasinmazRepository;
        IMapper _mapper;
        HttpContext _httpContext;

        public TasinmazService(IGenericRepository<TasinmazDto> tasinmazRepository, IMapper mapper, HttpContext httpContext)
        {
            _tasinmazRepository = tasinmazRepository;
            _mapper = mapper;
            _httpContext = httpContext;
        }


        public async Task<LogKaydi<List<TasinmazDto>>> GetTasinmazlarAsync(TasinmazDto tasinmazDto)
        {
            LogKaydi<List<TasinmazDto>> _logKaydi = new LogKaydi<List<TasinmazDto>>();
            try
            {
                var tasinmazList = await _tasinmazRepository.GetAllAsync(x => x.Adi.Contains(tasinmazDto.Adi) &&
                    x.IlAdi.Contains(tasinmazDto.IlAdi) && x.IlceAdi.Contains(tasinmazDto.IlceAdi) && x.Adi.Contains(tasinmazDto.IlceAdi) &&
                    x.MahalleAdi.Contains(tasinmazDto.MahalleAdi) && x.Ada.Contains(tasinmazDto.Ada) && x.Parsel.Contains(tasinmazDto.Parsel) &&
                    x.Nitelik.Contains(tasinmazDto.Nitelik) && x.KoordinatBilgileri.Contains(tasinmazDto.KoordinatBilgileri));
                if (tasinmazList == null)
                {
                    _logKaydi.Tarih = DateTime.Now;
                    _logKaydi.KullaniciIp = Convert.ToString(_httpContext.Connection.RemoteIpAddress);
                    _logKaydi.Islem = "Arama";
                    _logKaydi.Durum = "Başarısız";
                    _logKaydi.Veri = null;
                    _logKaydi.Aciklama = $"Açıklama: \"Taşınmaz does not exist.\" - Veri: \"{_logKaydi.Veri}\" - Hata Mesajları: \"{JsonConvert.SerializeObject(_logKaydi.HataMesajlari)}\"";
                    return _logKaydi;
                }

                _logKaydi.Tarih = DateTime.Now;
                _logKaydi.KullaniciIp = Convert.ToString(_httpContext.Connection.RemoteIpAddress);
                _logKaydi.Islem = "Arama";
                _logKaydi.Durum = "Başarılı";
                _logKaydi.Veri = tasinmazList.ToList();
                _logKaydi.Aciklama = $"Açıklama: \"Taşınmazlar searched.\" - Veri: \"{JsonConvert.SerializeObject(_logKaydi.Veri)}\" - Hata Mesajları: \"{JsonConvert.SerializeObject(_logKaydi.HataMesajlari)}\"";
            }
            catch (Exception e)
            {
                _logKaydi.Tarih = DateTime.Now;
                _logKaydi.KullaniciIp = Convert.ToString(_httpContext.Connection.RemoteIpAddress);
                _logKaydi.Islem = "Arama";
                _logKaydi.Durum = "Başarısız";
                _logKaydi.Veri = null;
                _logKaydi.HataMesajlari = new List<string> { Convert.ToString(e.Message) };
                _logKaydi.Aciklama = $"Açıklama: \"Error occured.\" - Veri: \"{JsonConvert.SerializeObject(_logKaydi.Veri)}\" - Hata Mesajları: \"{JsonConvert.SerializeObject(_logKaydi.HataMesajlari)}\"";
            }
            return _logKaydi;
        }

        public async Task<LogKaydi<TasinmazDto>> AddTasinmazAsync(TasinmazDto tasinmazDto)
        {
            LogKaydi<TasinmazDto> _logKaydi = new LogKaydi<TasinmazDto>();
            try
            {
                if (await _tasinmazRepository.AnyAsync(x => x.Adi == tasinmazDto.Adi))
                {
                    _logKaydi.Tarih = DateTime.Now;
                    _logKaydi.KullaniciIp = Convert.ToString(_httpContext.Connection.RemoteIpAddress);
                    _logKaydi.Islem = "Yeni Kayıt";
                    _logKaydi.Durum = "Başarısız";
                    _logKaydi.Veri = null;
                    _logKaydi.Aciklama = $"Açıklama: \"Taşınmaz exists.\" - Veri: \"{_logKaydi.Veri}\" - Hata Mesajları: \"{JsonConvert.SerializeObject(_logKaydi.HataMesajlari)}\"";
                    return _logKaydi;
                }

                if (!await _tasinmazRepository.AddAsync(tasinmazDto))
                {
                    _logKaydi.Tarih = DateTime.Now;
                    _logKaydi.KullaniciIp = Convert.ToString(_httpContext.Connection.RemoteIpAddress);
                    _logKaydi.Islem = "Yeni Kayıt";
                    _logKaydi.Durum = "Başarısız";
                    _logKaydi.Veri = null;
                    _logKaydi.Aciklama = $"Açıklama: \"Repository error.\" - Veri: \"{_logKaydi.Veri}\" - Hata Mesajları: \"{JsonConvert.SerializeObject(_logKaydi.HataMesajlari)}\"";
                    return _logKaydi;
                }
                _logKaydi.Tarih = DateTime.Now;
                _logKaydi.KullaniciIp = Convert.ToString(_httpContext.Connection.RemoteIpAddress);
                _logKaydi.Islem = "Yeni Kayıt";
                _logKaydi.Durum = "Başarılı";
                _logKaydi.Veri = tasinmazDto;
                _logKaydi.Aciklama = $"Açıklama: \"Taşınmaz created.\" - Veri: \"{JsonConvert.SerializeObject(_logKaydi.Veri)}\" - Hata Mesajları: \"{JsonConvert.SerializeObject(_logKaydi.HataMesajlari)}\"";
            }
            catch (Exception e)
            {
                _logKaydi.Tarih = DateTime.Now;
                _logKaydi.KullaniciIp = Convert.ToString(_httpContext.Connection.RemoteIpAddress);
                _logKaydi.Islem = "Yeni Kayıt";
                _logKaydi.Durum = "Başarısız";
                _logKaydi.Veri = null;
                _logKaydi.HataMesajlari = new List<string> {Convert.ToString(e.Message)};
                _logKaydi.Aciklama = $"Açıklama: \"Error occured.\" - Veri: \"{JsonConvert.SerializeObject(_logKaydi.Veri)}\" - Hata Mesajları: \"{JsonConvert.SerializeObject(_logKaydi.HataMesajlari)}\"";
            }
            return _logKaydi;
        }

        public async Task<LogKaydi<TasinmazDto>> UpdateTasinmazAsync(TasinmazDto tasinmazDto)
        {
            LogKaydi<TasinmazDto> _logKaydi = new LogKaydi<TasinmazDto>();
            try
            {
                if (await _tasinmazRepository.GetASync(x => x.Id == tasinmazDto.Id) == null)
                {
                    _logKaydi.Tarih = DateTime.Now;
                    _logKaydi.KullaniciIp = Convert.ToString(_httpContext.Connection.RemoteIpAddress);
                    _logKaydi.Islem = "Güncelleme";
                    _logKaydi.Durum = "Başarısız";
                    _logKaydi.Veri = null;
                    _logKaydi.Aciklama = $"Açıklama: \"Not Found.\" - Veri: \"{_logKaydi.Veri}\" - Hata Mesajları: \"{JsonConvert.SerializeObject(_logKaydi.HataMesajlari)}\"";
                    return _logKaydi;
                }

                if (!await _tasinmazRepository.UpdateAsync(tasinmazDto))
                {
                    _logKaydi.Tarih = DateTime.Now;
                    _logKaydi.KullaniciIp = Convert.ToString(_httpContext.Connection.RemoteIpAddress);
                    _logKaydi.Islem = "Güncelleme";
                    _logKaydi.Durum = "Başarısız";
                    _logKaydi.Veri = null;
                    _logKaydi.Aciklama = $"Açıklama: \"Repository error.\" - Veri: \"{_logKaydi.Veri}\" - Hata Mesajları: \"{JsonConvert.SerializeObject(_logKaydi.HataMesajlari)}\"";
                    return _logKaydi;
                }

                _logKaydi.Tarih = DateTime.Now;
                _logKaydi.KullaniciIp = Convert.ToString(_httpContext.Connection.RemoteIpAddress);
                _logKaydi.Islem = "Güncelleme";
                _logKaydi.Durum = "Başarılı";
                _logKaydi.Veri = tasinmazDto;
                _logKaydi.Aciklama = $"Açıklama: \"Taşınmaz updated.\" - Veri: \"{JsonConvert.SerializeObject(_logKaydi.Veri)}\" - Hata Mesajları: \"{JsonConvert.SerializeObject(_logKaydi.HataMesajlari)}\"";
            }
            catch (Exception e)
            {
                _logKaydi.Tarih = DateTime.Now;
                _logKaydi.KullaniciIp = Convert.ToString(_httpContext.Connection.RemoteIpAddress);
                _logKaydi.Islem = "Güncelleme";
                _logKaydi.Durum = "Başarısız";
                _logKaydi.Veri = null;
                _logKaydi.HataMesajlari = new List<string> { Convert.ToString(e.Message) };
                _logKaydi.Aciklama = $"Açıklama: \"Error occured.\" - Veri: \"{JsonConvert.SerializeObject(_logKaydi.Veri)}\" - Hata Mesajları: \"{JsonConvert.SerializeObject(_logKaydi.HataMesajlari)}\"";
            }
            return _logKaydi;
        }

        public async Task<LogKaydi<TasinmazDto>> DeleteTasinmazAsync(TasinmazDto tasinmazDto)
        {
            LogKaydi<TasinmazDto> _logKaydi = new LogKaydi<TasinmazDto>();
            try
            {
                if (!await _tasinmazRepository.AnyAsync(x => x.Id == tasinmazDto.Id))
                {
                    _logKaydi.Tarih = DateTime.Now;
                    _logKaydi.KullaniciIp = Convert.ToString(_httpContext.Connection.RemoteIpAddress);
                    _logKaydi.Islem = "Silme";
                    _logKaydi.Durum = "Başarısız";
                    _logKaydi.Aciklama = $"Açıklama: \"Not Found.\" - Veri: \"{_logKaydi.Veri}\" - Hata Mesajları: \"{JsonConvert.SerializeObject(_logKaydi.HataMesajlari)}\"";
                    return _logKaydi;
                }

                if (!await _tasinmazRepository.DeleteAsync(tasinmazDto))
                {
                    _logKaydi.Tarih = DateTime.Now;
                    _logKaydi.KullaniciIp = Convert.ToString(_httpContext.Connection.RemoteIpAddress);
                    _logKaydi.Islem = "Silme";
                    _logKaydi.Durum = "Başarısız";
                    _logKaydi.Aciklama = $"Açıklama: \"Repository error.\" - Veri: \"{_logKaydi.Veri}\" - Hata Mesajları: \"{JsonConvert.SerializeObject(_logKaydi.HataMesajlari)}\"";
                    return _logKaydi;
                }

                _logKaydi.Tarih = DateTime.Now;
                _logKaydi.KullaniciIp = Convert.ToString(_httpContext.Connection.RemoteIpAddress);
                _logKaydi.Islem = "Silme";
                _logKaydi.Durum = "Başarılı";
                _logKaydi.Aciklama = $"Açıklama: \"Taşınmaz updated.\" - Veri: \"{JsonConvert.SerializeObject(_logKaydi.Veri)}\" - Hata Mesajları: \"{JsonConvert.SerializeObject(_logKaydi.HataMesajlari)}\"";
            }
            catch (Exception e)
            {
                _logKaydi.Tarih = DateTime.Now;
                _logKaydi.KullaniciIp = Convert.ToString(_httpContext.Connection.RemoteIpAddress);
                _logKaydi.Islem = "Silme";
                _logKaydi.Durum = "Başarısız";
                _logKaydi.HataMesajlari = new List<string> { Convert.ToString(e.Message) };
                _logKaydi.Aciklama = $"Açıklama: \"Error occured.\" - Veri: \"{JsonConvert.SerializeObject(_logKaydi.Veri)}\" - Hata Mesajları: \"{JsonConvert.SerializeObject(_logKaydi.HataMesajlari)}\"";
            }
            return _logKaydi;
        }
    }
}