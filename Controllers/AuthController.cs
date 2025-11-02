using Banking_CapStone.DTO.Request.Auth;
using Banking_CapStone.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Banking_CapStone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase 
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LoginAsync(request);

            if (!result.Success)
                return Unauthorized(result);

            return Ok(result);
        }


        [HttpPost("register/superadmin")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterSuperAdmin([FromBody] RegisterSuperAdminRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterSuperAdminAsync(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("register/bankuser")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> RegisterBankUser([FromBody] RegisterBankUserRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterBankUserAsync(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("register/client")]
        [Authorize(Roles = "BankUser,SuperAdmin")]
        public async Task<IActionResult> RegisterClient([FromBody] RegisterClientRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterClientAsync(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = GetUserId();
            if (userId == 0)
                return Unauthorized("Invalid user token");

            var result = await _authService.ChangePasswordAsync(userId, request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var userId = GetUserId();
            if (userId == 0)
                return Unauthorized("Invalid user token");

            var result = await _authService.GetUserProfileAsync(userId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPut("deactivate/{userId}")]
        [Authorize(Roles = "SuperAdmin,BankUser")]
        public async Task<IActionResult> DeactivateUser(int userId)
        {
            var result = await _authService.DeactivateUserAsync(userId);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("activate/{userId}")]
        [Authorize(Roles = "SuperAdmin,BankUser")]
        public async Task<IActionResult> ActivateUser(int userId)
        {
            var result = await _authService.ActivateUserAsync(userId);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

   
        [HttpPost("validate-token")]
        [AllowAnonymous]
        public async Task<IActionResult> ValidateToken([FromBody] string token)
        {
            var isValid = await _authService.ValidateTokenAsync(token);
            return Ok(new { token, isValid });
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
        }
    }
}
