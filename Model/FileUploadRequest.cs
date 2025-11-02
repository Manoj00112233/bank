namespace Banking_CapStone.Model
{
    public class FileUploadRequest
    {
        public IFormFile File { get; set; }

        public string? FolderName { get; set; } = "documents";
    }
}
