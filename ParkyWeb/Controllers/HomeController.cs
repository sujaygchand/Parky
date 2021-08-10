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
using System.Threading.Tasks;

namespace ParkyWeb.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly INationalParkRepository _nationalParkRepo;
		private readonly ITrailRepository _trailRepo;

		public HomeController(ILogger<HomeController> logger, INationalParkRepository nationalParkRepo, ITrailRepository trailRepo)
		{
			_logger = logger;
			_nationalParkRepo = nationalParkRepo;
			_trailRepo = trailRepo;
		}

		public async Task<IActionResult> Index()
		{
			if (_nationalParkRepo == null || _trailRepo == null)
				return NotFound();

			IndexVM indexVM = new IndexVM()
			{
				NationalParkList = await _nationalParkRepo.GetAllAsync(StaticDetails.NationalParkAPIPath),
				TrailList = await _trailRepo.GetAllAsync(StaticDetails.TrailAPIPath)
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
	}
}
