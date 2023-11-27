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

    public async Task<List<Walk>> GetAllWalksAsync(string? filterOn = null, string? filterQuery = null,string?
        sortBy = null,bool? isAscending = null,
        int pageNumber = 1, int pageSize = 1000
        )
    {
        var walks = _dbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();

        //Filtering
        if (!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrWhiteSpace(filterQuery))
        {
            if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                walks = walks.Where(x => x.Name.Contains(filterQuery));


            else if (filterOn.Equals("Description", StringComparison.OrdinalIgnoreCase))
                walks = walks.Where(x => x.Description.Contains(filterQuery));


            else if (filterOn.Equals("LengthInKm", StringComparison.OrdinalIgnoreCase))
                walks = walks.Where(x => Math.Abs(x.LengthInKm - float.Parse(filterQuery)) < 1);
        }


        //Sorting
        if (!string.IsNullOrWhiteSpace(sortBy) && isAscending != null)
        {
            if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                walks = isAscending.Value ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);
            else if (sortBy.Equals("Description", StringComparison.OrdinalIgnoreCase))
                walks = isAscending.Value
                    ? walks.OrderBy(x => x.Description)
                    : walks.OrderByDescending(x => x.Description);
        }
        
        //Pagination
        var skipResults = (pageNumber - 1) * pageSize;

        return await walks.Skip(skipResults).Take(pageSize).ToListAsync();
        
        //return await _dbContext.Walks.Include("Difficulty").Include("Region").ToListAsync(); 
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