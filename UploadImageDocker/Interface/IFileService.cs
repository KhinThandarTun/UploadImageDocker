namespace UploadImageDocker.Interface
{
    public interface IFileService
    {
        bool WriteImage(IFormFile file, string name, string folder);
    }
}
