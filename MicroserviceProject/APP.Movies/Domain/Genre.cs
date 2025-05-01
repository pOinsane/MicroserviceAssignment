using CORE.APP.Domain;
using System.ComponentModel.DataAnnotations;

namespace APP.Movies.Domain
{
    public class Genre : Entity
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public ICollection<MovieGenre> MovieGenres { get; set; }
    }
}
