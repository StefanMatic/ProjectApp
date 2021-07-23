using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Logging.Debug;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;
using Interfaces.Service;
using Interfaces.DAL;
using DAL;
using Service;
using System.Net.Http.Headers;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Core;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.ApplicationInsights.AspNetCore;

using WebAPI.APIConfiguration;




namespace WebAPI
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
            //services.AddMemoryCache();
            //services.AddTransient<IProjectDAL, ProjectDAL>();
            //services.AddTransient<IProjectService, ProjectService>();
            //services.AddTransient<IWorkItemDAL, WorkItemsDAL>();
            //services.AddTransient<IWorkItemService, WorkItemService>(); 
            //services.AddTransient<IProjectWorkItemsDAL, ProjectWorkItemsDAL>();
            //services.AddTransient<IBlobFileDAL, BlobFileDAL>();
            //services.AddTransient<IProjectRepoService, ProjectRepoService>();
            //services.AddTransient<IProjectFileDAL, ProjectFileDAL>();
            //services.AddTransient<IProjectFileHistoryDAL, ProjectFileHistoryDAL>();
            //services.AddTransient<IUserDAL, UserDAL>();
            //services.AddTransient<UserService, UserService>();

            services.AddAndConfigureTransient();

            //var _storageAccountConnectionString = Configuration["webapi-storageaccount"];
            //var _dbConnectionString = Configuration["webapi-db"];
            //services.AddScoped(_storageAccount => CloudStorageAccount.Parse(_storageAccountConnectionString));
            //services.AddScoped(_blobServiceClient => new BlobServiceClient(_storageAccountConnectionString));
            //services.AddScoped(_sqlConnection => new SqlConnection(_dbConnectionString));

            services.AddAndConfigureScoped(Configuration);

            //var _keyString = Configuration["webapi-secretkey"];
            //var _secretKey = Encoding.ASCII.GetBytes("rQWZdh7?w2cz243gpmJJJchxqtPcRcAG");
            //services.AddAuthentication(x =>
            //{
            //    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //    .AddJwtBearer(x =>
            //    {
            //        x.RequireHttpsMetadata = false;
            //        x.SaveToken = true;
            //        x.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            ValidateIssuerSigningKey = true,
            //            IssuerSigningKey = new SymmetricSecurityKey(_secretKey),
            //            ValidateIssuer = false,
            //            ValidateAudience = false
            //        };
            //    });

            services.AddAndConfigureAuthentication(Configuration);


            services.AddSingleton<IConfiguration>(Configuration);

            services.AddApplicationInsightsTelemetry(Configuration["APPINSIGHTS_CONNECTIONSTRING"]);

            //services.AddSwaggerGen(c =>
            //{
            //    //First we define the security scheme
            //    c.AddSecurityDefinition("Bearer", //Name the security scheme
            //        new OpenApiSecurityScheme
            //        {
            //            Description = "JWT Authorization header using the Bearer scheme.",
            //            Type = SecuritySchemeType.Http, //We set the scheme type to http since we're using bearer authentication
            //            Scheme = "bearer" //The name of the HTTP Authorization scheme to be used in the Authorization header. In this case "bearer".
            //        });

            //    c.AddSecurityRequirement(new OpenApiSecurityRequirement{
            //        {
            //            new OpenApiSecurityScheme{
            //                Reference = new OpenApiReference{
            //                    Id = "Bearer", //The name of the previously defined security scheme.
            //                    Type = ReferenceType.SecurityScheme
            //                }
            //            },new List<string>()
            //        }
            //    });
            //});

            services.AddAndConfigureSwagger();
            // Add swagger service
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Swagger
            app.UseSwagger();

            app.UseSwaggerUI( c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
