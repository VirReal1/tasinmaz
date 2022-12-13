﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using tasinmaz.API.Data;
using tasinmaz.API.Dtos;
using tasinmaz.API.Dtos.Tasinmaz;
using tasinmaz.API.Entities.Concrete;
using tasinmaz.API.Models;
using tasinmaz.API.Models.Concrete;
using tasinmaz.API.ServiceResponder;
using tasinmaz.API.Services.Abstract;

namespace tasinmaz.API.Services.Concrete
{
    public class UserService : IUserService
    {
        IUserRepository _userRepository;
        IHttpContextAccessor _httpAccessor;
        IMapper _mapper;
        ILogService _logService;

        public UserService(IUserRepository userRepository, IHttpContextAccessor httpAccessor, IMapper mapper, ILogService logService)
        {
            _userRepository = userRepository;
            _httpAccessor = httpAccessor;
            _mapper = mapper;
            _logService = logService;
        }
        public async Task<ServiceResponse<List<KullaniciForShowDeleteDto>>> GetAllAsync()
        {
            ServiceResponse<List<KullaniciForShowDeleteDto>> response = new ServiceResponse<List<KullaniciForShowDeleteDto>>();
            Log log = new Log();

            try
            {
                var usersAllList = await _userRepository.GetAllAsync();
                var usersDtoAllList = new List<KullaniciForShowDeleteDto>();

                foreach (var item in usersAllList)
                {
                    usersDtoAllList.Add(_mapper.Map<KullaniciForShowDeleteDto>(item));
                }

                response.Process = "Kullanıcılar";
                response.Message = "Bütün kullanıcılar getirildi.";
                response.Data = usersDtoAllList;

                log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                log.Tarih = DateTime.Now;
                log.Durum = "Başarılı";
                log.Islem = "Getirme";
                log.Aciklama = $"Açıklama: \"{response.Message}.\" - Veri: \"{JsonConvert.SerializeObject(response.Data)}\"";
                await _logService.AddAsync(log);
            }
            catch (Exception e)
            {
                response.Process = "Kullanıcılar";
                response.Message = "Taşınmazları getirirken servis katmanında bir hata oluştu.";
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

        public async Task<ServiceResponse<List<KullaniciForShowDeleteDto>>> GetUsersAsync(KullaniciForShowDeleteDto kullaniciForShowDto)
        {
            ServiceResponse<List<KullaniciForShowDeleteDto>> response = new ServiceResponse<List<KullaniciForShowDeleteDto>>();
            Log log = new Log();

            try
            {
                var usersAllList = await _userRepository.GetAllAsync();
                var usersDtoAllList = new List<KullaniciForShowDeleteDto>();
                var usersList = await _userRepository.GetAllAsync(x =>
                    (kullaniciForShowDto.Id == default || x.Id.ToString().ToLower().Contains(kullaniciForShowDto.Id.ToString().ToLower())) &&
                    (kullaniciForShowDto.Ad == null || x.Ad.ToLower().Contains(kullaniciForShowDto.Ad.ToLower())) &&
                    (kullaniciForShowDto.Soyad == null || x.Soyad.ToLower().Contains(kullaniciForShowDto.Soyad.ToLower())) &&
                    (kullaniciForShowDto.Email == null || x.Email.ToLower().Contains(kullaniciForShowDto.Email.ToLower())) &&
                    (x.AdminMi == kullaniciForShowDto.AdminMi));

                foreach (var item in usersAllList)
                {
                    usersDtoAllList.Add(_mapper.Map<KullaniciForShowDeleteDto>(item));
                }

                if (usersList == null)
                {
                    response.Process = "Kullanıcılar";
                    response.Message = "Arama parametreleri veri tabanıyla eşleşmedi.";
                    response.Warning = true;
                    response.Data = usersDtoAllList;

                    log.KullaniciId = kullaniciForShowDto.Id;
                    log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                    log.Tarih = DateTime.Now;
                    log.Durum = "Başarısız";
                    log.Islem = "Arama";
                    log.Aciklama = $"Açıklama: \"{response.Message}.\" - Veri: \"Veri yok.\"";
                    await _logService.AddAsync(log);
                    return response;
                }
                response.Process = "Kullanıcılar";
                response.Message = "Kullanıcılar başarıyla filtrelendi.";
                response.Data = _mapper.Map<List<KullaniciForShowDeleteDto>>(usersList);

                log.KullaniciId = kullaniciForShowDto.Id;
                log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                log.Tarih = DateTime.Now;
                log.Durum = "Başarılı";
                log.Islem = "Arama";
                log.Aciklama = $"Açıklama: \"{response.Message}.\" - Veri: \"{JsonConvert.SerializeObject(response.Data)}\"";
                await _logService.AddAsync(log);
            }
            catch (Exception e)
            {
                response.Process = "Kullanıcılar";
                response.Message = "Taşınmazları filtrelerken servis katmanında bir hata oluştu.";
                response.Error = true;
                response.Data = null;
                response.ErrorMessages = new List<string> { Convert.ToString(e.Message) };

                log.KullaniciId = kullaniciForShowDto.Id;
                log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                log.Tarih = DateTime.Now;
                log.Durum = "Başarısız";
                log.Islem = "Arama";
                log.Aciklama = $"Açıklama: \"{response.Message}\" - Veri: \"Veri yok.\" - Hata Mesajları: \"{JsonConvert.SerializeObject(response.ErrorMessages)}\"";
                await _logService.AddAsync(log);
            }
            return response;
        }

        public async Task<ServiceResponse<KullaniciToken>> LoginUserAsync(KullaniciForLoginDto kullaniciForLoginDto)
        {
            ServiceResponse<KullaniciToken> response = new ServiceResponse<KullaniciToken>();
            Log log = new Log();

            try
            {
                var kullaniciToken = await _userRepository.LoginUserAsync(kullaniciForLoginDto);

                if (kullaniciToken == null)
                {
                    response.Process = "Kullanıcılar";
                    response.Message = "E-Mail ya da şifre hatalı.";
                    response.Warning = true;
                    response.Data = null;

                    log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                    log.Tarih = DateTime.Now;
                    log.Durum = "Başarısız";
                    log.Islem = "Giriş";
                    log.Aciklama = $"Açıklama: \"{response.Message}.\" - Veri: \"Veri yok.\"";
                    await _logService.AddAsync(log);
                    return response;
                }
                response.Process = "Kullanıcılar";
                response.Message = "Başarıyla giriş yapıldı.";
                response.Data = kullaniciToken;

                log.KullaniciId = kullaniciToken.Id;
                log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                log.Tarih = DateTime.Now;
                log.Durum = "Başarılı";
                log.Islem = "Giriş";
                log.Aciklama = $"Açıklama: \"{response.Message}\" - Veri: \"{JsonConvert.SerializeObject(response.Data)}\"";
                await _logService.AddAsync(log);
            }
            catch (Exception e)
            {
                response.Process = "Kullanıcılar";
                response.Message = "Kullanıcı servis katmanında giriş yapılırken bir hata oluştu.";
                response.Error = true;
                response.Data = null;
                response.ErrorMessages = new List<string> { Convert.ToString(e.Message) };

                log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                log.Tarih = DateTime.Now;
                log.Durum = "Başarısız";
                log.Islem = "Giriş";
                log.Aciklama = $"Açıklama: \"{response.Message}\" - Veri: \"Veri yok.\" - Hata Mesajları: \"{JsonConvert.SerializeObject(response.ErrorMessages)}\"";
                await _logService.AddAsync(log);
            }
            return response;
        }

        public async Task<ServiceResponse<KullaniciForShowDeleteDto>> AddUserAsync(KullaniciForAddUpdateDto kullaniciForAddDto)
        {
            ServiceResponse<KullaniciForShowDeleteDto> response = new ServiceResponse<KullaniciForShowDeleteDto>();
            Log log = new Log();

            try
            {
                var userExists = await _userRepository.Exists(x => x.Email == kullaniciForAddDto.Email);
                if (userExists)
                {
                    response.Process = "Kullanıcılar";
                    response.Message = "Kullanıcı veri tabanında mevcut.";
                    response.Warning = true;
                    response.Data = null;

                    log.KullaniciId = kullaniciForAddDto.Id;
                    log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                    log.Tarih = DateTime.Now;
                    log.Durum = "Başarısız";
                    log.Islem = "Yeni Kayıt";
                    log.Aciklama = $"Açıklama: \"{response.Message}.\" - Veri: \"Veri yok.\"";
                    await _logService.AddAsync(log);
                    return response;
                }

                if (!await _userRepository.AddAsync(kullaniciForAddDto))
                {
                    response.Process = "Kullanıcılar";
                    response.Message = "Kullanıcı veri tabanına eklenirken bir hata oluştu.";
                    response.Error = true;
                    response.Data = null;

                    log.KullaniciId = kullaniciForAddDto.Id;
                    log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                    log.Tarih = DateTime.Now;
                    log.Durum = "Başarısız";
                    log.Islem = "Yeni Kayıt";
                    log.Aciklama = $"Açıklama: \"Kullanıcı: {kullaniciForAddDto.Email} veri tabanına eklenirken bir hata oluştu.\" - Veri: \"Veri yok.\"";
                    await _logService.AddAsync(log);
                    return response;
                }
                response.Process = "Kullanıcılar";
                response.Message = "Kullanıcı başarıyla eklendi.";
                response.Data = _mapper.Map<KullaniciForShowDeleteDto>(kullaniciForAddDto);

                log.KullaniciId = kullaniciForAddDto.Id;
                log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                log.Tarih = DateTime.Now;
                log.Durum = "Başarılı";
                log.Islem = "Yeni Kayıt";
                log.Aciklama = $"Açıklama: \"{response.Message}\" - Veri: \"{JsonConvert.SerializeObject(response.Data)}\"";
                await _logService.AddAsync(log);
            }
            catch (Exception e)
            {
                response.Process = "Kullanıcılar";
                response.Message = "Kullanıcı servis katmanında eklenirken bir hata oluştu.";
                response.Error = true;
                response.Data = null;
                response.ErrorMessages = new List<string> { Convert.ToString(e.Message) };

                log.KullaniciId = kullaniciForAddDto.Id;
                log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                log.Tarih = DateTime.Now;
                log.Durum = "Başarısız";
                log.Islem = "Yeni Kayıt";
                log.Aciklama = $"Açıklama: \"Kullanıcı: \"{kullaniciForAddDto.Email}\" servis katmanında eklenirken bir hata oluştu.\" - Veri: \"Veri yok.\" - Hata Mesajları: \"{JsonConvert.SerializeObject(response.ErrorMessages)}\"";
                await _logService.AddAsync(log);
            }
            return response;
        }
        public async Task<ServiceResponse<KullaniciForShowDeleteDto>> UpdateUserAsync(KullaniciForAddUpdateDto kullaniciForUpdateDto)
        {
            ServiceResponse<KullaniciForShowDeleteDto> response = new ServiceResponse<KullaniciForShowDeleteDto>();
            Log log = new Log();

            try
            {
                var existingUser = await _userRepository.Get(x => x.Id == kullaniciForUpdateDto.Id);

                if (existingUser == null)
                {
                    response.Process = "Kullanıcılar";
                    response.Message = "Kullanıcı veri tabanında bulunamadı.";
                    response.Warning = true;
                    response.Data = null;

                    log.KullaniciId = kullaniciForUpdateDto.Id;
                    log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                    log.Tarih = DateTime.Now;
                    log.Durum = "Başarısız";
                    log.Islem = "Güncelleme";
                    log.Aciklama = $"Açıklama: \"{response.Message}.\" - Veri: \"Veri yok.\"";
                    await _logService.AddAsync(log);
                    return response;
                }

                if (!await _userRepository.UpdateAsync(kullaniciForUpdateDto))
                {
                    response.Process = "Kullanıcılar";
                    response.Message = "Kullanıcı veri tabanına güncellenirken bir hata oluştu.";
                    response.Error = true;
                    response.Data = null;

                    log.KullaniciId = kullaniciForUpdateDto.Id;
                    log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                    log.Tarih = DateTime.Now;
                    log.Durum = "Başarısız";
                    log.Islem = "Güncelleme";
                    log.Aciklama = $"Açıklama: \"Kullanıcı: \"{kullaniciForUpdateDto.Email}\" veri tabanına güncellenirken bir hata oluştu.\" - Veri: \"Veri yok.\"";
                    await _logService.AddAsync(log);
                    return response;
                }
                response.Process = "Kullanıcılar";
                response.Message = "Taşınmaz başarıyla güncellendi.";
                response.Data = _mapper.Map<KullaniciForShowDeleteDto>(kullaniciForUpdateDto);

                log.KullaniciId = kullaniciForUpdateDto.Id;
                log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                log.Tarih = DateTime.Now;
                log.Durum = "Başarılı";
                log.Islem = "Güncelleme";
                log.Aciklama = $"Açıklama: \"{response.Message}\" - Veri: \"{JsonConvert.SerializeObject(response.Data)}\"";
                await _logService.AddAsync(log);
            }
            catch (Exception e)
            {
                response.Process = "Kullanıcılar";
                response.Message = "Kullanıcı servis katmanında güncellenirken bir hata oluştu.";
                response.Error = true;
                response.Data = null;
                response.ErrorMessages = new List<string> { Convert.ToString(e.Message) };

                log.KullaniciId = kullaniciForUpdateDto.Id;
                log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                log.Tarih = DateTime.Now;
                log.Durum = "Başarısız";
                log.Islem = "Güncelleme";
                log.Aciklama = $"Açıklama: \"Kullanıcı: \"{kullaniciForUpdateDto.Email}\" servis katmanında güncellenirken bir hata oluştu.\" - Veri: \"Veri yok.\" - Hata Mesajları: \"{JsonConvert.SerializeObject(response.ErrorMessages)}\"";
                await _logService.AddAsync(log);
            }
            return response;
        }

        public async Task<ServiceResponse<KullaniciForShowDeleteDto>> DeleteUserAsync(KullaniciForShowDeleteDto kullaniciForDeleteDto)
        {
            ServiceResponse<KullaniciForShowDeleteDto> response = new ServiceResponse<KullaniciForShowDeleteDto>();
            Log log = new Log();

            try
            {
                if (!await _userRepository.Exists(x => x.Id == kullaniciForDeleteDto.Id))
                {
                    response.Process = "Kullanıcılar";
                    response.Message = "Kullanıcı veri tabanında bulunamadı.";
                    response.Warning = true;
                    response.Data = null;

                    log.KullaniciId = kullaniciForDeleteDto.Id;
                    log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                    log.Tarih = DateTime.Now;
                    log.Durum = "Başarısız";
                    log.Islem = "Silme";
                    log.Aciklama = $"Açıklama: \"{response.Message}.\" - Veri: \"Veri yok.\"";
                    await _logService.AddAsync(log);
                    return response;
                }

                if (!await _userRepository.DeleteAsync(_mapper.Map<Kullanici>(kullaniciForDeleteDto)))
                {
                    response.Process = "Kullanıcılar";
                    response.Message = "Taşınmaz veri tabanından silinirken bir hata oluştu.";
                    response.Error = true;
                    response.Data = null;

                    log.KullaniciId = kullaniciForDeleteDto.Id;
                    log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                    log.Tarih = DateTime.Now;
                    log.Durum = "Başarısız";
                    log.Islem = "Silme";
                    log.Aciklama = $"Açıklama: \"Kullanıcı: \"{kullaniciForDeleteDto.Email}\" veri tabanından silinirken bir hata oluştu.\" - Veri: \"Veri yok.\"";
                    await _logService.AddAsync(log);
                    return response;
                }
                response.Process = "Kullanıcılar";
                response.Message = "Kullanıcı başarıyla silindi.";
                response.Data = null;

                log.KullaniciId = kullaniciForDeleteDto.Id;
                log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                log.Tarih = DateTime.Now;
                log.Durum = "Başarılı";
                log.Islem = "Silme";
                log.Aciklama = $"Açıklama: \"{response.Message}\" - Veri: \"{JsonConvert.SerializeObject(response.Data)}\"";
                await _logService.AddAsync(log);
            }
            catch (Exception e)
            {
                response.Process = "Kullanıcılar";
                response.Message = "Kullanıcı servis katmanından silinirken bir hata oluştu.";
                response.Error = true;
                response.Data = null;
                response.ErrorMessages = new List<string> { Convert.ToString(e.Message) };

                log.KullaniciId = kullaniciForDeleteDto.Id;
                log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                log.Tarih = DateTime.Now;
                log.Durum = "Başarısız";
                log.Islem = "Silme";
                log.Aciklama = $"Açıklama: \"Kullanıcı: \"{kullaniciForDeleteDto.Email}\" servis katmanından silinirken bir hata oluştu.\" - Veri: \"Veri yok.\" - Hata Mesajları: \"{JsonConvert.SerializeObject(response.ErrorMessages)}\"";
                await _logService.AddAsync(log);
            }
            return response;
        }
    }
}