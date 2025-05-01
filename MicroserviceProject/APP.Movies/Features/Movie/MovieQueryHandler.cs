using APP.Movies.Context;
using APP.Movies.Domain;
using CORE.APP.Features;
using MediatR;
using System.Globalization;
using Microsoft.EntityFrameworkCore;

namespace APP.Movies.Features.Movie
{
    public class MovieQueryRequest : Request, IRequest<IQueryable<MovieQueryResponse>>
    {
    }

    public class MovieQueryResponse : QueryResponse
    {
        public string Name { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public decimal TotalRevenue { get; set; }
        public int DirectorId { get; set; }
        public List<int> GenreIds { get; set; }
    }

    public class MovieQueryHandler : Handler, IRequestHandler<MovieQueryRequest, IQueryable<MovieQueryResponse>>
    {
        private readonly MoviesDb _db;

        public MovieQueryHandler(MoviesDb db) : base(CultureInfo.CurrentCulture)
        {
            _db = db;
        }

        public Task<IQueryable<MovieQueryResponse>> Handle(MovieQueryRequest request, CancellationToken cancellationToken)
        {
            var query = _db.Movies
                .Include(m => m.MovieGenres)
                .Select(m => new MovieQueryResponse
                {
                    Id = m.Id,
                    Name = m.Name,
                    ReleaseDate = m.ReleaseDate,
                    TotalRevenue = m.TotalRevenue,
                    DirectorId = m.DirectorId,
                    GenreIds = m.MovieGenres.Select(mg => mg.GenreId).ToList()
                });

            return Task.FromResult(query);
        }
    }
}
