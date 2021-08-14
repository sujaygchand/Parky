using Microsoft.IdentityModel.Tokens;
using ParkyAPI.Data;
using ParkyAPI.Model;
using ParkyAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ParkyAPI.Repository
{
	public class UserRepository : IUserRepository
	{
		private readonly ApplicationDbContext _db;
		private readonly ApplicationSettings _appSettings;

		public UserRepository(ApplicationDbContext db, ApplicationSettings appSettings)
		{
			_db = db;
			_appSettings = appSettings;
		}
		public User Authenticate(string username, string password)
		{
			var user = _db?.Users.SingleOrDefault(k => k.Username == username && k.Password == password);

			// User not found
			if (user == null || _appSettings == null)
				return null;

			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[] {
					new Claim(ClaimTypes.Name, user.Role.ToString())
				}),
				Expires = DateTime.UtcNow.AddDays(7),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			user.Token = tokenHandler.WriteToken(token);
			user.Password = "";
			return user;
		}

		public bool IsUniqueUser(string username)
		{
			var user = _db?.Users.SingleOrDefault(k => k.Username == username);

			return user == null;
		}

		public User Register(string username, string password)
		{
			if (_db == null)
				return null;

			User user = new User()
			{
				Username = username,
				Password = password
			};

			_db.Users.Add(user);
			_db.SaveChanges();
			user.Password = string.Empty;
			return user;
		}
	}
}
