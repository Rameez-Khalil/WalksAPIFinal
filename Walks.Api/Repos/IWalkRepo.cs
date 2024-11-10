using Walks.Api.Model.Domain;
using Walks.Api.Model.DTOs;

namespace Walks.Api.Repos
{
    public interface IWalkRepo
    {
        public Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? sorting=null, bool isAscending=true, int pageNumber=1, int pageSize=1000);
        public Task<Walk?> GetWalkByIdAsync(Guid id);
        public Task<Walk?> CreateWalkAsync(Walk walk);
        public Task<Walk?> UpdateWalkAsync(Walk walk, WalkDto providedData);
        public Task<Walk?> DeleteWalkAsync(Walk walk); 
    }
}
