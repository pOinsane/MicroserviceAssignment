using CORE.APP.Features;
using APP.Users.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace APP.Users.Features.Skill
{
    public class SkillQuery : Request, IRequest<QueryResponse>
    {
    }

    public class SkillQueryHandler : Handler, IRequestHandler<SkillQuery, QueryResponse>
    {
        private readonly UsersDb _db;

        public SkillQueryHandler(UsersDb db) : base(System.Globalization.CultureInfo.CurrentCulture)
        {
            _db = db;
        }

        public async Task<QueryResponse> Handle(SkillQuery request, CancellationToken cancellationToken)
        {
            var result = await _db.Skills.ToListAsync();
            return new SkillQueryResponse
            {
                Id = 0,
                Skills = result
            };
        }
    }

    public class SkillQueryResponse : QueryResponse
    {
        public List<Domain.Skill> Skills { get; set; }
    }
}
