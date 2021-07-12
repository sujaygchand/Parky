using AutoMapper;
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

		[HttpGet("{id:int}")]
		public IActionResult GetNationalPark(int id)
		{
			NationalPark park = _nationalParkRepository?.GetNationalPark(id);

			if (park == null)
				return NotFound();

			NationalParkDto parkDto = _mapper.Map<NationalParkDto>(park);
			return Ok(parkDto);
		}
	}
}
