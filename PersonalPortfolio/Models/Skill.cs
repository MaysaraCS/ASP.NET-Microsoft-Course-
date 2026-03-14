using System.ComponentModel.DataAnnotations;

namespace PersonalPortfolio.Models
{
    public class Skill
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Category { get; set; } = string.Empty;

        [Range(1, 5)]
        public int ProficiencyLevel { get; set; }

        [StringLength(100)]
        public string IconClass { get; set; } = string.Empty;
    }
}