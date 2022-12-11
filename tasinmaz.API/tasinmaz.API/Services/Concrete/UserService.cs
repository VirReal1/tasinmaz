using AutoMapper;
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

            var usersAllList = await _userRepository.GetAllAsync();
            var usersDtoAllList = new List<KullaniciForShowDeleteDto>();

            foreach (var item in usersAllList)
            {
                usersDtoAllList.Add(_mapper.Map<KullaniciForShowDeleteDto>(item));
            }

            try
            {
                response.Process = "Users";
                response.Message = "Got all users.";
                response.Success = true;
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
                response.Process = "Users";
                response.Message = "Error occured.";
                response.Success = false;
                response.Data = usersDtoAllList;
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

        public async Task<ServiceResponse<List<KullaniciForShowDeleteDto>>> GetUsersAsync(KullaniciForShowDeleteDto kullaniciForShowDeleteDto)
        {
            ServiceResponse<List<KullaniciForShowDeleteDto>> response = new ServiceResponse<List<KullaniciForShowDeleteDto>>();
            Log log = new Log();

            var usersAllList = await _userRepository.GetAllAsync();
            var usersDtoAllList = new List<KullaniciForShowDeleteDto>();
            var usersList = await _userRepository.GetAllAsync(x =>
                (kullaniciForShowDeleteDto.Id == default || x.Id.ToString().ToLower().Contains(kullaniciForShowDeleteDto.Id.ToString().ToLower())) &&
                (kullaniciForShowDeleteDto.Ad == null || x.Ad.ToLower().Contains(kullaniciForShowDeleteDto.Ad.ToLower())) &&
                (kullaniciForShowDeleteDto.Soyad == null || x.Soyad.ToLower().Contains(kullaniciForShowDeleteDto.Soyad.ToLower())) &&
                (kullaniciForShowDeleteDto.Email == null || x.Email.ToLower().Contains(kullaniciForShowDeleteDto.Email.ToLower())) &&
                (x.AdminMi==kullaniciForShowDeleteDto.AdminMi));

            foreach (var item in usersAllList)
            {
                usersDtoAllList.Add(_mapper.Map<KullaniciForShowDeleteDto>(item));
            }

            try
            {
                if (kullaniciForShowDeleteDto == null)
                {
                    response.Process = "Users";
                    response.Message = "No Parameter";
                    response.Success = false;
                    response.Data = usersDtoAllList;

                    log.KullaniciId = kullaniciForShowDeleteDto.Id;
                    log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                    log.Tarih = DateTime.Now;
                    log.Durum = "Başarısız";
                    log.Islem = "Arama";
                    log.Aciklama = $"Açıklama: \"{response.Message}.\" - Veri: \"No Data\"";
                    await _logService.AddAsync(log);
                    return response;
                }

                if (usersList == null)
                {
                    response.Process = "Users";
                    response.Message = "Parameters does not match with the database.";
                    response.Success = false;
                    response.Data = usersDtoAllList;

                    log.KullaniciId = kullaniciForShowDeleteDto.Id;
                    log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                    log.Tarih = DateTime.Now;
                    log.Durum = "Başarısız";
                    log.Islem = "Arama";
                    log.Aciklama = $"Açıklama: \"{response.Message}.\" - Veri: \"No Data\"";
                    await _logService.AddAsync(log);
                    return response;
                }
                response.Process = "Users";
                response.Message = "Users searched.";
                response.Success = false;
                response.Data = _mapper.Map<List<KullaniciForShowDeleteDto>>(usersList);

                log.KullaniciId = kullaniciForShowDeleteDto.Id;
                log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                log.Tarih = DateTime.Now;
                log.Durum = "Başarılı";
                log.Islem = "Arama";
                log.Aciklama = $"Açıklama: \"{response.Message}.\" - Veri: \"{JsonConvert.SerializeObject(response.Data)}\"";
                await _logService.AddAsync(log);
            }
            catch (Exception e)
            {
                response.Process = "Users";
                response.Message = "Error occured.";
                response.Success = false;
                response.Data = usersDtoAllList;
                response.ErrorMessages = new List<string> { Convert.ToString(e.Message) };

                log.KullaniciId = kullaniciForShowDeleteDto.Id;
                log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                log.Tarih = DateTime.Now;
                log.Durum = "Başarısız";
                log.Islem = "Arama";
                log.Aciklama = $"Açıklama: \"{response.Message}\" - Veri: \"No Data\" - Hata Mesajları: \"{JsonConvert.SerializeObject(response.ErrorMessages)}\"";
                await _logService.AddAsync(log);
            }
            return response;
        }

        public async Task<ServiceResponse<KullaniciToken>> LoginUserAsync(KullaniciForLoginDto kullaniciForLoginDto)
        {
            ServiceResponse<KullaniciToken> response = new ServiceResponse<KullaniciToken>();
            Log log = new Log();

            var kullaniciToken = await _userRepository.LoginUserAsync(kullaniciForLoginDto);
            try
            {
                if (kullaniciToken == null)
                {
                    response.Process = "Users";
                    response.Message = "E-Mail or password is wrong.";
                    response.Success = false;
                    response.Data = null;

                    log.KullaniciId = kullaniciToken.Id;
                    log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                    log.Tarih = DateTime.Now;
                    log.Durum = "Başarısız";
                    log.Islem = "Giriş";
                    log.Aciklama = $"Açıklama: \"{response.Message}.\" - Veri: \"No Data\"";
                    await _logService.AddAsync(log);
                    return response;
                }
                response.Process = "Users";
                response.Message = "User logged in.";
                response.Success = true;
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
                response.Process = "Users";
                response.Message = "Error occured.";
                response.Success = false;
                response.Data = null;
                response.ErrorMessages = new List<string> { Convert.ToString(e.Message) };

                log.KullaniciId = kullaniciToken.Id;
                log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                log.Tarih = DateTime.Now;
                log.Durum = "Başarısız";
                log.Islem = "Giriş";
                log.Aciklama = $"Açıklama: \"Something went wrong in service layer when logging user {kullaniciForLoginDto}.\" - Veri: \"No Data\" - Hata Mesajları: \"{JsonConvert.SerializeObject(response.ErrorMessages)}\"";
                await _logService.AddAsync(log);
            }
            return response;
        }

        public async Task<ServiceResponse<KullaniciForShowDeleteDto>> AddUserAsync(KullaniciForAddUpdateDto kullaniciForAddUpdateDto)
        {
            ServiceResponse<KullaniciForShowDeleteDto> response = new ServiceResponse<KullaniciForShowDeleteDto>();
            Log log = new Log();

            try
            {
                var userExists = await _userRepository.Exists(x => x.Email == kullaniciForAddUpdateDto.Email);
                if (userExists)
                {
                    response.Process = "Users";
                    response.Message = "User exists.";
                    response.Success = false;
                    response.Data = null;

                    log.KullaniciId = kullaniciForAddUpdateDto.Id;
                    log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                    log.Tarih = DateTime.Now;
                    log.Durum = "Başarısız";
                    log.Islem = "Yeni Kayıt";
                    log.Aciklama = $"Açıklama: \"{response.Message}.\" - Veri: \"No Data\"";
                    await _logService.AddAsync(log);
                    return response;
                }

                if (!await _userRepository.AddAsync(kullaniciForAddUpdateDto))
                {
                    response.Process = "Users";
                    response.Message = "Repository error.";
                    response.Success = false;
                    response.Data = null;

                    log.KullaniciId = kullaniciForAddUpdateDto.Id;
                    log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                    log.Tarih = DateTime.Now;
                    log.Durum = "Başarısız";
                    log.Islem = "Yeni Kayıt";
                    log.Aciklama = $"Açıklama: \"Something went wrong in repository layer when adding user {kullaniciForAddUpdateDto}.\" - Veri: \"No Data\"";
                    await _logService.AddAsync(log);
                    return response;
                }
                response.Process = "Users";
                response.Message = "User created";
                response.Success = true;
                response.Data = _mapper.Map<KullaniciForShowDeleteDto>(kullaniciForAddUpdateDto);

                log.KullaniciId = kullaniciForAddUpdateDto.Id;
                log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                log.Tarih = DateTime.Now;
                log.Durum = "Başarılı";
                log.Islem = "Yeni Kayıt";
                log.Aciklama = $"Açıklama: \"{response.Message}\" - Veri: \"{JsonConvert.SerializeObject(response.Data)}\"";
                await _logService.AddAsync(log);
            }
            catch (Exception e)
            {
                response.Process = "Users";
                response.Message = "Error occured.";
                response.Success = false;
                response.Data = null;
                response.ErrorMessages = new List<string> { Convert.ToString(e.Message) };

                log.KullaniciId = kullaniciForAddUpdateDto.Id;
                log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                log.Tarih = DateTime.Now;
                log.Durum = "Başarısız";
                log.Islem = "Yeni Kayıt";
                log.Aciklama = $"Açıklama: \"Something went wrong in service layer when adding user {kullaniciForAddUpdateDto}.\" - Veri: \"No Data\" - Hata Mesajları: \"{JsonConvert.SerializeObject(response.ErrorMessages)}\"";
                await _logService.AddAsync(log);
            }
            return response;
        }
        public async Task<ServiceResponse<KullaniciForShowDeleteDto>> UpdateUserAsync(KullaniciForAddUpdateDto kullaniciForAddUpdateDto)
        {
            ServiceResponse<KullaniciForShowDeleteDto> response = new ServiceResponse<KullaniciForShowDeleteDto>();
            Log log = new Log();

            try
            {
                var existingUser = await _userRepository.Get(x => x.Id == kullaniciForAddUpdateDto.Id);

                if (existingUser == null)
                {
                    response.Process = "Users";
                    response.Message = "Not found.";
                    response.Success = false;
                    response.Data = null;

                    log.KullaniciId = kullaniciForAddUpdateDto.Id;
                    log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                    log.Tarih = DateTime.Now;
                    log.Durum = "Başarısız";
                    log.Islem = "Güncelleme";
                    log.Aciklama = $"Açıklama: \"{response.Message}.\" - Veri: \"No Data\"";
                    await _logService.AddAsync(log);
                    return response;
                }

                if (!await _userRepository.UpdateAsync(kullaniciForAddUpdateDto))
                {
                    response.Process = "Users";
                    response.Message = "Repository error.";
                    response.Success = false;
                    response.Data = null;

                    log.KullaniciId = kullaniciForAddUpdateDto.Id;
                    log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                    log.Tarih = DateTime.Now;
                    log.Durum = "Başarısız";
                    log.Islem = "Güncelleme";
                    log.Aciklama = $"Açıklama: \"Something went wrong in repository layer when updating user {kullaniciForAddUpdateDto}.\" - Veri: \"No Data\"";
                    await _logService.AddAsync(log);
                    return response;
                }
                response.Process = "Users";
                response.Message = "Taşınmaz updated";
                response.Success = true;
                response.Data = _mapper.Map<KullaniciForShowDeleteDto>(kullaniciForAddUpdateDto);

                log.KullaniciId = kullaniciForAddUpdateDto.Id;
                log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                log.Tarih = DateTime.Now;
                log.Durum = "Başarılı";
                log.Islem = "Güncelleme";
                log.Aciklama = $"Açıklama: \"{response.Message}\" - Veri: \"{JsonConvert.SerializeObject(response.Data)}\"";
                await _logService.AddAsync(log);
            }
            catch (Exception e)
            {
                response.Process = "Users";
                response.Message = "Error occured.";
                response.Success = false;
                response.Data = null;
                response.ErrorMessages = new List<string> { Convert.ToString(e.Message) };

                log.KullaniciId = kullaniciForAddUpdateDto.Id;
                log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                log.Tarih = DateTime.Now;
                log.Durum = "Başarısız";
                log.Islem = "Güncelleme";
                log.Aciklama = $"Açıklama: \"Something went wrong in service layer when updating user {kullaniciForAddUpdateDto}.\" - Veri: \"No Data\" - Hata Mesajları: \"{JsonConvert.SerializeObject(response.ErrorMessages)}\"";
                await _logService.AddAsync(log);
            }
            return response;
        }

        public async Task<ServiceResponse<KullaniciForShowDeleteDto>> DeleteUserAsync(KullaniciForShowDeleteDto kullaniciForShowDeleteDto)
        {
            ServiceResponse<KullaniciForShowDeleteDto> response = new ServiceResponse<KullaniciForShowDeleteDto>();
            Log log = new Log();

            try
            {
                if (!await _userRepository.Exists(x => x.Id == kullaniciForShowDeleteDto.Id))
                {
                    response.Process = "Users";
                    response.Message = "Not found.";
                    response.Success = false;
                    response.Data = null;

                    log.KullaniciId = kullaniciForShowDeleteDto.Id;
                    log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                    log.Tarih = DateTime.Now;
                    log.Durum = "Başarısız";
                    log.Islem = "Silme";
                    log.Aciklama = $"Açıklama: \"{response.Message}.\" - Veri: \"No Data\"";
                    await _logService.AddAsync(log);
                    return response;
                }

                if (!await _userRepository.DeleteAsync(_mapper.Map<Kullanici>(kullaniciForShowDeleteDto)))
                {
                    response.Process = "Users";
                    response.Message = "Repository error.";
                    response.Success = false;
                    response.Data = null;

                    log.KullaniciId = kullaniciForShowDeleteDto.Id;
                    log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                    log.Tarih = DateTime.Now;
                    log.Durum = "Başarısız";
                    log.Islem = "Silme";
                    log.Aciklama = $"Açıklama: \"Something went wrong in repository layer when deleting user {kullaniciForShowDeleteDto}.\" - Veri: \"No Data\"";
                    await _logService.AddAsync(log);
                    return response;
                }
                response.Process = "Users";
                response.Success = true;
                response.Data = null;
                response.Message = "User deleted.";

                log.KullaniciId = kullaniciForShowDeleteDto.Id;
                log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                log.Tarih = DateTime.Now;
                log.Durum = "Başarılı";
                log.Islem = "Silme";
                log.Aciklama = $"Açıklama: \"{response.Message}\" - Veri: \"{JsonConvert.SerializeObject(response.Data)}\"";
                await _logService.AddAsync(log);
            }
            catch (Exception e)
            {
                response.Process = "Users";
                response.Success = false;
                response.Data = null;
                response.Message = "Error occured.";
                response.ErrorMessages = new List<string> { Convert.ToString(e.Message) };

                log.KullaniciId = kullaniciForShowDeleteDto.Id;
                log.KullaniciIp = _httpAccessor.HttpContext.Connection.ToString();
                log.Tarih = DateTime.Now;
                log.Durum = "Başarısız";
                log.Islem = "Silme";
                log.Aciklama = $"Açıklama: \"Something went wrong in service layer when deleting user {kullaniciForShowDeleteDto}.\" - Veri: \"No Data\" - Hata Mesajları: \"{JsonConvert.SerializeObject(response.ErrorMessages)}\"";
                await _logService.AddAsync(log);
            }
            return response;
        }
    }
}