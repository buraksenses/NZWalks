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
}