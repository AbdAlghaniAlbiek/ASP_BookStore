using BookStore.API.Models;
using BookStore.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUp signUp)
        {
            var result = await _accountRepository.SignUpAsync(signUp);

            if (result.Succeeded)
                return Ok(result.Succeeded);

            return Unauthorized();
        }

        [ApiExplorerSettings(IgnoreApi = true, GroupName = "Home")]
        [HttpPost("login")]
        public async Task<IActionResult> SignUp([FromBody] SignIn signIn)
        {
            var result = await _accountRepository.LoginAsync(signIn);

            if (string.IsNullOrEmpty(result))
                return Unauthorized();

            return Ok(result);
        }
    }
}
