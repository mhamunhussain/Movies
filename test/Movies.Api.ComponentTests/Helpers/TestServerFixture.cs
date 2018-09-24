using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Movies.Api.Data;
using System;
using System.Net.Http;

namespace Movies.Api.ComponentTests.Helpers
{
    public class TestServerFixture : IDisposable
    {
        public HttpClient Client { get; }
        public ApplicationDbContext DbContext { get; }

        public TestServerFixture()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var options = optionsBuilder.UseInMemoryDatabase("TestMovies").Options;
            var context = new ApplicationDbContext(options);

            var server = new TestServer(new WebHostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddSingleton(context);
                })
                .UseEnvironment("Testing")
                .UseStartup<Startup>());

            DbContext = context;
            Client = server.CreateClient();
        }

        public void Dispose() =>
            Client.Dispose();
    }
}


