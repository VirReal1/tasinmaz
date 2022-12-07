using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using tasinmaz.API.Models;

namespace tasinmaz.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasinmazlarController : ControllerBase
    {
        public ActionResult GetTasinmazlar()
        {
            return Ok(201);
        }

        [HttpPost("add")]
        public ActionResult Add([FromBody] Tasinmaz tasinmaz)
        {
            return Ok(tasinmaz);
        }
    }
}
