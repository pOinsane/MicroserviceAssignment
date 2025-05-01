using APP.Books.Context;
using CORE.APP.Features;
using MediatR;

namespace APP.Books.Features.Author
{
    public class AuthorDeleteRequest : Request, IRequest<CommandResponse>
    {
    }

    public class AuthorDeleteHandler : Handler, IRequestHandler<AuthorDeleteRequest, CommandResponse>
    {
        private readonly BooksDb _db;

        public AuthorDeleteHandler(BooksDb db) : base(System.Globalization.CultureInfo.CurrentCulture)
        {
            _db = db;
        }

        public async Task<CommandResponse> Handle(AuthorDeleteRequest request, CancellationToken cancellationToken)
        {
            var author = await _db.Authors.FindAsync(request.Id);
            if (author == null)
                return Error("Author not found");

            _db.Authors.Remove(author);
            await _db.SaveChangesAsync();

            return Success("Author deleted successfully", author.Id);
        }
    }
}
