using OAIMVC.Models;

namespace OAIMVC.ViewModels
{
    public class ChatViewModel
    {
        public string Input { get; set; }
        public string Response { get; set; }
        public List<ChatEntry> History { get; set; } = new();
    }
}
