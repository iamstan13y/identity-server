using IdentityServer.Enums;
using IdentityServer.Models;
using IdentityServer.Models.Data;
using IdentityServer.Models.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [HttpPost("sign-up")]
        [ProducesResponseType(typeof(Result<Account>),  StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateAccount([FromBody] AccountRequest request)
        {
            var result = await _accountRepository.CreateAsync(new Account
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Password = request.Password,
                PhoneNumber = request.PhoneNumber,
                Status =  Status.Unverified,
                DateCreated = DateTime.Now,
            });

            return Ok(result);
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(Result<Account>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<Account>), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _accountRepository.LoginAsync(request);

            if (!result.Success)
                return StatusCode(StatusCodes.Status403Forbidden, result);
            
            return Ok(result);
        }

        [HttpPost("change-password")]
        [ProducesResponseType(typeof(Result<Account>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<Account>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
        {
            var result = await _accountRepository.ChangePasswordAsync(request);
            if (!result.Success) return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("reset-password/verification-code/{email}")]
        [ProducesResponseType(typeof(Result<Account>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<Account>), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ResetPassword(string email)
        {
            var result = await _accountRepository.GetResetPasswordCodeAsync(email);
            if (!result.Success) return BadRequest(result);

            return Ok(result);
        }
    }
}