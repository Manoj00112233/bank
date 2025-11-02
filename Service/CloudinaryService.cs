using Banking_CapStone.Model;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Account = CloudinaryDotNet.Account;

namespace Banking_CapStone.Service
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IConfiguration configuration)
        {
            var account = new Account(
                configuration["Cloudinary:CloudName"],
                configuration["Cloudinary:ApiKey"],
                configuration["Cloudinary:ApiSecret"]);

            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadDocumentAsync(IFormFile file, string folderName = "documents")
        {
            using var stream = file.OpenReadStream();
            var uploadParams = new RawUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = folderName,
                PublicId = $"{Guid.NewGuid()}_{file.FileName}"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            return uploadResult.SecureUrl.ToString();
        }

        public async Task<string> UploadImageAsync(IFormFile file, string folderName = "images")
        {
            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = folderName,
                PublicId = $"{Guid.NewGuid()}_{file.FileName}"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            return uploadResult.SecureUrl.ToString();
        }

        public async Task<string> UploadProfilePictureAsync(IFormFile file, string userId)
        {
            return await UploadImageAsync(file, $"profiles/{userId}");
        }

        public async Task<bool> DeleteFileAsync(string publicId)
        {
            var deletionParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deletionParams);
            return result.Result == "ok";
        }

        public async Task<string> GetFileUrlAsync(string publicId)
        {
            return _cloudinary.Api.UrlImgUp.BuildUrl(publicId);
        }

        public async Task<bool> ValidateFileAsync(IFormFile file, long maxSizeInBytes = 5242880, string[] allowedExtensions = null)
        {
            if (file == null || file.Length == 0)
                return false;

            if (file.Length > maxSizeInBytes)
                return false;

            if (allowedExtensions != null)
            {
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(extension))
                    return false;
            }

            return true;
        }

        public async Task<string> GenerateUniqueFileNameAsync(string originalFileName)
        {
            var extension = Path.GetExtension(originalFileName);
            return $"{Guid.NewGuid()}{extension}";
        }

        public async Task<(bool Success, string Url, string PublicId)> UploadFileWithDetailsAsync(IFormFile file, string folderName)
        {
            var url = await UploadDocumentAsync(file, folderName);
            return (true, url, "");
        }

        public async Task<bool> FileExistsAsync(string publicId)
        {
            return true;
        }

        public async Task<long> GetFileSizeAsync(string publicId)
        {
            return 0;
        }

        public async Task<string> GetFileTypeAsync(string publicId)
        {
            return "unknown";
        }
    }
}
