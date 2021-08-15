using Newtonsoft.Json;
using ParkyWeb.Models;
using ParkyWeb.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ParkyWeb.Repository
{
	public class AccountRepository : Repository<User>, IAccountRepository
	{
		public AccountRepository(IHttpClientFactory clientFactory) : base(clientFactory)
		{

		}

		public async Task<User> LoginAsync(string url, User userToCreate)
		{
			if (_clientFactory == null)
				return null;

			if (userToCreate == null)
				return new User();

			var request = new HttpRequestMessage(HttpMethod.Post, url);

			request.Content = new StringContent(JsonConvert.SerializeObject(userToCreate), Encoding.UTF8, "application/json");

			var client = _clientFactory.CreateClient();
			HttpResponseMessage response = await client.SendAsync(request);

			if (response.StatusCode != System.Net.HttpStatusCode.OK)
				return new User();

			var jsonString = await response.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<User>(jsonString);
		}

		public async Task<bool> RegisterAsync(string url, User userToCreate)
		{
			if (_clientFactory == null || userToCreate == null)
				return false;

			var request = new HttpRequestMessage(HttpMethod.Post, url);

			request.Content = new StringContent(JsonConvert.SerializeObject(userToCreate), Encoding.UTF8, "application/json");

			var client = _clientFactory.CreateClient();
			HttpResponseMessage response = await client.SendAsync(request);

			return response.StatusCode == System.Net.HttpStatusCode.OK;
		}
	}
}
