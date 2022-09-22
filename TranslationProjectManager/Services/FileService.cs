namespace TranslationProjectManager.Services
{
    public enum FileType
    {
        ProjectFile
    }
    
    public interface IFileStorageConfigurationService
    {
        string Root { get; }
        string GetFilePath(FileType fileType, string fileName);
    }

    public class FileStorageConfigurationService : IFileStorageConfigurationService
    {
        public string Root { get; set; }

        private readonly Dictionary<FileType, string> storageDirectories = new Dictionary<FileType, string>();
        
        public FileStorageConfigurationService(IConfiguration configuration)
        {
            Root = configuration["FileStorage:Root"];

            storageDirectories.Add(FileType.ProjectFile, configuration["FileStorage:ProjectFile"]);
        }

        public string GetFilePath(FileType fileType, string fileName)
        {
            return Path.Combine(Root, storageDirectories[fileType], fileName);
        }
    }
    
    public interface IFileStorageService
    {
        Task<string> SaveFile(FileType fileType, Stream stream);
        Task<Stream> GetFile(string path);
        Task<bool> DeleteFile(string path);
    }

    public class FileStorageService : IFileStorageService
    {
        private readonly IFileStorageConfigurationService configurationService;

        public FileStorageService(IFileStorageConfigurationService configurationService)
        {
            this.configurationService = configurationService;
        }

        public async Task<string> SaveFile(FileType fileType, Stream stream)
        {
            var filePath = configurationService.GetFilePath(fileType, Guid.NewGuid().ToString() + ".dat");
            var directoryPath = Path.GetDirectoryName(filePath);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await stream.CopyToAsync(fileStream);
            }

            return filePath;
        }

        public async Task<Stream> GetFile(string path)
        {
            if (!File.Exists(path))
            {
                return null;
            }

            return File.OpenRead(path);
        }

        public async Task<bool> DeleteFile(string path)
        {
            if (!File.Exists(path))
            {
                return false;
            }

            File.Delete(path);

            return true;
        }
    }
}
