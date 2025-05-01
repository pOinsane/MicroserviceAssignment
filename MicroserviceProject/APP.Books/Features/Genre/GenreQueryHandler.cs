using APP.Books.Context;
using CORE.APP.Features;
using MediatR;

namespace APP.Books.Features.Genre
{
    public class GenreQueryRequest : Request, IRequest<IQueryable<GenreQueryResponse>> { }

    public class GenreQueryResponse : QueryResponse
    {
        public string Name { get; set; }
    }

    public class GenreQueryHandler : Handler, IRequestHandler<GenreQueryRequest, IQueryable<GenreQueryResponse>>
    {
        private readonly BooksDb _db;

        public GenreQueryHandler(BooksDb db) : base(System.Globalization.CultureInfo.CurrentCulture)
        {
            _db = db;
        }

        public Task<IQueryable<GenreQueryResponse>> Handle(GenreQueryRequest request, CancellationToken cancellationToken)
        {
            var query = _db.Genres.Select(g => new GenreQueryResponse
            {
                Id = g.Id,
                Name = g.Name
            });

            return Task.FromResult(query);
        }
    }
}
