using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Movies.Api.Data;
using Movies.Api.Repositories;
using Movies.Api.Validators;

namespace Movies.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            if (Environment.IsDevelopment())
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlite("DataSource=Movies.db"));

            services.AddTransient<IMovieRequestValidator, MovieRequestValidator>();
            services.AddTransient<IMovieRepository, MovieRepository>();
            services.AddTransient<IMovieRatingRepository, MovieRatingRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IDatabaseInitialiser, DatabaseInitialiser>();

            services.AddAutoMapper();
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IDatabaseInitialiser databaseInitialiser)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                databaseInitialiser.Initialise();
            }

            app.UseMvc();
        }
    }
}
