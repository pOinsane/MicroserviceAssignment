using APP.Books.Context;
using APP.Books.Domain;
using CORE.APP.Features;
using MediatR;

namespace APP.Books.Features.Book
{
    public class BookCreateRequest : Request, IRequest<CommandResponse>
    {
        public string Name { get; set; }
        public short? NumberOfPages { get; set; }
        public DateTime PublishDate { get; set; }
        public decimal Price { get; set; }
        public bool IsTopSeller { get; set; }
        public int AuthorId { get; set; }
        public List<int> GenreIds { get; set; }
    }

    public class BookCreateHandler : Handler, IRequestHandler<BookCreateRequest, CommandResponse>
    {
        private readonly BooksDb _db;

        public BookCreateHandler(BooksDb db) : base(System.Globalization.CultureInfo.CurrentCulture)
        {
            _db = db;
        }

        public async Task<CommandResponse> Handle(BookCreateRequest request, CancellationToken cancellationToken)
        {
            var book = new Domain.Book
            {
                Name = request.Name,
                NumberOfPages = request.NumberOfPages,
                PublishDate = request.PublishDate,
                Price = request.Price,
                IsTopSeller = request.IsTopSeller,
                AuthorId = request.AuthorId,
                BookGenres = request.GenreIds?.Select(id => new BookGenre { GenreId = id }).ToList()
            };

            _db.Books.Add(book);
            await _db.SaveChangesAsync();

            return Success("Book created", book.Id);
        }
    }
}
