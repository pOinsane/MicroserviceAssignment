using CORE.APP.Features;
using APP.Users.Context;
using APP.Users.Domain;
using MediatR;

namespace APP.Users.Features.Role
{
    public class RoleCreateCommand : Request, IRequest<CommandResponse>
    {
        public string RoleName { get; set; }
    }

    public class RoleCreateHandler : Handler, IRequestHandler<RoleCreateCommand, CommandResponse>
    {
        private readonly UsersDb _db;

        public RoleCreateHandler(UsersDb db) : base(System.Globalization.CultureInfo.CurrentCulture)
        {
            _db = db;
        }

        public async Task<CommandResponse> Handle(RoleCreateCommand request, CancellationToken cancellationToken)
        {
            var role = new Domain.Role { RoleName = request.RoleName };
            _db.Roles.Add(role);
            await _db.SaveChangesAsync();
            return Success("Role created", role.Id);
        }
    }
}
