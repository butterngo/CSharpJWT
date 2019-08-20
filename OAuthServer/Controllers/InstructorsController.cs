namespace OAuthServer.Controllers
{
    using CSharpJWT.Domain;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Data.SqlClient;

    [Route("api/[controller]")]
    [ApiController]
    public class InstructorsController: ControllerBase
    {
                
        private readonly SeedData _seedData;

        private readonly CSharpJWTContext _context;

        private IConfiguration _configuration;

        public InstructorsController(SeedData seedData,
            CSharpJWTContext context,
            IConfiguration configuration)
        {
            _context = context;
            _seedData = seedData;
            _configuration = configuration;
        }

        [HttpGet("migrate-scheme")]
        public IActionResult MigrateScheme()
        {
            try
            {
                _context.Database.Migrate();

                CreateDistCache();

            }
            catch (Exception ex)
            {
                return BadRequest($"Message: {ex.Message} " +
                    $"/<br/> StackTrace: {ex.StackTrace} " +
                    $"/<br/> InnerException: {ex.InnerException}" +
                    $"/<br/> connectionstring: {_context.Database.GetDbConnection().ConnectionString}");
            }


            return Ok("Done");
        }


        [HttpGet("init-data")]
        public IActionResult initData()
        {
            
            try
            {
                _seedData.SeedClient();

                _seedData.SeedUser();

                _seedData.SeedRole();

                _seedData.SeedUserRole();

                _seedData.SeedUserClient();
            }
            catch (Exception ex)
            {
                return BadRequest($"Message: {ex.Message} /<br/> StackTrace: {ex.StackTrace} /<br/> InnerException: {ex.InnerException}");
            }


            return Ok("Done");
        }

        private void CreateDistCache()
        {
            string queryString = @"
                        USE [DistCache]
                        SET ANSI_NULLS ON
                        SET QUOTED_IDENTIFIER ON
                        CREATE TABLE [dbo].[TestCache](
	                        [Id] [nvarchar](449) NOT NULL,
	                        [Value] [varbinary](max) NOT NULL,
	                        [ExpiresAtTime] [datetimeoffset](7) NOT NULL,
	                        [SlidingExpirationInSeconds] [bigint] NULL,
	                        [AbsoluteExpiration] [datetimeoffset](7) NULL,
                        PRIMARY KEY CLUSTERED 
                        (
	                        [Id] ASC
                        )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                        ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]";

            string connectionString = _configuration.GetConnectionString("DistCache_ConnectionString");

            connectionString = connectionString.Replace("DistCache", "master");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "CREATE DATABASE DistCache";
                command.ExecuteNonQuery();
                command.CommandText = queryString;
                command.ExecuteNonQuery();
            }
        }
    }
}
