using APP.Movies.Context;
using APP.Movies.Domain;
using CORE.APP.Features;
using MediatR;
using System.Globalization;
using Microsoft.EntityFrameworkCore;

namespace APP.Movies.Features.Director
{
    public class DirectorQueryRequest : Request, IRequest<IQueryable<DirectorQueryResponse>>
    {
    }

    public class DirectorQueryResponse : QueryResponse
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool IsRetired { get; set; }
    }

    public class DirectorQueryHandler : Handler, IRequestHandler<DirectorQueryRequest, IQueryable<DirectorQueryResponse>>
    {
        private readonly MoviesDb _db;

        public DirectorQueryHandler(MoviesDb db) : base(CultureInfo.CurrentCulture)
        {
            _db = db;
        }

        public Task<IQueryable<DirectorQueryResponse>> Handle(DirectorQueryRequest request, CancellationToken cancellationToken)
        {
            var query = _db.Directors.Select(d => new DirectorQueryResponse
            {
                Id = d.Id,
                Name = d.Name,
                Surname = d.Surname,
                IsRetired = d.IsRetired
            });

            return Task.FromResult(query);
        }
    }
}
