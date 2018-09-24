using Microsoft.EntityFrameworkCore;
using Movies.Api.Data;
using Movies.Api.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserDomainModel> GetAsync(Guid userId)
        {
            var user = await _context.Users
                .Where(u => u.Id == userId)
                .SingleOrDefaultAsync();

            if (user == null)
                return null;

            return new UserDomainModel
            {
                Id = user.Id,
                FullName = user.FullName
            };
        }
    }
}
