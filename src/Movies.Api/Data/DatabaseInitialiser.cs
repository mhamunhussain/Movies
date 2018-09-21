using Microsoft.EntityFrameworkCore;

namespace Movies.Api.Data
{
    public class DatabaseInitialiser : IDatabaseInitialiser
    {
        private readonly ApplicationContext _context;

        public DatabaseInitialiser(ApplicationContext context)
        {
            _context = context;
        }

        public void Initialise()
        {
            _context.Database.Migrate();
        }
    }
}
