using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;

using Interfaces.Service;
using Interfaces.DAL;
using DAL;
using Service;


using Azure.Storage.Blobs;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Data.SqlClient;


using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;


using Microsoft.OpenApi.Models;


namespace WebAPI.APIConfiguration
{
    public static class ServiceCollectionExtensions
    {
        
        public static IServiceCollection AddAndConfigureAuthentication(this IServiceCollection _service, IConfiguration configuration)
        {
            var _keyString = configuration["webapi-secretkey"];
            var _secretKey = Encoding.ASCII.GetBytes("rQWZdh7?w2cz243gpmJJJchxqtPcRcAG");

            _service.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(_secretKey),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            return _service;
        }

        public static IServiceCollection AddAndConfigureTransient(this IServiceCollection _service)
        {

            _service.AddTransient<IProjectDAL, ProjectDAL>();
            _service.AddTransient<IProjectService, ProjectService>();
            _service.AddTransient<IWorkItemDAL, WorkItemsDAL>();
            _service.AddTransient<IWorkItemService, WorkItemService>();
            _service.AddTransient<IProjectWorkItemsDAL, ProjectWorkItemsDAL>();
            _service.AddTransient<IBlobFileDAL, BlobFileDAL>();
            _service.AddTransient<IProjectRepoService, ProjectRepoService>();
            _service.AddTransient<IProjectFileDAL, ProjectFileDAL>();
            _service.AddTransient<IProjectFileHistoryDAL, ProjectFileHistoryDAL>();

            _service.AddTransient<IUserDAL, UserDAL>();
            _service.AddTransient<UserService, UserService>(); //Need to add interface

            return _service;
        }

        public static IServiceCollection AddAndConfigureScoped(this IServiceCollection _service, IConfiguration configuration)
        {
            var _storageAccountConnectionString = configuration["webapi-storageaccount"];
            var _dbConnectionString = configuration["webapi-db"];
            _service.AddScoped(_storageAccount => CloudStorageAccount.Parse(_storageAccountConnectionString));
            _service.AddScoped(_blobServiceClient => new BlobServiceClient(_storageAccountConnectionString));
            _service.AddScoped(_sqlConnection => new SqlConnection(_dbConnectionString));

            return _service;
        }

        public static IServiceCollection AddAndConfigureSingleton(this IServiceCollection _service)
        {
            return _service;
        }

        public static IServiceCollection AddAndConfigureSwagger(this IServiceCollection _service) 
        {
            _service.AddSwaggerGen(c =>
            {
                //First we define the security scheme
                c.AddSecurityDefinition("Bearer", //Name the security scheme
                    new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme.",
                        Type = SecuritySchemeType.Http, //We set the scheme type to http since we're using bearer authentication
                        In = ParameterLocation.Header,
                        Scheme = "bearer", //The name of the HTTP Authorization scheme to be used in the Authorization header. In this case "bearer".
                        BearerFormat = "JWT",
                        Reference = new OpenApiReference
                        {
                            Id = JwtBearerDefaults.AuthenticationScheme,
                            Type = ReferenceType.SecurityScheme
                        }
                    });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement{
                    {
                        new OpenApiSecurityScheme{
                            Reference = new OpenApiReference{
                                Id = "Bearer", //The name of the previously defined security scheme.
                                Type = ReferenceType.SecurityScheme
                            }
                        },new List<string>()
                    }
                });
                //c.OperationFilter<SwaggerFilter>();
        
            });
            return _service;
        }
    }

}
