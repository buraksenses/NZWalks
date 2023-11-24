using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories;

public class SqlWalkRepository : IWalkRepository
{
    private readonly NZWalksDbContext _dbContext;

    public SqlWalkRepository(NZWalksDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Walk> CreateWalkAsync(Walk walk)
    {
        await _dbContext.Walks.AddAsync(walk);
        await _dbContext.SaveChangesAsync();
        return walk;
    }

    public async Task<List<Walk>> GetAllWalksAsync()
    {
        return await _dbContext.Walks.Include("Difficulty").Include("Region").ToListAsync(); 
    }

    public async Task<Walk?> GetWalkByIdAsync(Guid id)
    {
        return await _dbContext.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Walk?> UpdateWalkAsync(Guid id, Walk walk)
    {
        var updatingWalk = await GetWalkByIdAsync(id);

        if (updatingWalk == null)
            return null;

        updatingWalk.Name = walk.Name;
        updatingWalk.Description = walk.Description;
        updatingWalk.WalkImageUrl = walk.WalkImageUrl;
        updatingWalk.LengthInKm = walk.LengthInKm;
        updatingWalk.RegionId = walk.RegionId;
        updatingWalk.DifficultyId = walk.DifficultyId;

        await _dbContext.SaveChangesAsync();
        return updatingWalk;
    }

    public async Task<Walk?> DeleteWalkAsync(Guid id)
    {
        var walkToDelete = await GetWalkByIdAsync(id);

        if (walkToDelete == null)
            return null;

        _dbContext.Walks.Remove(walkToDelete);
        await _dbContext.SaveChangesAsync();

        return walkToDelete;
    }
}