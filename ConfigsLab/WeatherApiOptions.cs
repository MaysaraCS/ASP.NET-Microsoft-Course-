using System.ComponentModel.DataAnnotations;
namespace ConfigsLab
{
    public class WeatherApiOptions
    {
        [Required(ErrorMessage = "API Key is required.")]
        [StringLength(25, MinimumLength = 10)]
        public string? ApiKey { get; set; }

        [Required]
        [Url(ErrorMessage = "Base URL must be a valid URL.")]
        [StringLength(5000, MinimumLength = 12)]
        public string? BaseUrl { get; set; }
        
        [Range(1, 300, ErrorMessage = "Timeout must be between 1 and 300 seconds.")]
        public int TimeoutSeconds { get; set; }
        public bool EnabledCashing { get; set; }
    }
}