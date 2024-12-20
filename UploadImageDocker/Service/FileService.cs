using Microsoft.Extensions.Configuration;
using UploadImageDocker.Interface;
using System.Drawing;
using System.Drawing.Imaging;

namespace UploadImageDocker.Service
{
    public class FileService(ILogger<FileService> logger, IConfiguration configuration) : IFileService
    {
        private readonly ILogger<FileService> _logger = logger;
        private readonly IConfiguration _configuration = configuration;

        public bool WriteImage(IFormFile file, string name, string folder)
        {
            try
            {
                #region Path Built

                string pathBuilt = Path.Combine(_configuration.GetSection("Application_Path").Value ?? "", $"wwwroot\\images\\{folder}\\");

                if (!Directory.Exists(pathBuilt))
                {
                    _ = Directory.CreateDirectory(pathBuilt);
                }

                #endregion

                string extension = ("." + file.FileName.Split('.')[^1]).ToLower();
                string path = pathBuilt + name + extension;

                _ = FileDelete(path);
                Stream stream = file.OpenReadStream();

                #region Image Compress
                using Bitmap bmpImage = new(stream);
                if (extension is ".jpg" or ".jpeg")
                {
                    ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);

                    Encoder QualityEncoder = Encoder.Quality;

                    EncoderParameters myEncoderParameters = new(1);

                    EncoderParameter myEncoderParameter = new(QualityEncoder, 40L);
                    myEncoderParameters.Param[0] = myEncoderParameter;
                    bmpImage.Save(path, jpgEncoder, myEncoderParameters);

                    myEncoderParameters.Dispose();
                }

                if (extension == ".png")
                {
                    ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Png);

                    Encoder QualityEncoder = Encoder.Quality;

                    EncoderParameters myEncoderParameters = new(1);

                    EncoderParameter myEncoderParameter = new(QualityEncoder, 40L);
                    myEncoderParameters.Param[0] = myEncoderParameter;
                    bmpImage.Save(path, jpgEncoder, myEncoderParameters);

                    myEncoderParameters.Dispose();
                }

                stream.Dispose();
                bmpImage.Dispose();

                #endregion

                _logger.LogInformation($"Image Write Successfully [Route:wwwroot\\images\\{folder}\\{name}]");

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        public bool FileDelete(string path)
        {
            FileInfo file = new(path);
            if (file.Exists) //check file exsit or not 
            {
                file.Delete();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
