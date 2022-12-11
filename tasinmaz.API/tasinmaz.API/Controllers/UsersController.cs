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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetAll() //WILL BE DELETED
        {
            var allUsers = await _userService.GetAllAsync();

            if (allUsers.Success == false && allUsers.Message == "Error occured.")
            {
                ModelState.AddModelError("", $"Something went wrong in service layer when getting users.");
                return StatusCode(500, ModelState);
            }

            return Ok(allUsers);
        }

        [HttpPost("search")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<KullaniciForShowDeleteDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetBySearch([FromBody] KullaniciForShowDeleteDto kullaniciForShowDeleteDto)
        {
            if (kullaniciForShowDeleteDto == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var searchUsers = await _userService.GetUsersAsync(kullaniciForShowDeleteDto);
            
            if (searchUsers.Success == false && searchUsers.Message == "No Parameter.")
            {
                ModelState.AddModelError("", searchUsers.Message);
                return StatusCode(404, ModelState);
            }

            if (searchUsers.Success == false && searchUsers.Message == "Parameters does not match with the database.")
            {
                ModelState.AddModelError("", searchUsers.Message);
                return StatusCode(404, ModelState);
            }

            if (searchUsers.Success == false && searchUsers.Message == "Error occured.")
            {
                ModelState.AddModelError("", $"Something went wrong in service layer when searching users.");
                return StatusCode(500, ModelState);
            }

            return Ok(searchUsers);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] KullaniciForLoginDto kullaniciForLoginDto)
        {
            if (kullaniciForLoginDto == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var kullaniciToken = await _userService.LoginUserAsync(kullaniciForLoginDto);

            if (kullaniciToken.Success == false && kullaniciToken.Message == "E-Mail or password is wrong")
            {
                return Unauthorized();
            }

            if (kullaniciToken.Success == false && kullaniciToken.Message == "Error occured.")
            {
                ModelState.AddModelError("", $"Something went wrong in service layer when logging user {kullaniciForLoginDto}.");
                return StatusCode(500, ModelState);
            }
            //TODO RETURN WRAPPER
            return Ok(new { kullaniciToken.Data.Token, kullaniciToken.Data.AdminMi });
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TasinmazDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Add([FromBody] KullaniciForAddUpdateDto kullaniciForAddUpdateDto)
        {
            if (kullaniciForAddUpdateDto == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createUser = await _userService.AddUserAsync(kullaniciForAddUpdateDto);

            if (createUser.Success == false && createUser.Message == "User exists.")
            {
                return Ok(createUser);
            }

            if (createUser.Success == false && createUser.Message == "Repository error.")
            {
                ModelState.AddModelError("", $"Something went wrong in repository layer when adding user {kullaniciForAddUpdateDto}.");
                return StatusCode(500, ModelState);
            }

            if (createUser.Success == false && createUser.Message == "Error occured.")
            {
                ModelState.AddModelError("", $"Something went wrong in service layer when adding user {kullaniciForAddUpdateDto}.");
                return StatusCode(500, ModelState);
            }

            return Ok(createUser);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Update([FromBody] KullaniciForAddUpdateDto kullaniciForAddUpdateDto)
        {
            if (kullaniciForAddUpdateDto == null)
            {
                return BadRequest(ModelState);
            }

            var updateUser = await _userService.UpdateUserAsync(kullaniciForAddUpdateDto);

            if (updateUser.Success == false && updateUser.Message == "Not found.")
            {
                return Ok(updateUser);
            }

            if (updateUser.Success == false && updateUser.Message == "Repository error.")
            {
                ModelState.AddModelError("", $"Something went wrong in repository layer when updating user {kullaniciForAddUpdateDto}.");
                return StatusCode(500, ModelState);
            }

            if (updateUser.Success == false && updateUser.Message == "Error occured.")
            {
                ModelState.AddModelError("", $"Something went wrong in service layer when updating user {kullaniciForAddUpdateDto}.");
                return StatusCode(500, ModelState);
            }
            return Ok(updateUser);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Delete([FromBody] KullaniciForShowDeleteDto kullaniciForShowDeleteDto)
        {
            if (kullaniciForShowDeleteDto == null)
            {
                return BadRequest(ModelState);
            }

            var removeUser = await _userService.DeleteUserAsync(kullaniciForShowDeleteDto);

            if (removeUser.Success == false && removeUser.Message == "Not found.")
            {
                ModelState.AddModelError("", removeUser.Message);
                return StatusCode(404, ModelState);

            }

            if (removeUser.Success == false && removeUser.Message == "Repository error.")
            {
                ModelState.AddModelError("", $"Something went wrong in repository layer when deleting user {kullaniciForShowDeleteDto}.");
                return StatusCode(500, ModelState);
            }

            if (removeUser.Success == false && removeUser.Message == "Error occured.")
            {
                ModelState.AddModelError("", $"Something went wrong in service layer when deleting user {kullaniciForShowDeleteDto}.");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
