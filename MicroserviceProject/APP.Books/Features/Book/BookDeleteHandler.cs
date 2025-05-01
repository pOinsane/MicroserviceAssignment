using APP.Books.Context;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace APP.Books.Features.Book
{
    public class BookDeleteRequest : Request, IRequest<CommandResponse> { }

    public class BookDeleteHandler : Handler, IRequestHandler<BookDeleteRequest, CommandResponse>
    {
        private readonly BooksDb _db;

        public BookDeleteHandler(BooksDb db) : base(System.Globalization.CultureInfo.CurrentCulture)
        {
            _db = db;
        }

        public async Task<CommandResponse> Handle(BookDeleteRequest request, CancellationToken cancellationToken)
        {
            var book = await _db.Books
                .Include(b => b.BookGenres)
                .FirstOrDefaultAsync(b => b.Id == request.Id);

            if (book == null)
                return Error("Book not found");

            _db.Books.Remove(book);
            await _db.SaveChangesAsync();

            return Success("Book deleted", book.Id);
        }
    }
}
