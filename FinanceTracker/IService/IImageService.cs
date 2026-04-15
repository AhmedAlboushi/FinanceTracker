namespace FinanceTracker.IService
{
    public interface IImageService
    {
        public Task<string> GenerateSignedUrl(string publicId);
        public Task<string> UploadImage(IFormFile file);
        public Task DeleteImage(string publicId);

    }
}
