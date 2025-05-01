using CORE.APP.Domain;
using System.ComponentModel.DataAnnotations;

namespace APP.Movies.Domain
{
    public class Movie : Entity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public decimal TotalRevenue { get; set; }

        public int DirectorId { get; set; }
        public Director Director { get; set; }

        public ICollection<MovieGenre> MovieGenres { get; set; }
    }
}
