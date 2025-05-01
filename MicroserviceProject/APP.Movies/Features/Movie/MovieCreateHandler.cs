using APP.Movies.Context;
using APP.Movies.Domain;
using CORE.APP.Features;
using MediatR;
using System.Globalization;

namespace APP.Movies.Features.Movie
{
    public class MovieCreateRequest : Request, IRequest<CommandResponse>
    {
        public string Name { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public decimal TotalRevenue { get; set; }
        public int DirectorId { get; set; }
        public List<int> GenreIds { get; set; }
    }

    public class MovieCreateHandler : Handler, IRequestHandler<MovieCreateRequest, CommandResponse>
    {
        private readonly MoviesDb _db;

        public MovieCreateHandler(MoviesDb db) : base(CultureInfo.CurrentCulture)
        {
            _db = db;
        }

        public async Task<CommandResponse> Handle(MovieCreateRequest request, CancellationToken cancellationToken)
        {
            var movie = new Domain.Movie
            {
                Name = request.Name,
                ReleaseDate = request.ReleaseDate,
                TotalRevenue = request.TotalRevenue,
                DirectorId = request.DirectorId,
                MovieGenres = request.GenreIds.Select(gid => new MovieGenre { GenreId = gid }).ToList()
            };

            _db.Movies.Add(movie);
            await _db.SaveChangesAsync();

            return Success("Movie created successfully", movie.Id);
        }
    }
}
