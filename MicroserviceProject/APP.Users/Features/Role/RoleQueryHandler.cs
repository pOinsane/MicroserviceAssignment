using CORE.APP.Features;
using APP.Users.Context;
using APP.Users.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace APP.Users.Features.Role
{
    public class RoleQuery : Request, IRequest<QueryResponse>
    {
    }

    public class RoleQueryHandler : Handler, IRequestHandler<RoleQuery, QueryResponse>
    {
        private readonly UsersDb _db;

        public RoleQueryHandler(UsersDb db) : base(System.Globalization.CultureInfo.CurrentCulture)
        {
            _db = db;
        }

        public async Task<QueryResponse> Handle(RoleQuery request, CancellationToken cancellationToken)
        {
            var result = await _db.Roles.ToListAsync();
            return new RoleQueryResponse
            {
                Id = 0,
                Roles = result
            };
        }
    }

    public class RoleQueryResponse : QueryResponse
    {
        public List<Domain.Role> Roles { get; set; }
    }
}
