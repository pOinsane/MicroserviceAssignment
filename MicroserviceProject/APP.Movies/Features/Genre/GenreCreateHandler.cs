using APP.Movies.Context;
using APP.Movies.Domain;
using CORE.APP.Features;
using MediatR;
using System.Globalization;

namespace APP.Movies.Features.Genre
{
    public class GenreCreateRequest : Request, IRequest<CommandResponse>
    {
        public string Name { get; set; }
    }

    public class GenreCreateHandler : Handler, IRequestHandler<GenreCreateRequest, CommandResponse>
    {
        private readonly MoviesDb _db;

        public GenreCreateHandler(MoviesDb db) : base(CultureInfo.CurrentCulture)
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

            return Success("Genre created successfully", genre.Id);
        }
    }
}
