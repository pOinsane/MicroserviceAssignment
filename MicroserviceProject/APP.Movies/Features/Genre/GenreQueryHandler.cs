using APP.Movies.Context;
using APP.Movies.Domain;
using CORE.APP.Features;
using MediatR;
using System.Globalization;
using Microsoft.EntityFrameworkCore;

namespace APP.Movies.Features.Genre
{
    public class GenreQueryRequest : Request, IRequest<IQueryable<GenreQueryResponse>>
    {
    }

    public class GenreQueryResponse : QueryResponse
    {
        public string Name { get; set; }
    }

    public class GenreQueryHandler : Handler, IRequestHandler<GenreQueryRequest, IQueryable<GenreQueryResponse>>
    {
        private readonly MoviesDb _db;

        public GenreQueryHandler(MoviesDb db) : base(CultureInfo.CurrentCulture)
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
