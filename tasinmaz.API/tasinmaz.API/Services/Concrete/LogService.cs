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

            var logAllList = await _logRepository.GetAllAsync();
            var logDtoAllList = new List<LogDto>();

            foreach (var item in logAllList)
            {
                logDtoAllList.Add(_mapper.Map<LogDto>(item));
            }

            try
            {
                response.Message = "Got all logs.";
                response.Success = true;
                response.Data = logDtoAllList;
            }
            catch (Exception e)
            {
                response.Message = "Error occured.";
                response.Success = false;
                response.Data = logDtoAllList;
                response.ErrorMessages = new List<string> { Convert.ToString(e.Message) };
            }
            return response;
        }

        public async Task<ServiceResponse<List<LogDto>>> GetLoglarAsync(LogDto logDto)
        {
            ServiceResponse<List<LogDto>> response = new ServiceResponse<List<LogDto>>();

            var logAllList = await _logRepository.GetAllAsync();
            var logDtoAllList = new List<LogDto>();

            var logList = await _logRepository.GetAllAsync(x =>
                (x.KullaniciIp.Contains(logDto.KullaniciIp, StringComparison.InvariantCultureIgnoreCase) || logDto.KullaniciIp == null) &&
                (x.Tarih.ToString().Contains(logDto.Tarih.ToString(), StringComparison.InvariantCultureIgnoreCase) || logDto.Tarih == null) &&
                (x.Durum.Contains(logDto.Durum, StringComparison.InvariantCultureIgnoreCase) || logDto.Durum == null) && 
                (x.Islem.Contains(logDto.Islem, StringComparison.InvariantCultureIgnoreCase) || logDto.Islem == null) &&
                (x.Aciklama.Contains(logDto.Aciklama, StringComparison.InvariantCultureIgnoreCase) || logDto.Aciklama == null) && 
                (x.Kullanici.Id.ToString().Contains(logDto.KullaniciId.ToString(), StringComparison.InvariantCultureIgnoreCase) || logDto.KullaniciId == default));

            foreach (var item in logAllList)
            {
                logDtoAllList.Add(_mapper.Map<LogDto>(item));
            }

            try
            {
                if (logDto == null)
                {
                    response.Message = "No Parameter";
                    response.Success = false;
                    response.Data = logDtoAllList;
                    return response;
                }

                if (logList == null)
                {
                    response.Message = "Parameters does not match with the database.";
                    response.Success = false;
                    response.Data = logDtoAllList;
                    return response;
                }
                response.Message = "Logs searched.";
                response.Success = false;
                response.Data = _mapper.Map<List<LogDto>>(logList);
            }
            catch (Exception e)
            {
                response.Message = "Error occured.";
                response.Success = false;
                response.Data = logDtoAllList;
                response.ErrorMessages = new List<string> { Convert.ToString(e.Message) };
            }
            return response;
        }

        public async Task<ServiceResponse<LogDto>> AddAsync(Log log)
        {
            ServiceResponse<LogDto> response = new ServiceResponse<LogDto>();
            try
            {
                
                if (!await _logRepository.AddAsync(log))
                {
                    response.Message = "Repository error.";
                    response.Success = false;
                    response.Data = null;
                    return response;
                }
                response.Success = true;
                response.Data = _mapper.Map<LogDto>(log);
                response.Message = "Log created";
            }
            catch (Exception e)
            {
                response.Message = "Error occured.";
                response.Success = false;
                response.Data = null;
                response.ErrorMessages = new List<string> { Convert.ToString(e.Message) };
            }

            return response;
        }
    }
}