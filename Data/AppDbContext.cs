using Microsoft.EntityFrameworkCore;
using time_tracker.Models;

namespace time_tracker.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
}
