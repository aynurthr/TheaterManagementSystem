using Theater.Infrastructure.Abstracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace Theater.Application.Services.File
{
    public class FileService : IFileService
    {
        private readonly IHostEnvironment env;

        public FileService(IHostEnvironment env)
        {
            this.env = env;
        }

        public async Task<string> UploadAsyncImage(IFormFile file)
        {
            string extension = Path.GetExtension(file.FileName); //.jpg
            string randomFileName = $"{Guid.NewGuid()}{extension}";
            string fullName = Path.Combine(env.ContentRootPath, "wwwroot", "uploads", "images", randomFileName);

            using (var fs = new FileStream(fullName, FileMode.Create, FileAccess.Write))
            {
                await file.CopyToAsync(fs);
            }

            return randomFileName;
        }

        public async Task<string> UploadAsyncFile(IFormFile file)
        {
            string extension = Path.GetExtension(file.FileName); //.pdf
            string randomFileName = $"{Guid.NewGuid()}{extension}";
            string fullName = Path.Combine(env.ContentRootPath, "wwwroot", "uploads", "files", randomFileName);

            using (var fs = new FileStream(fullName, FileMode.Create, FileAccess.Write))
            {
                await file.CopyToAsync(fs);
            }

            return randomFileName;
        }


        public Task<string> ChangeFileAsyncImage(string oldFileName, IFormFile file)
        {
            string oldFilePath = Path.Combine(env.ContentRootPath, "wwwroot", "uploads", "images", oldFileName);

            if (System.IO.File.Exists(oldFilePath))
            {
                string archiveFilePath = Path.Combine(env.ContentRootPath, "wwwroot", "uploads", "images", $"archive-{oldFileName}");

                System.IO.File.Move(oldFilePath, archiveFilePath);
            }

            return UploadAsyncImage(file);
        }

        public Task<string> ChangeFileAsyncFile(string oldFileName, IFormFile file)
        {
            string oldFilePath = Path.Combine(env.ContentRootPath, "wwwroot", "uploads", "files", oldFileName);

            if (System.IO.File.Exists(oldFilePath))
            {
                string archiveFilePath = Path.Combine(env.ContentRootPath, "wwwroot", "uploads", "files", $"archive-{oldFileName}");

                System.IO.File.Move(oldFilePath, archiveFilePath);
            }

            return UploadAsyncImage(file);
        }
    }
}
