using GameZone.Data.DataModels;
using System.ComponentModel.DataAnnotations;
using static GameZone.Data.Common.DataConstants;

namespace GameZone.Models
{
    public class GameFormViewModel
    {
        [Required]
        [StringLength(GameTitleMaxLength, MinimumLength = GameTitleMinLength)]
        public string Title { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }

        [Required]
        [StringLength(GameDescriptionMaxLength, MinimumLength = GameDescriptionMinLength)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^\d{4}-\d{2}-\d{2}$", ErrorMessage = DateTimeErrorMsg)]
        public string ReleasedOn { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue)]
        public int GenreId { get; set; }

        public IEnumerable<GenreViewModel> Genres { get; set; } = new List<GenreViewModel>();
    }
}
