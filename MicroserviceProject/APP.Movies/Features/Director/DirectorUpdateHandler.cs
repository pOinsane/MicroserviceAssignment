using APP.Movies.Context;
using APP.Movies.Domain;
using CORE.APP.Features;
using MediatR;
using System.Globalization;

namespace APP.Movies.Features.Director
{
    public class DirectorUpdateRequest : Request, IRequest<CommandResponse>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool IsRetired { get; set; }
    }

    public class DirectorUpdateHandler : Handler, IRequestHandler<DirectorUpdateRequest, CommandResponse>
    {
        private readonly MoviesDb _db;

        public DirectorUpdateHandler(MoviesDb db) : base(CultureInfo.CurrentCulture)
        {
            _db = db;
        }

        public async Task<CommandResponse> Handle(DirectorUpdateRequest request, CancellationToken cancellationToken)
        {
            var director = await _db.Directors.FindAsync(request.Id);
            if (director == null)
                return Error("Director not found");

            director.Name = request.Name;
            director.Surname = request.Surname;
            director.IsRetired = request.IsRetired;

            await _db.SaveChangesAsync();
            return Success("Director updated successfully", director.Id);
        }
    }
}
