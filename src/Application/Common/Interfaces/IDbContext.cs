using Microsoft.EntityFrameworkCore;
using Onion.Domain.Entities.Identity;

namespace Onion.Application.Common.Interfaces;

public interface IDbContext
{
    DbSet<User> Users { get; }

    int Commit();
}
