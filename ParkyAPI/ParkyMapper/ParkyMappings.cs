﻿using AutoMapper;
using ParkyAPI.Model;
using ParkyAPI.Model.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI.ParkyMapper
{
	public class ParkyMappings : Profile
	{
		public ParkyMappings()
		{
			CreateMap<NationalPark, NationalParkDto>().ReverseMap();
			CreateMap<Trail, TrailDto>().ReverseMap();
			CreateMap<Trail, TrailUpdateDto>().ReverseMap();
			CreateMap<Trail, TrailCreateDto>().ReverseMap();
		}
	}
}
