using APP.Books.Context;
using APP.Books.Domain;
using CORE.APP.Features;
using MediatR;

namespace APP.Books.Features.Genre
{
    public class GenreCreateRequest : Request, IRequest<CommandResponse>
    {
        public string Name { get; set; }
    }

    public class GenreCreateHandler : Handler, IRequestHandler<GenreCreateRequest, CommandResponse>
    {
        private readonly BooksDb _db;

        public GenreCreateHandler(BooksDb db) : base(System.Globalization.CultureInfo.CurrentCulture)
        {
            _db = db;
        }

        public async Task<CommandResponse> Handle(GenreCreateRequest request, CancellationToken cancellationToken)
        {
            var genre = new Domain.Genre
            {
                Name = request.Name
            };

            _db.Genres.Add(genre);
            await _db.SaveChangesAsync();

            return Success("Genre created", genre.Id);
        }
    }
}
