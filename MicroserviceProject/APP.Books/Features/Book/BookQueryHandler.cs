using APP.Books.Context;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace APP.Books.Features.Book
{
    public class BookQueryRequest : Request, IRequest<IQueryable<BookQueryResponse>> { }

    public class BookQueryResponse : QueryResponse
    {
        public string Name { get; set; }
        public short? NumberOfPages { get; set; }
        public DateTime PublishDate { get; set; }
        public decimal Price { get; set; }
        public bool IsTopSeller { get; set; }
        public int AuthorId { get; set; }
        public List<int> GenreIds { get; set; }
    }

    public class BookQueryHandler : Handler, IRequestHandler<BookQueryRequest, IQueryable<BookQueryResponse>>
    {
        private readonly BooksDb _db;

        public BookQueryHandler(BooksDb db) : base(System.Globalization.CultureInfo.CurrentCulture)
        {
            _db = db;
        }

        public Task<IQueryable<BookQueryResponse>> Handle(BookQueryRequest request, CancellationToken cancellationToken)
        {
            var query = _db.Books
                .Include(b => b.BookGenres)
                .Select(b => new BookQueryResponse
                {
                    Id = b.Id,
                    Name = b.Name,
                    NumberOfPages = b.NumberOfPages,
                    PublishDate = b.PublishDate,
                    Price = b.Price,
                    IsTopSeller = b.IsTopSeller,
                    AuthorId = b.AuthorId,
                    GenreIds = b.BookGenres.Select(bg => bg.GenreId).ToList()
                });

            return Task.FromResult(query);
        }
    }
}
