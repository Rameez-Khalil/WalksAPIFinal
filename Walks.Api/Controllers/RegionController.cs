using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Scaffolding;
using System.Globalization;
using System.Text.Json;
using Walks.Api.Model.Domain;
using Walks.Api.Model.DTOs;
using Walks.Api.Repos;

namespace Walks.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class RegionController : ControllerBase
    {
        private readonly IRegionRepository _regionRepository;
        private readonly ILogger<RegionController> logger;

        public RegionController(IRegionRepository regionRepository, ILogger<RegionController> logger)
        {
            this._regionRepository = regionRepository;
            this.logger = logger;
        }

        [HttpGet]
        //[Authorize(Roles ="Reader")]
        public async Task<IActionResult> GetAllRegions()
        {
            logger.LogInformation("Get all regions invoked"); 

            //Returns the list of regions.
            var regions = await _regionRepository.GetAllAsync();

            if (regions.Count == 0) {
                return NotFound("There are no regions"); 
            }


            //Return the DTO format.
            var regionList = new List<RegionDTO>(); 

            //Map thrugh the list of regions.
            foreach(var region in regions)
            {
                regionList.Add(new RegionDTO
                {
                    Name = region.Name,
                    Code = region.Code,
                    RegionImageUrl = region.RegionImageUrl
                }); 
            }

            //return the DTO representation.
            logger.LogInformation($"Finished querying the data : {JsonSerializer.Serialize(regions)}"); 
            return Ok(regionList); 
            
        }

        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetRegionById([FromRoute] Guid id)
        {
            //Find the region.
            var region = await _regionRepository.GetRegionByIdAsync(id);
            //Return the dto.
            if (region == null) {
                return NotFound("There are no regions associated with the provided id"); 
            }
            //Return the success, after mapping.
            return Ok(new RegionDTO
            {
                Name = region.Name,
                Code = region.Code,
                RegionImageUrl = region.RegionImageUrl
            }); 
            
        }

        [HttpPost]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> CreateRegion([FromBody] RegionDTO providedRegion)
        {
            //validate the model.
            if (!ModelState.IsValid)
            {
                return BadRequest("Provided data is not as per the policy"); 
            }

            //send this data to the repo after conversion.
            var region = new Region
            {
                Name = providedRegion.Name,
                Code = providedRegion.Code,
                RegionImageUrl = providedRegion.RegionImageUrl
            };
            var regionSaved = await _regionRepository.CreateAsync(region);
            if ((regionSaved==null))
            {
                return BadRequest("Cannot save the data"); 
            }

            //map the saved region back to the DTO and send it through the created at action.
            var regionDto = new RegionDTO
            {
                Name = region.Name,
                Code = region.Code,
                RegionImageUrl = region.RegionImageUrl
            };
            return CreatedAtAction(nameof(GetRegionById), new { id = region.Id }, regionDto); 
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> UpdateRegionAsync([FromRoute] Guid id, [FromBody] RegionDTO providedDto)
        {
            //validate the model.
            if (!ModelState.IsValid) return BadRequest("Please provide the details");

            //Find the region.
            var regionFound = await _regionRepository.GetRegionByIdAsync(id);

            if (regionFound == null) 
            {
                return NotFound("No region was found"); 
            }

            //map the data to the region and send it to the repo.
            var region = new Region
            {
                Name = providedDto.Name,
                Code = providedDto.Code,
                RegionImageUrl = providedDto.RegionImageUrl
            }; 

            var regionUpdated = await _regionRepository.UpdateAsync(id, region);

            //check for the null status.
            if (regionUpdated == null) return BadRequest("Cannot complete the operaton");

            return Ok(); 
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]

        public async Task<IActionResult> DeleteRegionAsync([FromRoute] Guid id)
        {
            //Find the region.
            var region = await _regionRepository.GetRegionByIdAsync(id);

            //check for the region's existence.
            if (region == null) return NotFound("Region not found");

            //perform the delete operation.
            var regionDeleted = await _regionRepository.DeleteAsync(id);

            //check for the nullability.
            if (regionDeleted == null) return BadRequest("Cannot perform this operation"); 


            
            return Ok();
        }
    }
}
