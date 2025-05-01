using CORE.APP.Domain;
using System.ComponentModel.DataAnnotations;

namespace APP.Users.Domain
{
    public class Role : Entity
    {
        [Required]
        [StringLength(50)]
        public string RoleName { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();

    }
}
