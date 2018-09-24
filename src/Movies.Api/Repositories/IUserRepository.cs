using Movies.Api.Models;
using System;
using System.Threading.Tasks;

namespace Movies.Api.Repositories
{
    public interface IUserRepository
    {
        Task<UserDomainModel> GetAsync(Guid userId);
    }
}
