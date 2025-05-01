using CORE.APP.Features;
using APP.Users.Context;
using APP.Users.Domain;
using MediatR;

namespace APP.Users.Features.User
{
    public class UserCreateCommand : Request, IRequest<CommandResponse>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public int RoleId { get; set; }
        public List<int> SkillIds { get; set; }
    }

    public class UserCreateHandler : Handler, IRequestHandler<UserCreateCommand, CommandResponse>
    {
        private readonly UsersDb _db;

        public UserCreateHandler(UsersDb db) : base(System.Globalization.CultureInfo.CurrentCulture)
        {
            _db = db;
        }

        public async Task<CommandResponse> Handle(UserCreateCommand request, CancellationToken cancellationToken)
        {
            var user = new Domain.User
            {
                UserName = request.UserName,
                Password = request.Password,
                IsActive = request.IsActive,
                Name = request.Name,
                Surname = request.Surname,
                RegistrationDate = request.RegistrationDate,
                RoleId = request.RoleId,
                UserSkills = request.SkillIds.Select(skillId => new UserSkill { SkillId = skillId }).ToList()
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return Success("User created", user.Id);
        }
    }
}
