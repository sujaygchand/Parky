using ParkyAPI.Model;
using ParkyAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI.Repository
{
	public class NationalParkRepository : INationalParkRepository
	{
		public NationalPark GetNationalPark(int nationalParkId)
		{
			throw new NotImplementedException();
		}

		public ICollection<NationalPark> GetNationalParks()
		{
			throw new NotImplementedException();
		}

		public bool NationalParkExists(string name)
		{
			throw new NotImplementedException();
		}

		public bool NationalParkExists(int id)
		{
			throw new NotImplementedException();
		}

		public bool Save()
		{
			throw new NotImplementedException();
		}

		public bool TryCreateNationalPark(NationalPark nationalPark)
		{
			throw new NotImplementedException();
		}

		public bool TryDeleteNationalPark(NationalPark nationalPark)
		{
			throw new NotImplementedException();
		}

		public bool TryUpdateNationalPark(NationalPark nationalPark)
		{
			throw new NotImplementedException();
		}
	}
}
