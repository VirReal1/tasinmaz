﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using tasinmaz.API.Dtos.Log;
using tasinmaz.API.Dtos.Tasinmaz;
using tasinmaz.API.Services.Abstract;

namespace tasinmaz.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        ILogService _logService;

        public LogsController(ILogService logService)
        {
            _logService = logService;
        }
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var loglar = await _logService.GetAllAsync();
            return Ok(loglar); //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        }
        [HttpGet("search")]
        public async Task<ActionResult> GetBySearch([FromBody] LogDto logDto = null)
        {
            var loglar = await _logService.GetLoglarAsync(logDto);
            return Ok(loglar); //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        }
    }
}