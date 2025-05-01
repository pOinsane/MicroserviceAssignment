using APP.Movies.Context;
using APP.Movies.Domain;
using CORE.APP.Features;
using MediatR;
using System.Globalization;

namespace APP.Movies.Features.Genre
{
    public class GenreDeleteRequest : Request, IRequest<CommandResponse>
    {
    }

    public class GenreDeleteHandler : Handler, IRequestHandler<GenreDeleteRequest, CommandResponse>
    {
        private readonly MoviesDb _db;

        public GenreDeleteHandler(MoviesDb db) : base(CultureInfo.CurrentCulture)
        {
            _db = db;
        }

        public async Task<CommandResponse> Handle(GenreDeleteRequest request, CancellationToken cancellationToken)
        {
            var genre = await _db.Genres.FindAsync(request.Id);
            if (genre == null)
                return Error("Genre not found");

            _db.Genres.Remove(genre);
            await _db.SaveChangesAsync();

            return Success("Genre deleted successfully", genre.Id);
        }
    }
}
