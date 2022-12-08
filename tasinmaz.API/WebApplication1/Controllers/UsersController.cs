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
using tasinmaz.API.Models;

namespace tasinmaz.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        IUserRepository _userRepository;
        IConfiguration _configuration;

        public UsersController(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        [HttpGet]
        public ActionResult GetUsers() //WILL BE DELETED
        {
            var users = _userRepository.GetUsers();
            return Ok(users);
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] KullaniciForRegisterDto kullaniciForRegisterDto)
        {
            if (await _userRepository.UserExists(kullaniciForRegisterDto.Email))
            {
                ModelState.AddModelError("Email", "Email already exists");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var kullaniciToCreate = new Kullanici()
            {
                Email = kullaniciForRegisterDto.Email,
                Ad = kullaniciForRegisterDto.Ad,
                Soyad = kullaniciForRegisterDto.Soyad,
                AdminMi = kullaniciForRegisterDto.AdminMi
            };

            var createdKullanici = await _userRepository.Register(kullaniciToCreate, kullaniciForRegisterDto.Password);
            return StatusCode(201, createdKullanici);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] KullaniciForLoginDto kullaniciForLoginDto)
        {
            var kullanici = await _userRepository.Login(kullaniciForLoginDto.Email, kullaniciForLoginDto.Password);

            if (kullanici == null)
            {
                return Unauthorized();
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("AppSettings:Token").Value);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, kullanici.Id.ToString()),
                    new Claim(ClaimTypes.Email, kullanici.Email)
                }),
                Expires = DateTime.Now.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return Ok(new { tokenString, kullanici.AdminMi });
        }
    }
}
