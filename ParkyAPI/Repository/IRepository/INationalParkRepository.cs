using ParkyAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI.Repository.IRepository
{
	public interface INationalParkRepository
	{
		ICollection<NationalPark> GetNationalParks();
		NationalPark GetNationalPark(int nationalParkId);
		bool NationalParkExists(string name);
		bool NationalParkExists(int id);
		bool TryCreateNationalPark(NationalPark nationalPark);
		bool TryUpdateNationalPark(NationalPark nationalPark);
		bool TryDeleteNationalPark(NationalPark nationalPark);
		bool Save();
	}
}
