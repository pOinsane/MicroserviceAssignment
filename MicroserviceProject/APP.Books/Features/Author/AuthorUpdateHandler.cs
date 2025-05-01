using APP.Books.Context;
using CORE.APP.Features;
using MediatR;

namespace APP.Books.Features.Author
{
    public class AuthorUpdateRequest : Request, IRequest<CommandResponse>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
    }

    public class AuthorUpdateHandler : Handler, IRequestHandler<AuthorUpdateRequest, CommandResponse>
    {
        private readonly BooksDb _db;

        public AuthorUpdateHandler(BooksDb db) : base(System.Globalization.CultureInfo.CurrentCulture)
        {
            _db = db;
        }

        public async Task<CommandResponse> Handle(AuthorUpdateRequest request, CancellationToken cancellationToken)
        {
            var author = await _db.Authors.FindAsync(request.Id);
            if (author == null)
                return Error("Author not found");

            author.Name = request.Name;
            author.Surname = request.Surname;

            await _db.SaveChangesAsync();

            return Success("Author updated successfully", author.Id);
        }
    }
}
