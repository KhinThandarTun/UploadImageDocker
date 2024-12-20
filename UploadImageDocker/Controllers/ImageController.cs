using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Reflection;
using System.Text;
using UploadImageDocker.Interface;
using UploadImageDocker.Model;

namespace UploadImageDocker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController(IFileService fileService, IConfiguration configuration) : ControllerBase
    {
        private readonly IFileService _fileService = fileService;
        private readonly IConfiguration _configuration = configuration;

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile([FromForm] FileUploadDto model)
        {
            if (model.File == null || model.File.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var applicationPath = Path.Combine(_configuration.GetSection("Application_Path").Value ?? "");

            var uploadPath = Path.Combine(applicationPath, "wwwroot", "uploads");

            // Ensure the uploads directory exists
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            // Combine the path and the file name
            var filePath = Path.Combine(uploadPath, model.File.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.File.CopyToAsync(stream);
            }

            return Ok(new { message = "File uploaded successfully." });
        }

        [HttpPost("excel")]
        public async Task<IActionResult> UploadExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            var filePath = Path.Combine(_configuration.GetSection("Application_Path").Value ?? "", "wwwroot", "excel");

            // Ensure the "uploads" folder exists
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            var fullPath = Path.Combine(filePath, file.FileName);

            // Save the uploaded file temporarily
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Register the code pages encoding provider (required for Windows-1252 encoding)
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            // Read the uploaded Excel file
            using (var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
            {
                // Create an Excel reader
                using var reader = ExcelReaderFactory.CreateReader(stream);
                var result = reader.AsDataSet();
                var sheet = result.Tables[0];  // Read the first sheet

                // Process the data here (e.g., loop through rows and columns)
                foreach (DataRow row in sheet.Rows)
                {
                    // Example: Read each row's data
                    for (int col = 0; col < sheet.Columns.Count; col++)
                    {
                        var cellValue = row[col].ToString();
                        // Process the cell value as needed
                    }
                }

                // Return a success response with a message or data if required
                return Ok("Excel File uploaded and processed successfully");
            }
        }
    }
}

