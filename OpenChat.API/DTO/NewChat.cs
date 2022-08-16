namespace OpenChat.API.DTO
{
    public class NewChat
    {
        public IFormFile? Logo { get; set; }
        public string Name { get; set; } = null!;

        public string[]? Users { get; set; } 
    }
}
