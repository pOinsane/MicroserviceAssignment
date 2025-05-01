using APP.Movies.Context;
using APP.Movies.Domain;
using CORE.APP.Features;
using MediatR;
using System.Globalization;

namespace APP.Movies.Features.Director
{
    public class DirectorCreateRequest : Request, IRequest<CommandResponse>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool IsRetired { get; set; }
    }

    public class DirectorCreateHandler : Handler, IRequestHandler<DirectorCreateRequest, CommandResponse>
    {
        private readonly MoviesDb _db;

        public DirectorCreateHandler(MoviesDb db) : base(CultureInfo.CurrentCulture)
        {
            _db = db;
        }

        public async Task<CommandResponse> Handle(DirectorCreateRequest request, CancellationToken cancellationToken)
        {
            var director = new Domain.Director
            {
                Name = request.Name,
                Surname = request.Surname,
                IsRetired = request.IsRetired
            };

            _db.Directors.Add(director);
            await _db.SaveChangesAsync();

            return Success("Director created successfully", director.Id);
        }
    }
}
