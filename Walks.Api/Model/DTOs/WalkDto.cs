using System.ComponentModel.DataAnnotations;
using Walks.Api.Model.Domain;

namespace Walks.Api.Model.DTOs
{
    public class WalkDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
        [Required]
        public double Length { get; set; }
        [Required]
        public string WalkImageUrl { get; set; }

        [Required]
        public Guid DifficultyId { get; set; }

        [Required]
        public Guid RegionId { get; set; }


       
    }
}
