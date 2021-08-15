﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyWeb.Utilities
{
	public static class StaticDetails
	{
		public static string APIBaseUrl = "https://localhost:44358/";
		public static string NationalParkAPIPath = APIBaseUrl + "api/v1/nationalparks/";
		public static string TrailAPIPath = APIBaseUrl + "api/v1/trails/";
		public static string AccountAPIPath = APIBaseUrl + "api/v1/users/";
	}
}
