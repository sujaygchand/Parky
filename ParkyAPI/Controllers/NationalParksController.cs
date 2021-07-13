using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Model;
using ParkyAPI.Model.DTOs;
using ParkyAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ParkyAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class NationalParksController : Controller
	{
		private readonly INationalParkRepository _nationalParkRepository;
		private readonly IMapper _mapper;

		public NationalParksController(INationalParkRepository nationalParkRepository, IMapper mapper)
		{
			_nationalParkRepository = nationalParkRepository;
			_mapper = mapper;
		}

		[HttpGet]
		public IActionResult GetNationalParks()
		{
			ICollection<NationalPark> parkList = _nationalParkRepository?.GetNationalParks();

			if (parkList == null || _mapper == null)
				return NotFound();

			List<NationalParkDto> parkDtos = new List<NationalParkDto>();

			foreach(var park in parkList)
			{
				parkDtos.Add(_mapper.Map<NationalParkDto>(park));
			}

			return Ok(parkDtos);
		}

		[HttpGet("{id:int}", Name = "GetNationalPark")]
		public IActionResult GetNationalPark(int id)
		{
			NationalPark park = _nationalParkRepository?.GetNationalPark(id);

			if (park == null)
				return NotFound();

			NationalParkDto parkDto = _mapper.Map<NationalParkDto>(park);
			return Ok(parkDto);
		}

		[HttpPost]
		public IActionResult CreateNationalPark([FromBody] NationalParkDto nationalParkDto)
		{
			if (nationalParkDto == null || _nationalParkRepository == null || _mapper == null)
				return BadRequest(ModelState);

			if (_nationalParkRepository.NationalParkExists(nationalParkDto.Name))
			{
				ModelState.AddModelError("", "National Park Exists");
				return StatusCode(404, ModelState);
			}

			NationalPark nationalPark = _mapper.Map<NationalPark>(nationalParkDto);

			if(_nationalParkRepository.TryCreateNationalPark(nationalPark) == false)
			{
				ModelState.AddModelError("", $"Something went wrong when saving the record {nationalPark.Name}");
				return StatusCode(500, ModelState);
			}

			return CreatedAtRoute(nameof(GetNationalPark), new { id = nationalPark.Id}, nationalPark);
		}

		[HttpPatch("{id:int}", Name = "UpdateNationalPark")]
		public IActionResult UpdateNationalPark(int id, [FromBody]NationalParkDto nationalParkDto)
		{
			if (id != nationalParkDto.Id)
			{
				ModelState.AddModelError("", "id did not match id in the body");
				return StatusCode(404, ModelState);
			}

			if (nationalParkDto == null || _nationalParkRepository == null || _mapper == null)
				return BadRequest(ModelState);

			NationalPark nationalPark = _mapper.Map<NationalPark>(nationalParkDto);

			if(_nationalParkRepository.TryUpdateNationalPark(nationalPark) == false)
			{
				ModelState.AddModelError("", $"Something went wrong when updating the record {nationalPark.Name}");
				return StatusCode(500, ModelState);
			}

			return NoContent();
		}
	}
}
