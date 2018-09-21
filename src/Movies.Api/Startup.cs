using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Movies.Api.Data;
using Movies.Api.Models;
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
            services.AddAutoMapper();

            if (Environment.IsDevelopment())
                services.AddDbContext<ApplicationContext>(options =>
                    options.UseSqlite("Data Source=Movies.db"));
            else
                services.AddDbContext<ApplicationContext>(options =>
                    options.UseSqlServer("connection string from config goes here"));

            services.AddTransient<IValidator<MovieFilterCriteria>, MovieRequestValidator>();
            services.AddTransient<IMovieRepository, MovieRepository>();
            services.AddTransient<IDatabaseInitialiser, DatabaseInitialiser>();

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IDatabaseInitialiser databaseInitialiser)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            databaseInitialiser.Initialise();
            app.UseMvc();
        }
    }
}
