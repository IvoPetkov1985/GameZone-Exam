using System.ComponentModel.DataAnnotations;

namespace GameZone.Data.DataModels
{
    public class Genre
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public IEnumerable<Game> Games { get; set; } = new List<Game>();
    }
}
