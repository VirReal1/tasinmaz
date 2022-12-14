using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<LogDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetAll()
        {
            var allLoglar = await _logService.GetAllAsync();

            if (allLoglar.Error)
            {
                ModelState.AddModelError("", allLoglar.Message);
                return StatusCode(500, ModelState);
            }

            return Ok(allLoglar);
        }
        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<LogDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetBySearch([FromBody] LogDto logDto)
        {
            var searchLoglar = await _logService.GetLoglarAsync(logDto);

            if (searchLoglar.Warning)
            {
                ModelState.AddModelError("", searchLoglar.Message);
                return StatusCode(404, ModelState);
            }

            if (searchLoglar.Error)
            {
                ModelState.AddModelError("", searchLoglar.Message);
                return StatusCode(500, ModelState);
            }

            return Ok(searchLoglar);
        }
    }
}
