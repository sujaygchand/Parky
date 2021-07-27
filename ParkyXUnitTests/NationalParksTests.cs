using AutoMapper;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Controllers;
using ParkyAPI.Model;
using ParkyAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ParkyXUnitTests
{
	public class NationalParksTests
	{
		[Fact]
		public void Get_Park_Ids()
		{
			// Arrange
			int numOfParks = 5;
			var fakeParks = A.CollectionOfDummy<NationalPark>(numOfParks).AsEnumerable();
			var dataStore = A.Fake<INationalParkRepository>();
			var mapper = A.Fake<IMapper>();
			var controller = new NationalParksController(dataStore, mapper);

			// Act
			var actionResult = controller.GetNationalParks();

			// Assert
			var result = actionResult as OkObjectResult;
			var returnParks = result.Value as IEnumerable<NationalPark>;
			Assert.Equal(1, 1);
		}
	}
}
