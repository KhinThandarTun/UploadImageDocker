using System.Net;
using UploadImageDocker.Interface;
using UploadImageDocker.Service;

var builder = WebApplication.CreateBuilder(args);

#region Linux Config

OperatingSystem os = Environment.OSVersion;
string osPlatorm = os.Platform.ToString();
if (!osPlatorm.Contains("win", StringComparison.CurrentCultureIgnoreCase))
    _ = builder.WebHost
        .UseKestrel(options =>
        {
            options.Listen(IPAddress.Any, 80);
            options.Listen(IPAddress.Any, 443);
        })
        .UseContentRoot(Directory.GetCurrentDirectory());

#endregion

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IFileService, FileService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
