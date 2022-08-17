using OpenChat.API.Interfaces;

namespace OpenChat.API.Managers
{
    public class LogoManager : ILogoManager
    {
        private readonly IWebHostEnvironment env;

        private string basePath = "https://localhost:7236/ChatsLogo";
        private string defaultLogoName = "default.jpg";

        public LogoManager(IWebHostEnvironment env)
        {
            this.env = env;
        }

        /// <summary>
        /// Return url for default log
        /// </summary>
        public string DefaultUrl => $"{basePath}/{defaultLogoName}";
        
        /// <summary>
        /// Create new logo in files
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public string Create(IFormFile file)
        {
            string type = file.FileName.Substring(file.FileName.LastIndexOf('.') + 1);
            string fileName = $"{Guid.NewGuid()}.{type}";
            string path = Path.Combine(env.WebRootPath, "ChatsLogo", fileName);
            using (FileStream fs = System.IO.File.Create(path))
            {
                file.CopyTo(fs);
                fs.Close();
            }
            string logoUrl = $"{basePath}/{fileName}";
            return logoUrl;
        }

        /// <summary>
        /// Delete logo by its name
        /// </summary>
        /// <param name="chatId"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Delete(Guid chatId)
        {
            throw new NotImplementedException();
        }
    }
}
