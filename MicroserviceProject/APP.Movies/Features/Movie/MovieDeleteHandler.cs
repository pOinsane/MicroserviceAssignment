using APP.Movies.Context;
using APP.Movies.Domain;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace APP.Movies.Features.Movie
{
    public class MovieDeleteRequest : Request, IRequest<CommandResponse>
    {
    }

    public class MovieDeleteHandler : Handler, IRequestHandler<MovieDeleteRequest, CommandResponse>
    {
        private readonly MoviesDb _db;

        public MovieDeleteHandler(MoviesDb db) : base(CultureInfo.CurrentCulture)
        {
            _db = db;
        }

        public async Task<CommandResponse> Handle(MovieDeleteRequest request, CancellationToken cancellationToken)
        {
            var movie = await _db.Movies
                .Include(m => m.MovieGenres)
                .FirstOrDefaultAsync(m => m.Id == request.Id);

            if (movie == null)
                return Error("Movie not found");

            _db.Movies.Remove(movie);
            await _db.SaveChangesAsync();

            return Success("Movie deleted successfully", movie.Id);
        }
    }
}
