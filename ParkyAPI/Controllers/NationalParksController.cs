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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace ParkyAPI.Controllers
{
	[Route("api/v{version:apiVersion}/nationalparks")]
	[ApiController]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public class NationalParksController : ControllerBase
	{
		private readonly INationalParkRepository _nationalParkRepository;
		private readonly IMapper _mapper;

		public NationalParksController(INationalParkRepository nationalParkRepository, IMapper mapper)
		{
			_nationalParkRepository = nationalParkRepository;
			_mapper = mapper;
		}

		/// <summary>
		/// Get list of national parks.
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[ProducesResponseType(200, Type = typeof(List<NationalParkDto>))]
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

		/// <summary>
		/// Get individual national park
		/// </summary>
		/// <param name="id"> The Id of the national Park </param>
		/// <returns></returns>
		[HttpGet("{id:int}", Name = "GetNationalPark")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NationalParkDto))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[Authorize]
		[ProducesDefaultResponseType]
		public IActionResult GetNationalPark(int id)
		{
			NationalPark park = _nationalParkRepository?.GetNationalPark(id);

			if (park == null)
				return NotFound();

			NationalParkDto parkDto = _mapper.Map<NationalParkDto>(park);
			return Ok(parkDto);
		}

		[HttpPost]
		[ProducesResponseType(201, Type = typeof(NationalParkDto))]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

			return CreatedAtRoute(nameof(GetNationalPark), new { version = HttpContext.GetRequestedApiVersion().ToString(), id = nationalPark.Id}, nationalPark);
		}

		[HttpPatch("{id:int}", Name = "UpdateNationalPark")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

		[HttpDelete("{id:int}", Name = "DeleteNationalPark")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status409Conflict)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public IActionResult DeleteNationalPark(int id)
		{
			if (_nationalParkRepository == null || _mapper == null)
				return BadRequest(ModelState);

			var nationalPark = _nationalParkRepository.GetNationalPark(id);

			if (nationalPark == null)
			{
				ModelState.AddModelError("", "National Park dose not exists for deletion");
				return StatusCode(404, ModelState);
			}

			if (_nationalParkRepository.TryDeleteNationalPark(nationalPark) == false)
			{
				ModelState.AddModelError("", $"Something went wrong when deleting the record {nationalPark.Name}");
				return StatusCode(500, ModelState);
			}

			return NoContent();
		}
	}
}
