using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Model;
using ParkyAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI.Controllers
{
	[Authorize]
	[Route("api/v{version:apiVersion}/Users")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IUserRepository _userRepo;

		public UserController(IUserRepository userRepo)
		{
			_userRepo = userRepo;
		}

		[AllowAnonymous]
		[HttpPost("authenticate")]
		public IActionResult Authenticate([FromBody] User model)
		{
			if (_userRepo == null)
				return BadRequest(new { message = "UserRepository is null" });

			var user = _userRepo.Authenticate(model.Username, model.Password);

			if(user == null)
				return BadRequest(new { message = "Username or password is incorrect" });

			return Ok(user);
		}
	}
}
