using AutoMapper;
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
	[Route("api/[controller]")]
	[ApiController]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public class NationalParksV2Controller : Controller
	{
		private readonly INationalParkRepository _nationalParkRepository;
		private readonly IMapper _mapper;

		public NationalParksV2Controller(INationalParkRepository nationalParkRepository, IMapper mapper)
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

			foreach (var park in parkList)
			{
				parkDtos.Add(_mapper.Map<NationalParkDto>(park));
			}

			return Ok(parkDtos);
		}
	}
}
