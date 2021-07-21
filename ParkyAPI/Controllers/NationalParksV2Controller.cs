﻿using AutoMapper;
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
	[Route("api/v{version:apiVersion}/nationalparks")]
	[ApiVersion("2.0")]
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
			var park = _nationalParkRepository?.GetNationalParks().FirstOrDefault();

			if (park == null || _mapper == null)
				return NotFound();

			return Ok(_mapper.Map<NationalParkDto>(park));
		}
	}
}
