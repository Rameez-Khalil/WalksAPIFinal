using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using Walks.Api.Data;
using Walks.Api.Model.Domain;
using Walks.Api.Model.DTOs;

namespace Walks.Api.Repos
{
    public class WalkRepo : IWalkRepo
    {
        private readonly WalksDbContext _dbContext;

        public WalkRepo(WalksDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }

        public async Task<List<Walk>> GetAllAsync(string? filterOn, string? filterQuery, string? sorting, bool isAscending, int pageNumber = 1, int pageSize = 1000)
        {
            var walks = _dbContext.Walks
                .Include(x=>x.Region)
                .Include(x=>x.Difficulty).AsQueryable();

            //filter the data.
            //First check and validate the incoming params.
            if(!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrWhiteSpace(filterQuery))
            {
                //check for the name filter.
                if(filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase)){
                    walks = walks.Where(x => x.Name.Contains(filterQuery)); 
                }
            }

            //sort the data.
            //validate the params.
            if (!string.IsNullOrWhiteSpace(sorting))
            {
                //check for the column name.
                if(sorting.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    //We now have a valid input.
                    walks = isAscending? walks.OrderBy(x=>x.Name): walks.OrderByDescending(x=>x.Name);
                }
            }

            //pagination.
            //calculate the skip results.
            int skipNumber = (pageNumber- 1) * pageSize;
            return await walks.Skip(skipNumber).Take(pageSize).ToListAsync(); 
            
        }

        public async Task<Walk?> GetWalkByIdAsync(Guid id)
        {
            //find the walk.
            var walk = await _dbContext.Walks.Include(x => x.Region)
                .Include(x => x.Difficulty)
                .FirstOrDefaultAsync(x=>x.Id==id);

            return walk;     
        }
        public async Task<Walk?> CreateWalkAsync(Walk walk)
        {
            //accept the walk.
            await _dbContext.Walks.AddAsync(walk);
            await _dbContext.SaveChangesAsync();
            return walk; 
        }

        public async Task<Walk> UpdateWalkAsync(Walk walk, WalkDto providedData)
        {
            walk.Name = providedData.Name; 
            walk.WalkImageUrl = providedData.WalkImageUrl;
            walk.Description = providedData.Description;
            walk.DifficultyId = providedData.DifficultyId;
            walk.RegionId = providedData.RegionId;
            walk.Length = providedData.Length;


            await _dbContext.SaveChangesAsync();
            return walk; 
        }

        public async Task<Walk?> DeleteWalkAsync(Walk walk)
        {
            //Receive the walk and delete it.
            _dbContext.Walks.Remove(walk); 
            await _dbContext.SaveChangesAsync();
            return walk; 
        }
    }
}
