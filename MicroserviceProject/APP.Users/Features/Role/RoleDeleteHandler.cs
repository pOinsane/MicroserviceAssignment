using CORE.APP.Features;
using APP.Users.Context;
using MediatR;

namespace APP.Users.Features.Role
{
    public class RoleDeleteCommand : Request, IRequest<CommandResponse>
    {
    }

    public class RoleDeleteHandler : Handler, IRequestHandler<RoleDeleteCommand, CommandResponse>
    {
        private readonly UsersDb _db;

        public RoleDeleteHandler(UsersDb db) : base(System.Globalization.CultureInfo.CurrentCulture)
        {
            _db = db;
        }

        public async Task<CommandResponse> Handle(RoleDeleteCommand request, CancellationToken cancellationToken)
        {
            var role = await _db.Roles.FindAsync(request.Id);
            if (role == null)
                return Error("Role not found");

            _db.Roles.Remove(role);
            await _db.SaveChangesAsync();
            return Success("Role deleted", role.Id);
        }
    }
}
