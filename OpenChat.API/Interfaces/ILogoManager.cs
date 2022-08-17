namespace OpenChat.API.Interfaces
{
    public interface ILogoManager
    {
        public string DefaultUrl { get; }

        public string Create(IFormFile file);

        public void Delete(Guid chatId);
    }
}
