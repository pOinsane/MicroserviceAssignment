using CORE.APP.Domain;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace APP.Books.Domain
{
    public class Genre : Entity
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public ICollection<BookGenre> BookGenres { get; set; }
    }
}
