using APP.Books.Context;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace APP.Books.Features.Author
{
    public class AuthorQueryRequest : Request, IRequest<IQueryable<AuthorQueryResponse>> { }

    public class AuthorQueryResponse : QueryResponse
    {
        public string Name { get; set; }
        public string Surname { get; set; }
    }

    public class AuthorQueryHandler : Handler, IRequestHandler<AuthorQueryRequest, IQueryable<AuthorQueryResponse>>
    {
        private readonly BooksDb _db;

        public AuthorQueryHandler(BooksDb db) : base(System.Globalization.CultureInfo.CurrentCulture)
        {
            _db = db;
        }

        public Task<IQueryable<AuthorQueryResponse>> Handle(AuthorQueryRequest request, CancellationToken cancellationToken)
        {
            var query = _db.Authors.Select(a => new AuthorQueryResponse
            {
                Id = a.Id,
                Name = a.Name,
                Surname = a.Surname
            });

            return Task.FromResult(query);
        }
    }
}
