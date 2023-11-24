using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WalksController : Controller
{
    private readonly IMapper _mapper;
    private readonly IWalkRepository _walkRepository;

    public WalksController(IMapper mapper,IWalkRepository walkRepository)
    {
        _mapper = mapper;
        _walkRepository = walkRepository;
    }
    //CREATE WALK
    //POST: /api/walks
    [HttpPost]
    public async Task<IActionResult> CreateWalk([FromBody] AddWalkRequestDto addWalkRequestDto)
    {
        //Map DTO to Domain model
        var walkDomainModel = _mapper.Map<Walk>(addWalkRequestDto);

        await _walkRepository.CreateWalkAsync(walkDomainModel);
        
        //Map Domain Model to DTO
        var walkDto = _mapper.Map<WalkDto>(walkDomainModel);
        
        return Ok(walkDto);
    }
    
    //GET ALL WALKS
    //GET: /api/walks
    [HttpGet]
    public async Task<IActionResult> GetAllWalks()
    {
        //Get Data From Database - Domain Models
        var walksDomain = await _walkRepository.GetAllWalksAsync();

        //Map Domain Models to DTOs
        var walkDto = _mapper.Map<List<WalkDto>>(walksDomain);

        //Return DTOs
        return Ok(walkDto);
    }
    
    //GET WALK BY ID
    //GET: /api/walks/{id}
    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetWalkById([FromRoute] Guid id)
    {
        //Get Data From Database - Domain Models
        var walkDomain = await _walkRepository.GetWalkByIdAsync(id);

        //Check if the walk exists
        if (walkDomain == null)
            return NotFound();

        //Map Domain Models to DTOs
        var walkDto = _mapper.Map<WalkDto>(walkDomain);

        return Ok(walkDto);
    }
    
    //UPDATE WALK
    //PUT: /api/walks/{id}
    [HttpPut]
    [Route("{id:guid}")]
    public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id,
        [FromBody] UpdateWalkRequestDto updateWalkRequestDto)
    {
        //Map DTO to Domain Model
        var walkDomain = _mapper.Map<Walk>(updateWalkRequestDto);

        walkDomain = await _walkRepository.UpdateWalkAsync(id, walkDomain);

        if (walkDomain == null)
            return NotFound();

        //Convert Domain Model to DTO
        var walkDto = _mapper.Map<WalkDto>(walkDomain);

        return Ok(walkDto);
    }
    
    //DELETE WALK
    //DELETE: /api/walks/{id}
    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> DeleteWalkAsync([FromRoute] Guid id)
    {
        var walkToDelete = await _walkRepository.DeleteWalkAsync(id);

        if (walkToDelete == null)
            return NotFound();

        return Ok(_mapper.Map<WalkDto>(walkToDelete));
    }
}