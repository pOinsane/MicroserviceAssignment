using CORE.APP.Features;
using APP.Users.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace APP.Users.Features.User
{
    public class UserQuery : Request, IRequest<QueryResponse>
    {
    }

    public class UserQueryHandler : Handler, IRequestHandler<UserQuery, QueryResponse>
    {
        private readonly UsersDb _db;

        public UserQueryHandler(UsersDb db) : base(System.Globalization.CultureInfo.CurrentCulture)
        {
            _db = db;
        }

        public async Task<QueryResponse> Handle(UserQuery request, CancellationToken cancellationToken)
        {
            var users = await _db.Users
                .Include(u => u.UserSkills)
                .ThenInclude(us => us.Skill)
                .ToListAsync();

            return new UserQueryResponse
            {
                Id = 0,
                Users = users
            };
        }
    }

    public class UserQueryResponse : QueryResponse
    {
        public List<Domain.User> Users { get; set; }
    }
}
