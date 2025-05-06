namespace MyPortfolio.Models.Services
{
    public class FileHandling
    {
        private readonly IWebHostEnvironment _env;
        public FileHandling(IWebHostEnvironment env)
        {
            _env = env;
        }
        public async Task<string> Uploadfile(IFormFile? file, string? folderName)
        {
            string fileName = null;
            if (file != null)
            {
                string uploadDr = Path.Combine(_env.WebRootPath, folderName);
                fileName = Guid.NewGuid().ToString() + "=" + file.FileName;
                string filePath = Path.Combine(uploadDr, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

            }
            return fileName;
        }

        public async Task DeleteFile(string folderName, string fileName)
        {
            if (!string.IsNullOrEmpty(fileName) && !(fileName.Contains("DefaultProfilePicture.png") || fileName.Contains("MyResumeDefault.pdf")))
            {
                string filePath = Path.Combine(_env.WebRootPath, folderName, fileName);
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }
           
        }
        public async Task<byte[]> DownloadFile(string folderName, string fileName)
        {
            string filePath = Path.Combine(_env.WebRootPath, folderName, fileName);
            if (File.Exists(filePath))
            {
                return await File.ReadAllBytesAsync(filePath);
            }
            else
            {
                throw new FileNotFoundException("The specified file was not found.");
            }
        }
    }
}
