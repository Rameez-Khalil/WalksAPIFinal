﻿using Walks.Api.Model.Domain;

namespace Walks.Api.Repos
{
    public interface IRegionRepository
    {
        public Task<List<Region>> GetAllAsync();
        public Task<Region?> GetRegionByIdAsync(Guid id);
        public Task<Region?> CreateAsync(Region region); 
        public Task<Region?> UpdateAsync(Guid id, Region region);
        public Task<Region?> DeleteAsync(Guid id); 
    }
}
