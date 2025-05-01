using CORE.APP.Domain;
using System.ComponentModel.DataAnnotations;

namespace APP.Users.Domain
{
    public class Skill : Entity
    {
        [Required]
        [StringLength(50)]
        public string SkillName { get; set; }

        public ICollection<UserSkill> UserSkills { get; set; } = new List<UserSkill>();

    }
}
