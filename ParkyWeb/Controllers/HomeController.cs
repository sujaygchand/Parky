using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ParkyWeb.Models;
using ParkyWeb.Models.ViewModel;
using ParkyWeb.Repository.IRepository;
using ParkyWeb.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ParkyWeb.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly INationalParkRepository _nationalParkRepo;
		private readonly ITrailRepository _trailRepo;
		private readonly IAccountRepository _accountRepo;

		public HomeController(ILogger<HomeController> logger, INationalParkRepository nationalParkRepo, ITrailRepository trailRepo, IAccountRepository accountRepo)
		{
			_logger = logger;
			_nationalParkRepo = nationalParkRepo;
			_trailRepo = trailRepo;
			_accountRepo = accountRepo;
		}

		public async Task<IActionResult> Index()
		{
			if (_nationalParkRepo == null || _trailRepo == null)
				return NotFound();

			IndexVM indexVM = new IndexVM()
			{
				NationalParkList = await _nationalParkRepo.GetAllAsync(StaticDetails.NationalParkAPIPath, HttpContext.Session.GetString("JWToken")),
				TrailList = await _trailRepo.GetAllAsync(StaticDetails.TrailAPIPath, HttpContext.Session.GetString("JWToken"))
			};
			return View(indexVM);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		[HttpGet]
		public IActionResult Login()
		{
			return View(new User());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(User inputUser)
		{
			if (_accountRepo == null)
				return NotFound();

			User user = await _accountRepo.LoginAsync(StaticDetails.AccountAPIPath + "authenticate/", inputUser);

			if (user?.Token == null)
				return View();

			var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
			identity.AddClaim(new Claim(ClaimTypes.Name, user.Username));
			identity.AddClaim(new Claim(ClaimTypes.Role, user.Role));
			var principal = new ClaimsPrincipal(identity);
			await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

			HttpContext.Session.SetString("JWToken", user.Token);

			TempData[StaticDetails.Alert] = string.Format("Welcome {0}!", user.Username);
			return RedirectToAction(nameof(Index));
		}

		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(User inputUser)
		{
			if (_accountRepo == null)
				return NotFound();

			bool isUserFound = await _accountRepo.RegisterAsync(StaticDetails.AccountAPIPath + "register/", inputUser);

			if (isUserFound == false)
				return View();

			TempData[StaticDetails.Alert] = "Registeration Successful";
			return RedirectToAction(nameof(Login));
		}

		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync();
			HttpContext.Session.SetString("JWToken", "");
			return RedirectToAction(nameof(Index));
		}


		[HttpGet]
		public IActionResult AccessDenied()
		{
			return View();
		}

	}
}
