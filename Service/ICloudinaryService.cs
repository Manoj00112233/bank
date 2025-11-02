namespace Banking_CapStone.Service
{
    public interface ICloudinaryService
    {
        Task<string> UploadDocumentAsync(IFormFile file, string folderName = "documents");
        Task<string> UploadImageAsync(IFormFile file, string folderName = "images");
        Task<string> UploadProfilePictureAsync(IFormFile file, string userId);
        Task<bool> DeleteFileAsync(string publicId);
        Task<string> GetFileUrlAsync(string publicId);
        Task<bool> ValidateFileAsync(IFormFile file, long maxSizeInBytes = 5242880, string[] allowedExtensions = null);
        Task<string> GenerateUniqueFileNameAsync(string originalFileName);
        Task<(bool Success, string Url, string PublicId)> UploadFileWithDetailsAsync(IFormFile file, string folderName);
        Task<bool> FileExistsAsync(string publicId);
        Task<long> GetFileSizeAsync(string publicId);
        Task<string> GetFileTypeAsync(string publicId);
    }
}


