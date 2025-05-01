using APP.Books.Context;
using CORE.APP.Features;
using MediatR;

namespace APP.Books.Features.Genre
{
    public class GenreUpdateRequest : Request, IRequest<CommandResponse>
    {
        public string Name { get; set; }
    }

    public class GenreUpdateHandler : Handler, IRequestHandler<GenreUpdateRequest, CommandResponse>
    {
        private readonly BooksDb _db;

        public GenreUpdateHandler(BooksDb db) : base(System.Globalization.CultureInfo.CurrentCulture)
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

            return Success("Genre updated", genre.Id);
        }
    }
}
