using CORE.APP.Features;
using APP.Users.Context;
using APP.Users.Domain;
using MediatR;

namespace APP.Users.Features.Skill
{
    public class SkillCreateCommand : Request, IRequest<CommandResponse>
    {
        public string SkillName { get; set; }
    }

    public class SkillCreateHandler : Handler, IRequestHandler<SkillCreateCommand, CommandResponse>
    {
        private readonly UsersDb _db;

        public SkillCreateHandler(UsersDb db) : base(System.Globalization.CultureInfo.CurrentCulture)
        {
            _db = db;
        }

        public async Task<CommandResponse> Handle(SkillCreateCommand request, CancellationToken cancellationToken)
        {
            var skill = new Domain.Skill { SkillName = request.SkillName };
            _db.Skills.Add(skill);
            await _db.SaveChangesAsync();
            return Success("Skill created", skill.Id);
        }
    }
}
