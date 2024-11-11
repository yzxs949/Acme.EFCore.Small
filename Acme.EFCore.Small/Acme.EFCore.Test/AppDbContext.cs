using Acme.EFCore.Repositorys;
using Microsoft.EntityFrameworkCore;

namespace Acme.EFCore.Test
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
    }
}
