using Microsoft.EntityFrameworkCore;
using ParkyAPI.Data;
using ParkyAPI.Model;
using ParkyAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI.Repository
{
	public class TrailRepository : ITrailRepository
	{
		public readonly ApplicationDbContext _db;

		public TrailRepository(ApplicationDbContext db)
		{
			_db = db;
		}

		public bool TryCreateTrail(Trail trail)
		{
			if (_db?.Trails == null)
				return false;

			_db.Trails.Add(trail);
			return Save();
		}

		public bool TryDeleteTrail(Trail trail)
		{
			if (_db?.Trails == null)
				return false;

			_db.Trails.Remove(trail);
			return Save();
		}

		public Trail GetTrail(int trailId)
		{
			if (_db?.Trails == null)
				return null;

			return _db.Trails.Include(k => k.NationalPark).FirstOrDefault(k => k.Id == trailId);
		}

		public ICollection<Trail> GetTrails()
		{
			return _db.Trails.Include(k => k.NationalPark).OrderBy(k => k.Name).ToList();
		}

		public ICollection<Trail> GetTrailsInNationalPark(int npId)
		{
			return _db.Trails.Include(k => k.NationalPark).Where(k => k.NationalParkId == npId).ToList();
		}

		public bool Save()
		{
			return _db.SaveChanges() >= 0 ? true : false;
		}

		public bool TrailExists(string name)
		{
			return _db.Trails.Any(k => k.Name.ToLower().Trim() == name.ToLower().Trim());
		}

		public bool TrailExists(int id)
		{
			return _db.Trails.Any(k => k.Id == id);
		}

		public bool TryUpdateTrail(Trail trail)
		{
			var trailObj = _db.Trails.Update(trail);
			return Save();
		}
	}
}
