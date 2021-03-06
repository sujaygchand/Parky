using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyWeb.Models;
using ParkyWeb.Repository.IRepository;
using ParkyWeb.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyWeb.Controllers
{
	[Authorize]
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

		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Upsert(int? id)
		{
			if (_nationalParkRepository == null)
				return NotFound();

			NationalPark park = new NationalPark();

			// Used for Create
			if (id == null)
				return View(park);

			// Update
			park = await _nationalParkRepository.GetAsync(StaticDetails.NationalParkAPIPath, id.GetValueOrDefault(), HttpContext.Session.GetString("JWToken"));

			if(park == null)
				return NotFound();

			return View(park);
		}

		[HttpPost, ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Upsert(NationalPark park)
		{
			if (_nationalParkRepository == null || park == null)
				return NotFound();

			if (!ModelState.IsValid)
				return View(park);

			IFormFileCollection files = HttpContext.Request.Form.Files;

			if(files.Count > 0)
			{
				byte[] picture = null;
				
				using(var fileStream = files[0].OpenReadStream())
				{
					using (var memoryStream = new MemoryStream())
					{
						fileStream.CopyTo(memoryStream);
						picture = memoryStream.ToArray();
					}
				}
				park.Picture = picture;
			}
			else
			{
				var parkFromDb = await _nationalParkRepository.GetAsync(StaticDetails.NationalParkAPIPath, park.Id, HttpContext.Session.GetString("JWToken"));
				
				if(parkFromDb != null)
					park.Picture = parkFromDb.Picture;
			}

			bool isUpdating = park.Id != 0;

			if(isUpdating)
				await _nationalParkRepository.UpdateAsync(StaticDetails.NationalParkAPIPath + park.Id, park, HttpContext.Session.GetString("JWToken"));
			else
				await _nationalParkRepository.CreateAsync(StaticDetails.NationalParkAPIPath, park, HttpContext.Session.GetString("JWToken"));

			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> GetAllNationalParks()
		{
			return Json(new { data = await _nationalParkRepository.GetAllAsync(StaticDetails.NationalParkAPIPath, HttpContext.Session.GetString("JWToken")) });
		}

		[HttpDelete]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Delete(int id)
		{
			if (_nationalParkRepository == null)
				return NotFound();

			var status = await _nationalParkRepository.DeleteAsync(StaticDetails.NationalParkAPIPath, id, HttpContext.Session.GetString("JWToken"));

			return Json(new { success = status, message = (status ? "Delete Successful" : "Delete Not Successful") });
		}
	}
}
