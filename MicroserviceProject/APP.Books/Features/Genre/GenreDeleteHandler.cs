using APP.Books.Context;
using CORE.APP.Features;
using MediatR;

namespace APP.Books.Features.Genre
{
    public class GenreDeleteRequest : Request, IRequest<CommandResponse>
    {
    }

    public class GenreDeleteHandler : Handler, IRequestHandler<GenreDeleteRequest, CommandResponse>
    {
        private readonly BooksDb _db;

        public GenreDeleteHandler(BooksDb db) : base(System.Globalization.CultureInfo.CurrentCulture)
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

            return Success("Genre deleted", genre.Id);
        }
    }
}
