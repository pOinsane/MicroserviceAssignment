using CORE.APP.Features;
using APP.Users.Context;
using MediatR;

namespace APP.Users.Features.Role
{
    public class RoleUpdateCommand : Request, IRequest<CommandResponse>
    {
        public string RoleName { get; set; }
    }

    public class RoleUpdateHandler : Handler, IRequestHandler<RoleUpdateCommand, CommandResponse>
    {
        private readonly UsersDb _db;

        public RoleUpdateHandler(UsersDb db) : base(System.Globalization.CultureInfo.CurrentCulture)
        {
            _db = db;
        }

        public async Task<CommandResponse> Handle(RoleUpdateCommand request, CancellationToken cancellationToken)
        {
            var role = await _db.Roles.FindAsync(request.Id);
            if (role == null)
                return Error("Role not found");

            role.RoleName = request.RoleName;
            await _db.SaveChangesAsync();
            return Success("Role updated", role.Id);
        }
    }
}
