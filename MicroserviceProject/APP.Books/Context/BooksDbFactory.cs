using Microsoft.EntityFrameworkCore;

namespace APP.Books.Context
{
    public class BooksDbFactory : IDbContextFactory<BooksDb>
    {
        private readonly DbContextOptions<BooksDb> _options;

        public BooksDbFactory(DbContextOptions<BooksDb> options)
        {
            _options = options;
        }

        public BooksDb CreateDbContext()
        {
            return new BooksDb(_options);
        }
    }
}
