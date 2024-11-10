using System.ComponentModel.DataAnnotations;

namespace Walks.Api.Model.DTOs
{
    public class RegionDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Code { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
