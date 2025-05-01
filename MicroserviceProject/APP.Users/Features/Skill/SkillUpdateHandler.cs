using CORE.APP.Features;
using APP.Users.Context;
using MediatR;

namespace APP.Users.Features.Skill
{
    public class SkillUpdateCommand : Request, IRequest<CommandResponse>
    {
        public string SkillName { get; set; }
    }

    public class SkillUpdateHandler : Handler, IRequestHandler<SkillUpdateCommand, CommandResponse>
    {
        private readonly UsersDb _db;

        public SkillUpdateHandler(UsersDb db) : base(System.Globalization.CultureInfo.CurrentCulture)
        {
            _db = db;
        }

        public async Task<CommandResponse> Handle(SkillUpdateCommand request, CancellationToken cancellationToken)
        {
            var skill = await _db.Skills.FindAsync(request.Id);
            if (skill == null)
                return Error("Skill not found");

            skill.SkillName = request.SkillName;
            await _db.SaveChangesAsync();
            return Success("Skill updated", skill.Id);
        }
    }
}
