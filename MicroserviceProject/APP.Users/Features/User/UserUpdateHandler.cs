using CORE.APP.Features;
using APP.Users.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace APP.Users.Features.User
{
    public class UserUpdateCommand : Request, IRequest<CommandResponse>
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

    public class UserUpdateHandler : Handler, IRequestHandler<UserUpdateCommand, CommandResponse>
    {
        private readonly UsersDb _db;

        public UserUpdateHandler(UsersDb db) : base(System.Globalization.CultureInfo.CurrentCulture)
        {
            _db = db;
        }

        public async Task<CommandResponse> Handle(UserUpdateCommand request, CancellationToken cancellationToken)
        {
            var user = await _db.Users
                .Include(u => u.UserSkills)
                .FirstOrDefaultAsync(u => u.Id == request.Id);

            if (user == null)
                return Error("User not found");

            user.UserName = request.UserName;
            user.Password = request.Password;
            user.IsActive = request.IsActive;
            user.Name = request.Name;
            user.Surname = request.Surname;
            user.RegistrationDate = request.RegistrationDate;
            user.RoleId = request.RoleId;

            user.UserSkills.Clear();
            foreach (var skillId in request.SkillIds)
                user.UserSkills.Add(new Domain.UserSkill { SkillId = skillId });

            await _db.SaveChangesAsync();
            return Success("User updated", user.Id);
        }
    }
}
