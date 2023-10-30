﻿using Application.Contracts.Identity;
using Application.Models.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("/login")]
        public async Task<ActionResult<AuthResponse>> Login(AuthRequest authRequest)
        {
            return Ok(await _authService.Login(authRequest));
        }

        [HttpPost("/register")]
        public async Task<ActionResult<RegistrationResponse>> Register(RegistrationRequest registrationRequest)
        {
            return Ok(await _authService.Register(registrationRequest));
        }
    }
}