using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static ParkyAPI.Model.Trail;

namespace ParkyAPI.Model.DTOs
{
	public class TrailCreateDto
	{
		[Required]
		public string Name { get; set; }

		[Required]
		public double Distance { get; set; }

		[Required]
		public double Elevation { get; set; }

		public DifficultyType Difficulty { get; set; }
		[Required]
		public int NationalParkId { get; set; }
	}
}
