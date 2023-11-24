using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories;

public class SQLRegionRepository : IRegionRepository
{
    private readonly NZWalksDbContext _dbContext;

    public SQLRegionRepository(NZWalksDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Region?>> GetAllAsync()
    {
        return await _dbContext.Regions.ToListAsync();
    }

    public async Task<Region?> GetRegionByIdAsync(Guid id)
    {
        return await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Region> CreateRegionAsync(Region region)
    {
        await _dbContext.Regions.AddAsync(region);
        await _dbContext.SaveChangesAsync();
        return region;
    }

    public async Task<Region?> UpdateRegionAsync(Guid id,Region region)
    {
        //Check if region exists
        var updatingRegion = GetRegionByIdAsync(id).Result;

        if (updatingRegion == null)
            return null;

        updatingRegion.Code = region.Code;
        updatingRegion.Name = region.Name;
        updatingRegion.RegionImageUrl = region.RegionImageUrl;
        
        await _dbContext.SaveChangesAsync();
        return updatingRegion;
    }

    public async Task<Region?> DeleteRegionAsync(Guid id)
    {
        //Check if region exists
        var regionToDelete = GetRegionByIdAsync(id).Result;
        if (regionToDelete == null)
            return null;
        
        _dbContext.Remove(regionToDelete);
        await _dbContext.SaveChangesAsync();
        return regionToDelete;
    }
}