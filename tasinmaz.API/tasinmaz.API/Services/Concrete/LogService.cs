using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using tasinmaz.API.Data;
using tasinmaz.API.Dtos.Log;
using tasinmaz.API.Dtos.Tasinmaz;
using tasinmaz.API.Entities.Concrete;
using tasinmaz.API.Models;
using tasinmaz.API.ServiceResponder;
using tasinmaz.API.Services.Abstract;

namespace tasinmaz.API.Services.Concrete
{
    public class LogService : ILogService
    {
        IMapper _mapper;
        ILogRepository _logRepository;

        public LogService(IMapper mapper, ILogRepository logRepository)
        {
            _mapper = mapper;
            _logRepository = logRepository;
        }

        public async Task<ServiceResponse<List<LogDto>>> GetAllAsync()
        {
            ServiceResponse<List<LogDto>> response = new ServiceResponse<List<LogDto>>();

            try
            {
                var logAllList = await _logRepository.GetAllAsync();
                var logDtoAllList = new List<LogDto>();

                if (logAllList.Count == 0)
                {
                    response.Process = "Loglar";
                    response.Message = "Loglar veri tabanında bulunamadı.";
                    response.Warning = true;
                    response.Data = null;
                    return response;
                }

                foreach (var item in logAllList)
                {
                    logDtoAllList.Add(_mapper.Map<LogDto>(item));
                }

                response.Process = "Loglar";
                response.Message = "Bütün loglar getirildi.";
                response.Data = logDtoAllList;
            }
            catch (Exception e)
            {
                response.Process = "Loglar";
                response.Message = "Logları getirirken servis katmanında bir hata oluştu.";
                response.Error = true;
                response.Data = null;
                response.ErrorMessages = new List<string> { Convert.ToString(e.Message) };
            }
            return response;
        }

        public async Task<ServiceResponse<List<LogDto>>> GetLoglarAsync(LogDto logDto)
        {
            ServiceResponse<List<LogDto>> response = new ServiceResponse<List<LogDto>>();

            try
            {
                var logList = await _logRepository.GetAllAsync(x =>
                    (x.KullaniciIp.Contains(logDto.KullaniciIp, StringComparison.InvariantCultureIgnoreCase) || logDto.KullaniciIp == null) &&
                    (x.Tarih.ToString().Contains(logDto.Tarih.ToString(), StringComparison.InvariantCultureIgnoreCase) || logDto.Tarih == null) &&
                    (x.Durum.Contains(logDto.Durum, StringComparison.InvariantCultureIgnoreCase) || logDto.Durum == null) &&
                    (x.Islem.Contains(logDto.Islem, StringComparison.InvariantCultureIgnoreCase) || logDto.Islem == null) &&
                    (x.Aciklama.Contains(logDto.Aciklama, StringComparison.InvariantCultureIgnoreCase) || logDto.Aciklama == null) &&
                    (x.Kullanici.Id.ToString().Contains(logDto.KullaniciId.ToString(), StringComparison.InvariantCultureIgnoreCase) || logDto.KullaniciId == default));

                if (logList == null)
                {
                    response.Process = "Loglar";
                    response.Message = "Arama parametreleri veri tabanıyla eşleşmedi.";
                    response.Warning = true;
                    response.Data = null;
                    return response;
                }
                response.Process = "Loglar";
                response.Message = "Loglar başarıyla filtrelendi.";
                response.Data = _mapper.Map<List<LogDto>>(logList);
            }
            catch (Exception e)
            {
                response.Process = "Loglar";
                response.Message = "Logları filtrelerken servis katmanında bir hata oluştu.";
                response.Error = true;
                response.Data = null;
                response.ErrorMessages = new List<string> { Convert.ToString(e.Message) };
            }
            return response;
        }

        public async Task<ServiceResponse<LogDto>> AddLogAsync(Log log)
        {
            ServiceResponse<LogDto> response = new ServiceResponse<LogDto>();
            try
            {
                if (!await _logRepository.AddAsync(log))
                {
                    response.Process = "Loglar";
                    response.Message = "Log veri tabanına eklenirken bir hata oluştu.";
                    response.Error = true;
                    response.Data = null;
                    return response;
                }
                response.Process = "Loglar";
                response.Message = "Log başarıyla eklendi.";
                response.Data = _mapper.Map<LogDto>(log);
            }
            catch (Exception e)
            {
                response.Process = "Loglar";
                response.Message = "Log servis katmanında eklenirken bir hata oluştu.";
                response.Error = true;
                response.Data = null;
                response.ErrorMessages = new List<string> { Convert.ToString(e.Message) };
            }

            return response;
        }
    }
}