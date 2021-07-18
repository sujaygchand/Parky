using ParkyAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI.Repository.IRepository
{
	public interface ITrailRepository
	{
		ICollection<Trail> GetTrails();
		ICollection<Trail> GetTrailsInNationalPark(int npId);
		Trail GetTrail(int trailId);
		bool TrailExists(string name);
		bool TrailExists(int id);
		bool TryCreateTrail(Trail trail);
		bool TryUpdateTrail(Trail trail);
		bool TryDeleteTrail(Trail trail);
		bool Save();
	}
}
