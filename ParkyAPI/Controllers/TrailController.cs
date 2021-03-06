using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Model;
using ParkyAPI.Model.DTOs;
using ParkyAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI.Controllers
{
	[Route("api/v{version:apiVersion}/trails")]
	[ApiController]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public class TrailController : ControllerBase
	{
		private readonly ITrailRepository _trailRepository;
		private readonly IMapper _mapper;

		public TrailController(ITrailRepository trailRepository, IMapper mapper)
		{
			_trailRepository = trailRepository;
			_mapper = mapper;
		}

		/// <summary>
		/// Get list of Trails.
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[ProducesResponseType(200, Type = typeof(List<TrailDto>))]
		public IActionResult GetTrails()
		{
			ICollection<Trail> trailList = _trailRepository?.GetTrails();

			if (trailList == null || _mapper == null)
				return NotFound();

			List<TrailDto> trailDtos = new List<TrailDto>();

			foreach (var trail in trailList)
			{
				trailDtos.Add(_mapper.Map<TrailDto>(trail));
			}

			return Ok(trailDtos);
		}

		/// <summary>
		/// Get individual Trail
		/// </summary>
		/// <param name="id"> The Id of the Trail </param>
		/// <returns></returns>
		[HttpGet("{id:int}", Name = "[action]")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TrailDto))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesDefaultResponseType]
		[Authorize(Roles = "Admin")]
		public IActionResult GetTrail(int id)
		{
			Trail trail = _trailRepository?.GetTrail(id);

			if (trail == null)
				return NotFound();

			TrailDto trailDto = _mapper.Map<TrailDto>(trail);
			return Ok(trailDto);
		}

		/// <summary>
		/// Get Trails in the national park
		/// </summary>
		/// <param name="id"> The Id of the National Park </param>
		/// <returns></returns>
		[HttpGet("[action]/{id:int}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TrailDto))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesDefaultResponseType]
		[Authorize(Roles = "Admin")]
		public IActionResult GetTrailsInNationalPark(int id)
		{
			var trails = _trailRepository?.GetTrailsInNationalPark(id);

			if (trails == null)
				return NotFound();

			List<TrailDto> trailDtos = new List<TrailDto>();

			foreach(var item in trails)
			{
				trailDtos.Add(_mapper.Map<TrailDto>(item));
			}
			
			return Ok(trailDtos);
		}


		[HttpPost]
		[ProducesResponseType(201, Type = typeof(TrailDto))]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[Authorize(Roles = "Admin")]
		public IActionResult CreateTrail([FromBody] TrailCreateDto trailDto)
		{
			if (trailDto == null || _trailRepository == null || _mapper == null)
				return BadRequest(ModelState);

			if (_trailRepository.TrailExists(trailDto.Name))
			{
				ModelState.AddModelError("", "Trail Exists");
				return StatusCode(404, ModelState);
			}

			Trail trail = _mapper.Map<Trail>(trailDto);

			if (_trailRepository.TryCreateTrail(trail) == false)
			{
				ModelState.AddModelError("", $"Something went wrong when saving the record {trail.Name}");
				return StatusCode(500, ModelState);
			}

			return CreatedAtRoute(nameof(GetTrail), new { id = trail.Id }, trail);
		}

		[HttpPatch("{id:int}", Name = "UpdateTrail")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[Authorize(Roles = "Admin")]
		public IActionResult UpdateTrail(int id, [FromBody] TrailUpdateDto trailDto)
		{
			if (id != trailDto.Id)
			{
				ModelState.AddModelError("", "id did not match id in the body");
				return StatusCode(404, ModelState);
			}

			if (trailDto == null || _trailRepository == null || _mapper == null)
				return BadRequest(ModelState);

			Trail trail = _mapper.Map<Trail>(trailDto);

			if (_trailRepository.TryUpdateTrail(trail) == false)
			{
				ModelState.AddModelError("", $"Something went wrong when updating the record {trail.Name}");
				return StatusCode(500, ModelState);
			}

			return NoContent();
		}

		[HttpDelete("{id:int}", Name = "DeleteTrail")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status409Conflict)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[Authorize(Roles = "Admin")]
		public IActionResult DeleteTrail(int id)
		{
			if (_trailRepository == null || _mapper == null)
				return BadRequest(ModelState);

			Trail trail = _trailRepository.GetTrail(id);

			if (trail == null)
			{
				ModelState.AddModelError("", "Trail dose not exists for deletion");
				return StatusCode(404, ModelState);
			}

			if (_trailRepository.TryDeleteTrail(trail) == false)
			{
				ModelState.AddModelError("", $"Something went wrong when deleting the record {trail.Name}");
				return StatusCode(500, ModelState);
			}

			return NoContent();
		}
	}
}
