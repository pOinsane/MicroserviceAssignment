using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace APP.Movies.Context
{
    public class MoviesDbFactory : IDesignTimeDbContextFactory<MoviesDb>
    {
        public MoviesDb CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MoviesDb>();
            optionsBuilder.UseSqlServer("Server=localhost;Database=MoviesDb;User Id=sa;Password=sa;TrustServerCertificate=True;");
            return new MoviesDb(optionsBuilder.Options);
        }
    }
}