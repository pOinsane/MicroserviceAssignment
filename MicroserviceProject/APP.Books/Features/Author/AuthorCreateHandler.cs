using APP.Books.Context;
using APP.Books.Domain;
using CORE.APP.Features;
using MediatR;

namespace APP.Books.Features.Author
{
    public class AuthorCreateRequest : Request, IRequest<CommandResponse>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
    }

    public class AuthorCreateHandler : Handler, IRequestHandler<AuthorCreateRequest, CommandResponse>
    {
        private readonly BooksDb _db;

        public AuthorCreateHandler(BooksDb db) : base(System.Globalization.CultureInfo.CurrentCulture)
        {
            _db = db;
        }

        public async Task<CommandResponse> Handle(AuthorCreateRequest request, CancellationToken cancellationToken)
        {
            var author = new Domain.Author
            {
                Name = request.Name,
                Surname = request.Surname
            };

            _db.Authors.Add(author);
            await _db.SaveChangesAsync();

            return Success("Author created successfully", author.Id);
        }
    }
}
