using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO.Image;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ImagesController : ControllerBase
{
    private readonly IImageRepository _repository;

    public ImagesController(IImageRepository repository)
    {
        _repository = repository;
    }
    
    //POST: api/Images/Upload
    public async Task<IActionResult> UploadImage([FromForm] ImageUploadRequestDto uploadRequestDto)
    {
        ValidateFileUpload(uploadRequestDto);
        if (ModelState.IsValid)
        {
            //convert Dto to domain model
            var imageDomainModel = new Image
            {
                File = uploadRequestDto.File,
                FileExtension = Path.GetExtension(uploadRequestDto.File.FileName),
                FileSizeInBytes = uploadRequestDto.File.Length,
                FileName = uploadRequestDto.FileName,
                FileDescription = uploadRequestDto.FileDescription
            };

            //User repository to uplaod image

            await _repository.Upload(imageDomainModel);
            return Ok(imageDomainModel);
        }

        return BadRequest(ModelState);
    }

    private void ValidateFileUpload(ImageUploadRequestDto requestDto)
    {
        var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };

        if (!allowedExtensions.Contains(Path.GetExtension(requestDto.File.FileName)))
        {
            ModelState.AddModelError("file","Unsupported file extension!");
        }

        if (requestDto.File.Length > 10485760)
        {
            ModelState.AddModelError("file","File size more than 10MB, please upload a smaller size file.");
        }
    }
}

