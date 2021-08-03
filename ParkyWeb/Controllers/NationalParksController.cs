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

		public async Task<IActionResult> Upsert(int? id)
		{
			if (_nationalParkRepository == null)
				return NotFound();

			NationalPark park = new NationalPark();

			// Used for Create
			if (id == null)
				return View(park);

			// Update
			park = await _nationalParkRepository.GetAsync(StaticDetails.NationalParkAPIPath, id.GetValueOrDefault());

			if(park == null)
				return NotFound();

			return View(park);
		}

		public async Task<IActionResult> GetAllNationalParks()
		{
			return Json(new { data = await _nationalParkRepository.GetAllAsync(StaticDetails.NationalParkAPIPath) });
		}
	}
}
