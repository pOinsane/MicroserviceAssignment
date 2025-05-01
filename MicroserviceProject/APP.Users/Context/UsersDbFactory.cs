using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace APP.Users.Context
{
    public class UsersDbFactory : IDesignTimeDbContextFactory<UsersDb>
    {
        public UsersDb CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<UsersDb>();
            optionsBuilder.UseSqlServer("Server=localhost;Database=UsersDb;User Id=sa;Password=sa;TrustServerCertificate=True;");
            return new UsersDb(optionsBuilder.Options);
        }
    }

}
