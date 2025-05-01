using APP.Movies.Context;
using APP.Movies.Domain;
using CORE.APP.Features;
using MediatR;
using System.Globalization;

namespace APP.Movies.Features.Director
{
    public class DirectorDeleteRequest : Request, IRequest<CommandResponse>
    {
    }

    public class DirectorDeleteHandler : Handler, IRequestHandler<DirectorDeleteRequest, CommandResponse>
    {
        private readonly MoviesDb _db;

        public DirectorDeleteHandler(MoviesDb db) : base(CultureInfo.CurrentCulture)
        {
            _db = db;
        }

        public async Task<CommandResponse> Handle(DirectorDeleteRequest request, CancellationToken cancellationToken)
        {
            var director = await _db.Directors.FindAsync(request.Id);
            if (director == null)
                return Error("Director not found");

            _db.Directors.Remove(director);
            await _db.SaveChangesAsync();
            return Success("Director deleted successfully", director.Id);
        }
    }
}
