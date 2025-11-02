using Banking_CapStone.Model;
using Banking_CapStone.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Banking_CapStone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CloudinaryController : BaseApiController
    {
        private readonly ICloudinaryService _cloudinaryService;
    public CloudinaryController(ICloudinaryService cloudinaryService)
        {
            _cloudinaryService = cloudinaryService;
        }

        [HttpPost("upload/document")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadDocument([FromForm] FileUploadRequest request)
        {
            var file = request.File;
            var folderName = request.FolderName ?? "documents";

            if (file == null || file.Length == 0)
                return BadRequest("File is required");

            var isValid = await _cloudinaryService.ValidateFileAsync(
                file,
                maxSizeInBytes: 5242880,
                allowedExtensions: new[] { ".pdf", ".jpg", ".jpeg", ".png", ".doc", ".docx" });

            if (!isValid)
                return BadRequest("Invalid file. Max size 5MB. Allowed: PDF, JPG, PNG, DOC, DOCX");

            try
            {
                var url = await _cloudinaryService.UploadDocumentAsync(file, folderName);
                return Ok(new { success = true, url, message = "Document uploaded successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("upload/image")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadImage([FromForm] FileUploadRequest request)
        {
            var file = request.File;
            var folderName = request.FolderName ?? "images";

            if (file == null || file.Length == 0)
                return BadRequest("File is required");

            var isValid = await _cloudinaryService.ValidateFileAsync(
                file,
                maxSizeInBytes: 2097152,
                allowedExtensions: new[] { ".jpg", ".jpeg", ".png", ".gif" });

            if (!isValid)
                return BadRequest("Invalid file. Max size 2MB. Allowed: JPG, PNG, GIF");

            try
            {
                var url = await _cloudinaryService.UploadImageAsync(file, folderName);
                return Ok(new { success = true, url, message = "Image uploaded successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("upload/profile-picture")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadProfilePicture([FromForm] FileUploadRequest request)
        {
            var file = request.File;
            var userId = GetUserId().ToString();

            if (file == null || file.Length == 0)
                return BadRequest("File is required");

            var isValid = await _cloudinaryService.ValidateFileAsync(
                file,
                maxSizeInBytes: 1048576,
                allowedExtensions: new[] { ".jpg", ".jpeg", ".png" });

            if (!isValid)
                return BadRequest("Invalid file. Max size 1MB. Allowed: JPG, PNG");

            try
            {
                var url = await _cloudinaryService.UploadProfilePictureAsync(file, userId);
                return Ok(new { success = true, url, message = "Profile picture uploaded successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteFile([FromQuery] string publicId)
        {
            if (string.IsNullOrEmpty(publicId))
                return BadRequest("Public ID is required");

            try
            {
                var deleted = await _cloudinaryService.DeleteFileAsync(publicId);

                if (deleted)
                    return Ok(new { success = true, message = "File deleted successfully" });

                return BadRequest(new { success = false, message = "Failed to delete file" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("url")]
        public async Task<IActionResult> GetFileUrl([FromQuery] string publicId)
        {
            if (string.IsNullOrEmpty(publicId))
                return BadRequest("Public ID is required");

            try
            {
                var url = await _cloudinaryService.GetFileUrlAsync(publicId);
                return Ok(new { success = true, url });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }

}