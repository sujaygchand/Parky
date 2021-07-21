using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ParkyAPI.Data;
using ParkyAPI.ParkyMapper;
using ParkyAPI.Repository;
using ParkyAPI.Repository.IRepository;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ParkyAPI
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

			services.AddScoped<INationalParkRepository, NationalParkRepository>();
			services.AddScoped<ITrailRepository, TrailRepository>();

			services.AddAutoMapper(typeof(ParkyMappings));
			services.AddApiVersioning(options =>
			{
				options.AssumeDefaultVersionWhenUnspecified = true;
				options.DefaultApiVersion = new ApiVersion(1, 0);
				options.ReportApiVersions = true;
			});

			services.AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'VVV");
			services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
			services.AddSwaggerGen();

			/*			services.AddSwaggerGen(options =>
						{
							options.SwaggerDoc("ParkyOpenAPISpec",
								new Microsoft.OpenApi.Models.OpenApiInfo()
								{
									Title = "Parky API",
									Version = "1",
									Description = "Sujay Parky API",
									Contact = new Microsoft.OpenApi.Models.OpenApiContact()
									{
										Email = "sujaygchand@gmail.com",
										Name = "Sujay Chand",
										Url = new Uri("https://sujaygchand.github.io/")
									},
									License = new Microsoft.OpenApi.Models.OpenApiLicense()
									{
										Name = "La Licence",
										Url = new Uri("https://youtu.be/dDagv6SA8nw?t=117")
									}
								});
						});*/

			services.AddControllers();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();
			app.UseSwagger();

			app.UseSwaggerUI(options =>
			{
				foreach(var desc in provider.ApiVersionDescriptions)
				{
					options.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json", 
						desc.GroupName.ToUpperInvariant());
					options.RoutePrefix = "";
				}
			});
			/*app.UseSwaggerUI(options =>
			{
				options.SwaggerEndpoint("/swagger/ParkyOpenAPISpec/swagger.json", "Parky API");
				options.RoutePrefix = "";
			});*/

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
