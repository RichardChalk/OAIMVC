using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using OAIMVC.Models;
using OAIMVC.ViewModels;
using OpenAI;
using System;
using System.Threading.Tasks;

namespace OAIMVC.Controllers
{
    public class ChatController : Controller
    {
        private readonly IChatCompletionService _chatService;

        public ChatController()
        {
            var apiKey = Environment.GetEnvironmentVariable("OpenAI-Key");
            _chatService = new OpenAIChatCompletionService("gpt-4o-mini", apiKey);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new ChatViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(ChatViewModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.Input))
            {
                var history = new ChatHistory();
                history.AddSystemMessage($"Today's date is {DateTime.UtcNow:yyyy-MM-dd}.");
                history.AddUserMessage("I was born may 21 1970");
                history.AddUserMessage(model.Input);

                var response = await _chatService.GetChatMessageContentAsync(history);

                model.History.Add(new ChatEntry
                {
                    Timestamp = DateTime.UtcNow,
                    Question = model.Input,
                    Answer = response.ToString()
                });

                model.History = model.History
                    .OrderByDescending(e => e.Timestamp)
                    .ToList(); 
            }
            
            // Jag vill rensa frågan efter varje submit!
            model.Input = string.Empty;
            ModelState.Clear();
            
            return View(model);
        }
    }
}
