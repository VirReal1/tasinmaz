using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using tasinmaz.API.Dtos.Log;
using tasinmaz.API.Dtos.Tasinmaz;
using tasinmaz.API.Helpers;
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
        //[Authorize(Policy = Policies.Admin)]
        public async Task<ActionResult> GetAll()
        {
            var allLoglar = await _logService.GetAllAsync();

            return Ok(allLoglar);
        }
        [HttpGet("search")]
        //[Authorize(Policy = Policies.Admin)]
        public async Task<ActionResult> GetBySearch([FromBody] LogDto logDto)
        {
            var searchLoglar = await _logService.GetLoglarAsync(logDto);

            return Ok(searchLoglar);
        }
    }
}
