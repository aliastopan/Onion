using Microsoft.EntityFrameworkCore;
using Onion.Domain.Entities.Identity;

namespace Onion.Application.Common.Extensions.Repositories;

public static class UserRepositoryExtensions
{
    public static User? Search(this DbSet<User> users, string username)
    {
        return users.FirstOrDefault(x => x.Username == username);
    }

    public static User? SearchByEmail(this DbSet<User> users, string email)
    {
        return users.FirstOrDefault(x => x.Email == email);
    }
}
