using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories;

public interface IWalkRepository
{
    Task<Walk> CreateWalkAsync(Walk walk);

    Task<List<Walk>> GetAllWalksAsync(string? filterOn = null,string? filterQuery = null,string? 
        sortBy = null,bool? isAscending = null,
        int pageNumber = 1, int pageSize = 1000
        );

    Task<Walk?> GetWalkByIdAsync(Guid id);

    Task<Walk?> UpdateWalkAsync(Guid id, Walk walk);

    Task<Walk?> DeleteWalkAsync(Guid id);
}