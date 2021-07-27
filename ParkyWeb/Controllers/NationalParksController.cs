using Microsoft.AspNetCore.Mvc;
using ParkyWeb.Models;
using ParkyWeb.Repository.IRepository;
using ParkyWeb.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyWeb.Controllers
{
	public class NationalParksController : Controller
	{
		public readonly INationalParkRepository _nationalParkRepository;

		public NationalParksController(INationalParkRepository nationalParkRepository)
		{
			_nationalParkRepository = nationalParkRepository;
		}

		public IActionResult Index()
		{
			return View(new NationalPark() { });
		}

		public async Task<IActionResult> GetAllNationalParks()
		{
			return Json(new { data = await _nationalParkRepository.GetAllAsync(StaticDetails.NationalParkAPIPath) });
		}
	}
}
