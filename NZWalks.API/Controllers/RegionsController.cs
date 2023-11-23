using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers;
//https://localhost:1234/api/regions
[Route("api/[controller]")]
[ApiController]
public class RegionsController : ControllerBase
{
   private readonly NZWalksDbContext _dbContext;
   public RegionsController(NZWalksDbContext dbContext)
   {
      this._dbContext = dbContext;
   }
   //GET ALL REGIONS
   //GET: https://localhost:portnumber/api/regions
   [HttpGet]
   public IActionResult GetAll()
   {
      //Get Data From Database - Domain Models
      var regionsDomain = _dbContext.Regions.ToList();
      
      //Map Domain Models to DTOs
      var regionsDto = regionsDomain.Select(region => new RegionDto
      {
         Id = region.Id, 
         Code = region.Code, 
         Name = region.Name, 
         RegionImageUrl = region.RegionImageUrl
      }).ToList();
      
      //Return DTOs
      return Ok(regionsDto);
   }

   //GET REGION BY ID
   //GET: https://localhost:portnumber/api/regions/{id}
   [HttpGet]
   [Route("{id:guid}")]
   public IActionResult GetRegionById([FromRoute]Guid id)
   {
      //var region = _dbContext.Regions.Find(id); this can only be used for Id property

      //Get Data From Database - Domain Model
      var regionDomain = _dbContext.Regions.FirstOrDefault(x => x.Id == id);

      if (regionDomain == null)
         return NotFound();
      
      //Map Domain Models to DTOs
      var regionDto = new RegionDto
      {
         Id = regionDomain.Id, 
         Code = regionDomain.Code, 
         Name = regionDomain.Name, 
         RegionImageUrl = regionDomain.RegionImageUrl
      };
      
      //Return DTO
      return Ok(regionDto);
   }
   
   //POST: To Create New Region
   //POST: https:localhost:portnumber/api/regions
   [HttpPost]
   public IActionResult CreateRegion([FromBody] AddRegionRequestDto addRegionRequestDto)
   {
      //Map or convert DTO to Domain Model
      var regionDomainModel = new Region
      {
         Code = addRegionRequestDto.Code,
         Name = addRegionRequestDto.Name,
         RegionImageUrl = addRegionRequestDto.RegionImageUrl
      };

      //Use Domain Model to create Region
      _dbContext.Regions.Add(regionDomainModel);
      _dbContext.SaveChanges();

      //Map Domain model back to DTO
      var regionDto = new RegionDto
      {
         Id = regionDomainModel.Id,
         Code = regionDomainModel.Code,
         Name = regionDomainModel.Name,
         RegionImageUrl = regionDomainModel.RegionImageUrl
      };
      
      return CreatedAtAction(nameof(GetRegionById),new {id = regionDto.Id}, regionDto);
   }
   
   
   //Update Region
   //PUT: https://localhost:portnumber/api/regions/{id}
   [HttpPut]
   [Route("{id:guid}")]
   public IActionResult UpdateRegion([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
   {
      //Check if region exists
      var updatingDomain = _dbContext.Regions.FirstOrDefault(x => x.Id == id);

      if (updatingDomain == null)
         return NotFound();

      //Map DTO to Domain Model
      updatingDomain.Code = updateRegionRequestDto.Code;
      updatingDomain.Name = updateRegionRequestDto.Name;
      updatingDomain.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;

      _dbContext.SaveChanges();

      //Convert Domain Model to DTO
      var regionDto = new RegionDto
      {
         Id = updatingDomain.Id,
         Code = updatingDomain.Code,
         Name = updatingDomain.Name,
         RegionImageUrl = updatingDomain.RegionImageUrl
      };

      return Ok(regionDto);
   }
   
   //Delete Region
   //DELETE: https://localhost:portnumber/api/reions/{id}
   [HttpDelete]
   [Route("{id:guid}")]
   public IActionResult DeleteRegion([FromRoute] Guid id)
   {
      //Check if region exists
      var regionDomainToDelete = _dbContext.Regions.FirstOrDefault(x => x.Id == id);

      if (regionDomainToDelete == null)
         return NotFound();

      _dbContext.Regions.Remove(regionDomainToDelete);
      _dbContext.SaveChanges();

      //return deleted region back
      var regionDto = new RegionDto
      {
         Id = regionDomainToDelete.Id,
         Code = regionDomainToDelete.Code,
         Name = regionDomainToDelete.Name,
         RegionImageUrl = regionDomainToDelete.RegionImageUrl
      };
      
      return Ok(regionDto);
   }
}