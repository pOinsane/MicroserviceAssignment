using CORE.APP.Domain;
using System.ComponentModel.DataAnnotations;

namespace APP.Movies.Domain
{
    public class Director : Entity
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Surname { get; set; }

        public bool IsRetired { get; set; }

        public ICollection<Movie> Movies { get; set; }
    }
}
