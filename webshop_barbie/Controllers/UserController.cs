using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using webshop_barbie.DTOs;
using webshop_barbie.Models;
using webshop_barbie.Service.Interfaces;

namespace webshop_barbie.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        [HttpGet("{userId}")]
        public async Task<ActionResult<UserDTO>> GetUserByIdAsync(int userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            return Ok(user);
        }

        [HttpGet("by-email")]
        public async Task<ActionResult<UserDTO?>> GetUserByEmailAsync([FromQuery] string email)
        {
            //üres email esetén 400 Bad Request
            if (string.IsNullOrWhiteSpace(email))
                return BadRequest("Az email megadása kötelező.");

            //email formátum ellenőrzése
            var emailAttribute = new EmailAddressAttribute();
            if (!emailAttribute.IsValid(email))
                return BadRequest("Az email formátuma érvénytelen.");

            var user = await _userService.GetUserByEmailAsync(email);

            return Ok(user);
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> AddUserAsync(RegisterRequestDTO user)
        {
            var newUser = await _userService.AddUserAsync(user);
            return Ok(newUser);
        }

        [HttpPut("{user.Id}")]
        public async Task<ActionResult<UserDTO>> UpdateUserAsync(UpdateUserRequestDTO user)
        {
            var updatedUser = await _userService.UpdateUserAsync(user);
            if (updatedUser == null)
                return NotFound($"A felhasználó ({user.Id}) nem található.");

            return Ok(updatedUser);
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDTO>> ValidateLoginAsync([FromBody] LoginRequestDTO request)
        {
            var loggedInUser = await _userService.ValidateLoginAsync(request.Email, request.Password);
            return Ok(loggedInUser);
        }
    }
}
