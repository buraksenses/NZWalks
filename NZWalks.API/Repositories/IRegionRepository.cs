using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Repositories;

public interface IRegionRepository
{
    Task<List<Region?>> GetAllAsync();

    Task<Region?> GetRegionByIdAsync(Guid id);

    Task<Region> CreateRegionAsync(Region region);

    Task<Region?> UpdateRegionAsync(Guid id,Region region);

    Task<Region?> DeleteRegionAsync(Guid id);
}