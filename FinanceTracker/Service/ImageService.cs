using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using FinanceTracker.IService;

public class ImageService : IImageService
{
    private readonly Cloudinary _cloudinary;

    public ImageService(Cloudinary cloudinary)
    {
        _cloudinary = cloudinary;
    }

    public async Task<string> UploadImage(IFormFile file)
    {
  

            using var stream = file.OpenReadStream();

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Type = "upload", // makes it public
            };

            var result = await _cloudinary.UploadAsync(uploadParams);
            return result.PublicId;

 
    }
    public async Task<string> GenerateSignedUrl(string publicId)
    {
        return _cloudinary.Api.UrlImgUp
            .Signed(true)
            .Secure(true)
            .BuildUrl(publicId);
    }

    public async Task DeleteImage(string publicId)
    {
        var deleteParams = new DeletionParams(publicId);
        await _cloudinary.DestroyAsync(deleteParams);
    }
}