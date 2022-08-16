namespace OpenChat.API.DTO
{
    public class NewChat
    {
        public string Name { get; set; } = null!;

        public string[]? Users { get; set; } 
    }
}
