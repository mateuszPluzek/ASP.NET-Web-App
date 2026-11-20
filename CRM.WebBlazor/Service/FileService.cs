using Microsoft.AspNetCore.Components.Forms;

namespace CRM.WebBlazor.Service
{
    public class FileService(IWebHostEnvironment webHostEnvironment) : IFileService
    {
        private readonly string[] _allowedImageTypes = ["image/jpeg", "image/png", "image/gif"];
        private const long MaxAllowedFileSize = 5 * 1024 * 1025; //5MB

        public async Task DeleteImageAsync(string container, string fileName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fileName) || fileName.Any(c => Path.GetInvalidFileNameChars().Contains(c)))
                    throw new InvalidOperationException("Invalid file name.");

                var folderPath = Path.Combine(webHostEnvironment.WebRootPath, "images", container);
                var filePath = Path.Combine(folderPath, fileName);

                if (File.Exists(filePath))
                {
                    await Task.Run(() => File.Delete(filePath));
                }
                else
                {
                    throw new FileNotFoundException($"File '{fileName}' not found in container '{container}'.");
                }
            }
            catch (Exception)
            {
                throw new ApplicationException("An error occurred while deleting the image.");
            }
        }


        public async Task<string> ReadFileAsync(string path)
        {
            if(!File.Exists(path))
            {
                throw new FileNotFoundException($"File not found: {path}");
            }
            return await File.ReadAllTextAsync(path);
        }

        public async Task<string> UploadImageAsync(IBrowserFile file, string container)
        {
            try
            {
                if(file.Size > MaxAllowedFileSize)
                {
                    throw new InvalidOperationException("Fiel is too large.");
                }
                if(!_allowedImageTypes.Contains(file.ContentType))
                {
                    throw new InvalidOperationException("Unsuported fiel type.");
                }

                IBrowserFile resizedFile = file;
                if (file.Size > 1024 * 1024)
                    resizedFile = await file.RequestImageFileAsync(file.ContentType, 500, 500);

                using var stream = new MemoryStream();
                await resizedFile.OpenReadStream().CopyToAsync(stream);

                var fileExstension = Path.GetExtension(file.Name);
                var fileName = $"{Guid.NewGuid()}{fileExstension}";

                if(fileName.Any(c => Path.GetInvalidFileNameChars().Contains(c)))
                    throw new InvalidOperationException("Invalid characters in file name.");

                var folderPath = Path.Combine(webHostEnvironment.WebRootPath, "images", container);
                if(!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                var filePath = Path.Combine(folderPath, fileName);
                await File.WriteAllBytesAsync(filePath, stream.ToArray());

                return fileName;
            }
            catch (Exception)
            {
                throw new ApplicationException("An error has occured while uploading the image.");
            }
        }
    }


}
