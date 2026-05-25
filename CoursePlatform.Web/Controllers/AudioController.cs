using Microsoft.AspNetCore.Mvc;

namespace CoursePlatform.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AudioController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public AudioController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadAudio(IFormFile audioFile)
        {
            try
            {
                if (audioFile == null || audioFile.Length == 0)
                {
                    return BadRequest(new { success = false, error = "Файл не загружен" });
                }

                string filename = $"{Guid.NewGuid()}_{audioFile.FileName}";
                string audiodir = Path.Combine(_env.WebRootPath, "uploads", "audio");

                if (!Directory.Exists(audiodir))
                {
                    Directory.CreateDirectory(audiodir);
                }

                string filepath = Path.Combine(audiodir, filename);

                // ОБЯЗАТЕЛЬНО: Сохраняем файл на диск
                using (var stream = new FileStream(filepath, FileMode.Create))
                {
                    await audioFile.CopyToAsync(stream);
                }

                // ОБЯЗАТЕЛЬНО: Возвращаем относительный URL для фронтенда
                var fileUrl = $"/uploads/audio/{filename}";
                return Ok(new { success = true, fileUrl = fileUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }
    }
}
