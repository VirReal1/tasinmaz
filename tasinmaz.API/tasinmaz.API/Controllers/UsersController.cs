using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using tasinmaz.API.Data;
using tasinmaz.API.Dtos;
using tasinmaz.API.Dtos.Tasinmaz;
using tasinmaz.API.Models;
using tasinmaz.API.Models.Concrete;
using tasinmaz.API.Services.Abstract;
using tasinmaz.API.Services.Concrete;

namespace tasinmaz.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        IUserService _userService;
        IConfiguration _configuration;

        public UsersController(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<KullaniciForShowDeleteDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetAll() //WILL BE DELETED
        {
            var allUsers = await _userService.GetAllAsync();

            if (allUsers.Error)
            {
                ModelState.AddModelError("", allUsers.Message);
                return StatusCode(500, ModelState);
            }

            return Ok(allUsers);
        }

        [HttpPost("search")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<KullaniciForShowDeleteDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetBySearch([FromBody] KullaniciForShowDeleteDto kullaniciForShowDeleteDto)
        {
            var searchUsers = await _userService.GetUsersAsync(kullaniciForShowDeleteDto);

            if (searchUsers.Warning)
            {
                ModelState.AddModelError("", searchUsers.Message);
                return StatusCode(404, ModelState);
            }

            if (searchUsers.Error)
            {
                ModelState.AddModelError("", searchUsers.Message);
                return StatusCode(500, ModelState);
            }

            return Ok(searchUsers);
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(KullaniciToken))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Login([FromBody] KullaniciForLoginDto kullaniciForLoginDto)
        {
            var kullaniciToken = await _userService.LoginUserAsync(kullaniciForLoginDto);

            if (kullaniciToken.Warning)
            {
                ModelState.AddModelError("", kullaniciToken.Message);
                return StatusCode(401, ModelState);
            }

            if (kullaniciToken.Error)
            {
                ModelState.AddModelError("", kullaniciToken.Message);
                return StatusCode(500, ModelState);
            }

            //TODO RETURN WRAPPER
            //return Ok(new { kullaniciToken.Data.Token, kullaniciToken.Data.AdminMi });
            return Ok(kullaniciToken.Data);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(KullaniciForAddUpdateDto))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Add([FromBody] KullaniciForAddUpdateDto kullaniciForAddUpdateDto)
        {
            var createUser = await _userService.AddUserAsync(kullaniciForAddUpdateDto);


            if (createUser.Warning)
            {
                ModelState.AddModelError("", createUser.Message);
                return StatusCode(403, ModelState);
            }

            if (createUser.Error)
            {
                ModelState.AddModelError("", createUser.Message);
                return StatusCode(500, ModelState);
            }

            return Ok(createUser);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(KullaniciForAddUpdateDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Update([FromBody] KullaniciForAddUpdateDto kullaniciForAddUpdateDto)
        {
            var updateUser = await _userService.UpdateUserAsync(kullaniciForAddUpdateDto);

            if (updateUser.Warning)
            {
                ModelState.AddModelError("", updateUser.Message);
                return StatusCode(404, ModelState);
            }

            if (updateUser.Error)
            {
                ModelState.AddModelError("", updateUser.Message);
                return StatusCode(500, ModelState);
            }

            return Ok(updateUser);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Delete([FromBody] KullaniciForShowDeleteDto kullaniciForShowDeleteDto)
        {
            var removeUser = await _userService.DeleteUserAsync(kullaniciForShowDeleteDto);

            if (removeUser.Warning)
            {
                ModelState.AddModelError("", removeUser.Message);
                return StatusCode(404, ModelState);
            }

            if (removeUser.Error)
            {
                ModelState.AddModelError("", removeUser.Message);
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
