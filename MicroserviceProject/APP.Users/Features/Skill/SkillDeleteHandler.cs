using CORE.APP.Features;
using APP.Users.Context;
using MediatR;

namespace APP.Users.Features.Skill
{
    public class SkillDeleteCommand : Request, IRequest<CommandResponse>
    {
    }

    public class SkillDeleteHandler : Handler, IRequestHandler<SkillDeleteCommand, CommandResponse>
    {
        private readonly UsersDb _db;

        public SkillDeleteHandler(UsersDb db) : base(System.Globalization.CultureInfo.CurrentCulture)
        {
            _db = db;
        }

        public async Task<CommandResponse> Handle(SkillDeleteCommand request, CancellationToken cancellationToken)
        {
            var skill = await _db.Skills.FindAsync(request.Id);
            if (skill == null)
                return Error("Skill not found");

            _db.Skills.Remove(skill);
            await _db.SaveChangesAsync();
            return Success("Skill deleted", skill.Id);
        }
    }
}
