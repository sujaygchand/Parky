using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ParkyWeb.Models;
using ParkyWeb.Models.ViewModel;
using ParkyWeb.Repository.IRepository;
using ParkyWeb.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyWeb.Controllers
{
	public class TrailsController : Controller
	{
		public readonly INationalParkRepository _nationalParkRepo;
		public readonly ITrailRepository _trailRepo;

		public TrailsController(INationalParkRepository nationalParkRepo, ITrailRepository trailRepo)
		{
			_nationalParkRepo = nationalParkRepo;
			_trailRepo = trailRepo;
		}

		public IActionResult Index()
		{
			return View(new Trail() { });
		}

		public async Task<IActionResult> Upsert(int? id)
		{
			if (_nationalParkRepo == null || _trailRepo == null)
				return NotFound();

			IEnumerable<NationalPark> parkList = await _nationalParkRepo.GetAllAsync(StaticDetails.NationalParkAPIPath);

			TrailsVM trailVM = new TrailsVM()
			{
				NationalParkList = parkList.Select(k => new SelectListItem
				{
					Text = k.Name,
					Value = k.Id.ToString()
				}),

				Trail = new Trail()
			};

			// Used for Create
			if (id == null)
				return View(trailVM);

			// Update
			trailVM.Trail = await _trailRepo.GetAsync(StaticDetails.TrailAPIPath, id.GetValueOrDefault());

			if (trailVM.Trail == null)
				return NotFound();

			return View(trailVM);
		}

		[HttpPost, ValidateAntiForgeryToken]
		public async Task<IActionResult> Upsert(TrailsVM trailVM)
		{
			if (_trailRepo == null || trailVM?.Trail == null)
				return NotFound();

			if (!ModelState.IsValid)
			{
				IEnumerable<NationalPark> parkList = await _nationalParkRepo.GetAllAsync(StaticDetails.NationalParkAPIPath);

				TrailsVM objVM = new TrailsVM()
				{
					NationalParkList = parkList.Select(k => new SelectListItem
					{
						Text = k.Name,
						Value = k.Id.ToString()
					}),

					Trail = trailVM.Trail
				};

				return View(objVM);
			}

			if (trailVM.Trail.Id == 0)
				await _trailRepo.CreateAsync(StaticDetails.TrailAPIPath, trailVM.Trail);
			else
				await _trailRepo.UpdateAsync(StaticDetails.TrailAPIPath + trailVM.Trail.Id, trailVM.Trail);

			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> GetAllTrails()
		{
			return Json(new { data = await _trailRepo.GetAllAsync(StaticDetails.TrailAPIPath) });
		}

		[HttpDelete]
		public async Task<IActionResult> Delete(int id)
		{
			if (_trailRepo == null)
				return NotFound();

			var status = await _trailRepo.DeleteAsync(StaticDetails.TrailAPIPath, id);

			return Json(new { success = status, message = (status ? "Delete Successful" : "Delete Not Successful") });
		}
	}
}
