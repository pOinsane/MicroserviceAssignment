using APP.Movies.Context;
using APP.Movies.Domain;
using CORE.APP.Features;
using MediatR;
using System.Globalization;

namespace APP.Movies.Features.Genre
{
    public class GenreUpdateRequest : Request, IRequest<CommandResponse>
    {
        public string Name { get; set; }
    }

    public class GenreUpdateHandler : Handler, IRequestHandler<GenreUpdateRequest, CommandResponse>
    {
        private readonly MoviesDb _db;

        public GenreUpdateHandler(MoviesDb db) : base(CultureInfo.CurrentCulture)
        {
            _db = db;
        }

        public async Task<CommandResponse> Handle(GenreUpdateRequest request, CancellationToken cancellationToken)
        {
            var genre = await _db.Genres.FindAsync(request.Id);
            if (genre == null)
                return Error("Genre not found");

            genre.Name = request.Name;
            await _db.SaveChangesAsync();

            return Success("Genre updated successfully", genre.Id);
        }
    }
}
