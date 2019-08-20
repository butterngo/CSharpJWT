namespace OAuthServer
{
    using CSharpJWT.Extensions;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using CSharpJWT.Domain;
    using Microsoft.EntityFrameworkCore.Design;
    using System.IO;

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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddDbContext<CSharpJWTContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("CSharpJWT")));

            services.AddCSharpIdentity<CSharpJWTContext>();

            CSharpJWTServerConfiguration.Init(Configuration);

            services.AddJWTAuthentication();

            services.AddTransient<SeedData>();

            services.AddCache(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, SeedData seedData)
        {
            //seedData.SeedUser();

            //seedData.SeedClient();

            //seedData.SeedRole();

            //seedData.SeedUserRole();

            app.AddJWTMiddleware();

            app.UseAuthentication();

            app.UseMvc();
        }

        public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CSharpJWTContext>
        {
            public CSharpJWTContext CreateDbContext(string[] args)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile($"appsettings.json", optional: true, reloadOnChange: true)
                    .Build();

                var builder = new DbContextOptionsBuilder<CSharpJWTContext>();

                var connectionString = configuration.GetConnectionString("CSharpJWT");

                builder.UseSqlServer(connectionString);

                return new CSharpJWTContext(builder.Options);
            }
        }
    }
}
