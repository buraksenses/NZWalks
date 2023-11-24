using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers;
//https://localhost:portnumber/api/regions
[Route("api/[controller]")]
[ApiController]
public class RegionsController : ControllerBase
{
   private readonly IRegionRepository _regionRepository;
   public RegionsController(IRegionRepository regionRepository)
   {
      this._regionRepository = regionRepository;
   }
   //GET ALL REGIONS
   //GET: https://localhost:portnumber/api/regions
   [HttpGet]
   public async Task<IActionResult> GetAll()
   {
      //Get Data From Database - Domain Models
      var regionsDomain = await _regionRepository.GetAllAsync();
      
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
   public async Task<IActionResult> GetRegionById([FromRoute]Guid id)
   {
      //var region = _dbContext.Regions.Find(id); this can only be used for Id property

      //Get Data From Database - Domain Model
      var regionDomain = await _regionRepository.GetRegionByIdAsync(id);

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
   public async Task<IActionResult> CreateRegion([FromBody] AddRegionRequestDto addRegionRequestDto)
   {
      //Map or convert DTO to Domain Model
      var regionDomainModel = new Region
      {
         Code = addRegionRequestDto.Code,
         Name = addRegionRequestDto.Name,
         RegionImageUrl = addRegionRequestDto.RegionImageUrl
      };

      //Use Domain Model to create Region
      regionDomainModel = await _regionRepository.CreateRegionAsync(regionDomainModel);

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
   public async Task<IActionResult> UpdateRegion([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
   {
      //Map DTO to Domain Model
      var regionDomainModel = new Region
      {
         Code = updateRegionRequestDto.Code,
         Name = updateRegionRequestDto.Name,
         RegionImageUrl = updateRegionRequestDto.RegionImageUrl
      };
      
      regionDomainModel = await _regionRepository.UpdateRegionAsync(id, regionDomainModel);

      if (regionDomainModel == null)
         return NotFound();

      //Convert Domain Model to DTO
      var regionDto = new RegionDto
      {
         Id = regionDomainModel.Id,
         Code = regionDomainModel.Code,
         Name = regionDomainModel.Name,
         RegionImageUrl = regionDomainModel.RegionImageUrl
      };

      return Ok(regionDto);
   }
   
   //Delete Region
   //DELETE: https://localhost:portnumber/api/reions/{id}
   [HttpDelete]
   [Route("{id:guid}")]
   public async Task<IActionResult> DeleteRegion([FromRoute] Guid id)
   {
      var regionToDelete = await _regionRepository.DeleteRegionAsync(id);
      if (regionToDelete == null)
         return NotFound();

      //return deleted region back
      var regionDto = new RegionDto
      {
         Id = regionToDelete.Id,
         Code = regionToDelete.Code,
         Name = regionToDelete.Name,
         RegionImageUrl = regionToDelete.RegionImageUrl
      };
      
      return Ok(regionDto);
   }
}