using CoursePlatform.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoursePlatform.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly YandexGptService _gptService;

        public ChatController(YandexGptService gptService)
        {
            _gptService = gptService;
        }

        [HttpPost("ask")]
        public async Task<IActionResult> AskQuestion([FromBody] ChatRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Question))
            {
                return BadRequest(new { success = false, error = "Вопрос не может быть пустым" });
            }

            try
            {
                var answer = await _gptService.GetAssistantResponseAsync(request.Question);
                return Ok(new { success = true, answer });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Ошибка при обработке запроса. Попробуйте позже." });
            }
        }
    }

    public class ChatRequest
    {
        public string Question { get; set; }
    }
}
