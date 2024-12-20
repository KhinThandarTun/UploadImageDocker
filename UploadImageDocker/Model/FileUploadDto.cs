namespace UploadImageDocker.Model
{
    public class FileUploadDto
    {
        public IFormFile? File { get; set; }
        public string? Description { get; set; }
    }
}
