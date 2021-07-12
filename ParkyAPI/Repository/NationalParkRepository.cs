using ParkyAPI.Data;
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
		private readonly ApplicationDbContext _db;
		public NationalParkRepository(ApplicationDbContext db)
		{
			_db = db;
		}

		public NationalPark GetNationalPark(int nationalParkId)
		{
			return _db.NationalParks.FirstOrDefault(k => k.Id == nationalParkId);
		}

		public ICollection<NationalPark> GetNationalParks()
		{
			return _db.NationalParks.OrderBy(k => k.Name).ToList();
		}

		public bool NationalParkExists(string name)
		{
			return _db.NationalParks.Any(k => k.Name.ToLower().Trim() == name.ToLower().Trim());
		}

		public bool NationalParkExists(int id)
		{
			return _db.NationalParks.Any(k => k.Id == id);
		}

		public bool Save()
		{
			return _db.SaveChanges() >= 0 ? true : false;
		}

		public bool TryCreateNationalPark(NationalPark nationalPark)
		{
			_db.NationalParks.Add(nationalPark);
			return Save();
		}

		public bool TryDeleteNationalPark(NationalPark nationalPark)
		{
			_db.NationalParks.Remove(nationalPark);
			return Save();
		}

		public bool TryUpdateNationalPark(NationalPark nationalPark)
		{
			_db.NationalParks.Update(nationalPark);
			return Save();
		}
	}
}
