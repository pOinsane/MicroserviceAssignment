using APP.Movies.Context;
using APP.Movies.Domain;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace APP.Movies.Features.Movie
{
    public class MovieUpdateRequest : Request, IRequest<CommandResponse>
    {
        public string Name { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public decimal TotalRevenue { get; set; }
        public int DirectorId { get; set; }
        public List<int> GenreIds { get; set; }
    }

    public class MovieUpdateHandler : Handler, IRequestHandler<MovieUpdateRequest, CommandResponse>
    {
        private readonly MoviesDb _db;

        public MovieUpdateHandler(MoviesDb db) : base(CultureInfo.CurrentCulture)
        {
            _db = db;
        }

        public async Task<CommandResponse> Handle(MovieUpdateRequest request, CancellationToken cancellationToken)
        {
            var movie = await _db.Movies
                .Include(m => m.MovieGenres)
                .FirstOrDefaultAsync(m => m.Id == request.Id);

            if (movie == null)
                return Error("Movie not found");

            movie.Name = request.Name;
            movie.ReleaseDate = request.ReleaseDate;
            movie.TotalRevenue = request.TotalRevenue;
            movie.DirectorId = request.DirectorId;

            movie.MovieGenres.Clear();
            foreach (var gid in request.GenreIds)
            {
                movie.MovieGenres.Add(new MovieGenre { GenreId = gid });
            }

            await _db.SaveChangesAsync();
            return Success("Movie updated successfully", movie.Id);
        }
    }
}
