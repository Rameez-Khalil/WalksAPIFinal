using Microsoft.EntityFrameworkCore;
using Walks.Api.Data;
using Walks.Api.Model.Domain;

namespace Walks.Api.Repos
{
    public class RegionRepository : IRegionRepository
    {
        private readonly WalksDbContext _dbcontext;

        public RegionRepository(WalksDbContext dbContext)
        {
            _dbcontext = dbContext;
        }

        public async Task<Region?> CreateAsync(Region region)
        {
            await _dbcontext.Regions.AddAsync(region);
            await _dbcontext.SaveChangesAsync();
            return region;
        }

        public async Task<List<Region>> GetAllAsync()
        {
            var regions = await _dbcontext.Regions.ToListAsync();
            return regions; 
        }
        public async Task<Region?> GetRegionByIdAsync(Guid id)
        {
            //Find the region.
            var region = await _dbcontext.Regions.FindAsync(id);
            return region; 
        }

        public async Task<Region?> UpdateAsync(Guid id, Region providedRegion)
        {
            //Find the region.
            var region = await GetRegionByIdAsync(id);

            //update the region.
            region.Name = providedRegion.Name;
            region.Code = providedRegion.Code;
            region.RegionImageUrl = providedRegion.RegionImageUrl;

            await _dbcontext.SaveChangesAsync(); 

            return region;
        }

        public async Task<Region?> DeleteAsync(Guid id)
        {
            //Find the region.
            var region = await _dbcontext.Regions.FindAsync(id);
            if (region != null)
            {
                _dbcontext.Regions.Remove(region); 
                await _dbcontext.SaveChangesAsync();
            }

            return region; 
        }
    }
}
