using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Custom_Action_Folders;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers;
//https://localhost:portnumber/api/regions
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class RegionsController : ControllerBase
{
   private readonly IRegionRepository _regionRepository;
   private readonly IMapper _mapper;
   public RegionsController(IRegionRepository regionRepository, IMapper mapper)
   {
      this._regionRepository = regionRepository;
      this._mapper = mapper;
   }
   //GET ALL REGIONS
   //GET: https://localhost:portnumber/api/regions
   [HttpGet]
   public async Task<IActionResult> GetAll()
   {
      //Get Data From Database - Domain Models
      var regionsDomain = await _regionRepository.GetAllAsync();
      
      // //Map Domain Models to DTOs
      // var regionsDto = regionsDomain.Select(region => new RegionDto
      // {
      //    Id = region.Id, 
      //    Code = region.Code, 
      //    Name = region.Name, 
      //    RegionImageUrl = region.RegionImageUrl
      // }).ToList();
      
      
      //Map Domain Models to DTOs
      var regionsDto = _mapper.Map<List<RegionDto>>(regionsDomain);
      
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
      var regionDto = _mapper.Map<RegionDto>(regionDomain);
      
      //Return DTO
      return Ok(regionDto);
   }
   
   //POST: To Create New Region
   //POST: https:localhost:portnumber/api/regions
   [HttpPost]
   [ValidateModel]
   public async Task<IActionResult> CreateRegion([FromBody] AddRegionRequestDto addRegionRequestDto)
   {
      //Map or convert DTO to Domain Model
      var regionDomainModel = _mapper.Map<Region>(addRegionRequestDto);

      //Use Domain Model to create Region
      regionDomainModel = await _regionRepository.CreateRegionAsync(regionDomainModel);

      //Map Domain model back to DTO
      var regionDto = _mapper.Map<RegionDto>(regionDomainModel);
      
      return CreatedAtAction(nameof(GetRegionById),new {id = regionDto.Id}, regionDto);
   }
   
   
   //Update Region
   //PUT: https://localhost:portnumber/api/regions/{id}
   [HttpPut]
   [Route("{id:guid}")]
   [ValidateModel]
   public async Task<IActionResult> UpdateRegion([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
   {
      //Map DTO to Domain Model
      var regionDomainModel = _mapper.Map<Region>(updateRegionRequestDto);
      
      regionDomainModel = await _regionRepository.UpdateRegionAsync(id, regionDomainModel);

      if (regionDomainModel == null)
         return NotFound();

      //Convert Domain Model to DTO
      var regionDto = _mapper.Map<RegionDto>(regionDomainModel);

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
      var regionDto = _mapper.Map<RegionDto>(regionToDelete);
      
      return Ok(regionDto);
   }
}