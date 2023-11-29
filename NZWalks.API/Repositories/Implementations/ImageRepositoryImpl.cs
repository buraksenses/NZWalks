using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories;

public class ImageRepositoryImpl : IImageRepository
{
    private readonly IWebHostEnvironment _environment;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly NZWalksDbContext _dbContext;

    public ImageRepositoryImpl(IWebHostEnvironment environment,IHttpContextAccessor httpContextAccessor,NZWalksDbContext dbContext)
    {
        _environment = environment;
        _httpContextAccessor = httpContextAccessor;
        _dbContext = dbContext;
    }
    
    public async Task<Image> Upload(Image image)
    {
        var localFilePath = Path.Combine(_environment.ContentRootPath, "Images", $"{image.FileName}{image.FileExtension}");

        //Upload image to local path
        using var stream = new FileStream(localFilePath, FileMode.Create);
        await image.File.CopyToAsync(stream);

        //https://localhost:1234/Images/image.jpg
        var urlFilePath = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/Images/{image.FileName}{image.FileExtension}";
        image.FilePath = urlFilePath;
        
        //Add Image to the Images table
        await _dbContext.Images.AddAsync(image);
        await _dbContext.SaveChangesAsync();

        return image;
    }
}