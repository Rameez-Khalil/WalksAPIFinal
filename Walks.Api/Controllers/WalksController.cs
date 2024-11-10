using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Walks.Api.Model.Domain;
using Walks.Api.Model.DTOs;
using Walks.Api.Repos;

namespace Walks.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IWalkRepo walkRepo;

        public WalksController(IWalkRepo _walkRepo)
        {
            walkRepo = _walkRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalks([FromQuery] string? filterOn, [FromQuery] string? filterQuery="name", [FromQuery] string? sorting = "name", [FromQuery] bool isAscending=true)
        {
            //delegate this responsibility to the repo.
            var walks = await walkRepo.GetAllAsync(filterOn, filterQuery, sorting, isAscending);

            //return not found.
            if (walks.Count == 0) { return NotFound("No walks found"); }

            //return the walks.
            //must convert each walk into its DTO representation.
            var walksDto = new List<WalkDto>();

            //Read each walk provided by the repo and convert it into its DTO.
            foreach (var walk in walks)
            {
                walksDto.Add(new WalkDto
                {
                    Description = walk.Description,
                    DifficultyId = walk.DifficultyId,
                    Length = walk.Length,
                    Name = walk.Name,
                    RegionId = walk.RegionId,
                    WalkImageUrl = walk.WalkImageUrl,

                });

            }

            return Ok(walksDto);
        }


        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetWalkById(Guid id)
        {
            //Find the walk based off of the id provided.
            var walk = await walkRepo.GetWalkByIdAsync(id);

            //check for nullability.
            if (walk == null)
            {
                return NotFound("Walk cannot be located");
            }

            //map the walk back to the DTO.
            var walkDto = new WalkDto
            {
                Description = walk.Description,
                Name = walk.Name,
                RegionId = walk.RegionId,
                Length = walk.Length,
                WalkImageUrl = walk.WalkImageUrl,
                DifficultyId = walk.DifficultyId,
            };

            return Ok(walkDto);

        }

        [HttpPost]
        public async Task<IActionResult> CreateWalk([FromBody] WalkDto walkDto)
        {
            //data validation.
            if (!ModelState.IsValid)
            {
                return BadRequest("Provided data is not valid");
            }

            //accept the data, and map it back to the walks domain model..
            var walk = new Walk
            {
                Name = walkDto.Name,
                DifficultyId = walkDto.DifficultyId,
                Length = walkDto.Length,
                Description = walkDto.Description,
                RegionId = walkDto.RegionId,
                WalkImageUrl = walkDto.WalkImageUrl,

            };

            //send it to the repo.
            var walkSaved = await walkRepo.CreateWalkAsync(walk);

            if (walkSaved == null)
            {
                return BadRequest("Operation failed");

            }

            return CreatedAtAction(nameof(GetWalkById), new { id = walkSaved.Id }, walkDto);


        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateWalk([FromRoute] Guid id, [FromBody] WalkDto providedWalk)
        {
            //find the walk.
            var existingWalk = await walkRepo.GetWalkByIdAsync(id);
            if (existingWalk == null) { return NotFound("walk cannot be located"); }

            //check for data validity.
            if (!ModelState.IsValid)
            {
                return BadRequest("Cannot perform the update operation with the provided details");
            }

            var walkUpdated = await walkRepo.UpdateWalkAsync(existingWalk, providedWalk);

            if (walkUpdated == null)
            {
                return BadRequest("Cannot complete the operation");
            }

            //return the updated results.
            return Ok(providedWalk);

        }


        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteWalk(Guid id)
        {
            //find the walk.
            var walk = await walkRepo.GetWalkByIdAsync(id);

            //null check.
            if (walk == null) { return NotFound("Cannot find the walk"); }

            //pass this walk onto the repo method.
            var walkDeleted = await walkRepo.DeleteWalkAsync(walk);

            return Ok(walkDeleted);

        }
    }
}
