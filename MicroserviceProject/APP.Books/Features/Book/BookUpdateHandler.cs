using APP.Books.Context;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace APP.Books.Features.Book
{
    public class BookUpdateRequest : Request, IRequest<CommandResponse>
    {
        public string Name { get; set; }
        public short? NumberOfPages { get; set; }
        public DateTime PublishDate { get; set; }
        public decimal Price { get; set; }
        public bool IsTopSeller { get; set; }
        public int AuthorId { get; set; }
        public List<int> GenreIds { get; set; }
    }

    public class BookUpdateHandler : Handler, IRequestHandler<BookUpdateRequest, CommandResponse>
    {
        private readonly BooksDb _db;

        public BookUpdateHandler(BooksDb db) : base(System.Globalization.CultureInfo.CurrentCulture)
        {
            _db = db;
        }

        public async Task<CommandResponse> Handle(BookUpdateRequest request, CancellationToken cancellationToken)
        {
            var book = await _db.Books
                .Include(b => b.BookGenres)
                .FirstOrDefaultAsync(b => b.Id == request.Id);

            if (book == null)
                return Error("Book not found");

            book.Name = request.Name;
            book.NumberOfPages = request.NumberOfPages;
            book.PublishDate = request.PublishDate;
            book.Price = request.Price;
            book.IsTopSeller = request.IsTopSeller;
            book.AuthorId = request.AuthorId;

            // Update genres
            book.BookGenres.Clear();
            foreach (var genreId in request.GenreIds ?? new List<int>())
            {
                book.BookGenres.Add(new Domain.BookGenre { GenreId = genreId });
            }

            await _db.SaveChangesAsync();
            return Success("Book updated", book.Id);
        }
    }
}
