using CORE.APP.Features;
using APP.Users.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace APP.Users.Features.User
{
    public class UserDeleteCommand : Request, IRequest<CommandResponse>
    {
    }

    public class UserDeleteHandler : Handler, IRequestHandler<UserDeleteCommand, CommandResponse>
    {
        private readonly UsersDb _db;

        public UserDeleteHandler(UsersDb db) : base(System.Globalization.CultureInfo.CurrentCulture)
        {
            _db = db;
        }

        public async Task<CommandResponse> Handle(UserDeleteCommand request, CancellationToken cancellationToken)
        {
            var user = await _db.Users
                .Include(u => u.UserSkills)
                .FirstOrDefaultAsync(u => u.Id == request.Id);

            if (user == null)
                return Error("User not found");

            _db.Users.Remove(user);
            await _db.SaveChangesAsync();
            return Success("User deleted", user.Id);
        }
    }
}
