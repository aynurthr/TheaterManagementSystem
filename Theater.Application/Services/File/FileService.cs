using Theater.Infrastructure.Abstracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Theater.Application.Services
{
    public class FileService : IFileService
    {
        private readonly IHostEnvironment env;

        public FileService(IHostEnvironment env)
        {
            this.env = env;
        }

        public async Task<string> UploadAsync(IFormFile file)
        {
            string extension = Path.GetExtension(file.FileName); //.jpg
            string randomFileName = $"{Guid.NewGuid()}{extension}";
            string uploadsPath = Path.Combine(env.ContentRootPath, "wwwroot", "uploads", "images");

            // Ensure the upload directory exists
            if (!Directory.Exists(uploadsPath))
            {
                Directory.CreateDirectory(uploadsPath);
            }

            string fullName = Path.Combine(uploadsPath, randomFileName);

            using (var fs = new FileStream(fullName, FileMode.Create, FileAccess.Write))
            {
                await file.CopyToAsync(fs);
            }

            return randomFileName;
        }

        public async Task<string> ChangeFileAsync(string oldFileName, IFormFile file)
        {
            string oldFilePath = ResolveOldFilePath(oldFileName);
            string archivePath = Path.Combine(env.ContentRootPath, "wwwroot", "uploads", "images", "archive");

            // Ensure the archive directory exists
            if (!Directory.Exists(archivePath))
            {
                Directory.CreateDirectory(archivePath);
            }

            if (File.Exists(oldFilePath))
            {
                string archiveFileName = $"archive-{Path.GetFileName(oldFilePath)}";
                string archiveFilePath = Path.Combine(archivePath, archiveFileName);

                File.Move(oldFilePath, archiveFilePath);
            }

            return await UploadAsync(file);
        }

        private string ResolveOldFilePath(string oldFileName)
        {
            string uploadsPath = Path.Combine(env.ContentRootPath, "wwwroot", "uploads", "images");
            string assetsPath = Path.Combine(env.ContentRootPath, "wwwroot", "assets", "media", "images");

            // Check if the old filename contains a relative path
            if (oldFileName.StartsWith(".."))
            {
                // Convert the relative path to an absolute path
                string relativePath = oldFileName.Replace("../", string.Empty).Replace("/", Path.DirectorySeparatorChar.ToString());
                return Path.Combine(env.ContentRootPath, relativePath);
            }

            // Default to the uploads path if no relative path is found
            return Path.Combine(uploadsPath, oldFileName);
        }
    }
}
