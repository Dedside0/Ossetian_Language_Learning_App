using CoursePlatform.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoursePlatform.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;
        private readonly YandexArtService _yandexArtService;

        public ImageController(IWebHostEnvironment env, IConfiguration configuration, YandexArtService yandexArtService)
        {
            _env = env;
            _configuration = configuration;
            _yandexArtService = yandexArtService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(IFormFile imageFile)
        {
            try
            {
                if (imageFile == null || imageFile.Length == 0)
                    return BadRequest(new { success = false, error = "Файл не выбран" });
                var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(imageFile.FileName)}";
                var uploadsPath = Path.Combine(_env.WebRootPath, "uploads", "images");

                if (!Directory.Exists(uploadsPath))
                    Directory.CreateDirectory(uploadsPath);

                var filePath = Path.Combine(uploadsPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                var fileUrl = $"/uploads/images/{fileName}";

                return Ok(new { success = true, fileUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateImage([FromBody] GenerateImageRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request?.Prompt))
                {
                    return BadRequest(new { success = false, error = "Промпт не может быть пустым" });
                }

                // Здесь вызывается ваша функция генерации изображения
                var imageBase64 = await _yandexArtService.GenerateMinimalistImageAsync(request.Prompt);

                if (string.IsNullOrEmpty(imageBase64))
                {
                    return BadRequest(new { success = false, error = "Не удалось сгенерировать изображение. Проверьте настройки API Yandex." });
                }
                var fileName = $"{Guid.NewGuid()}_generated.jpg"; // YandexArt генерирует JPEG
                var uploadsPath = Path.Combine(_env.WebRootPath, "uploads", "images");

                if (!Directory.Exists(uploadsPath))
                    Directory.CreateDirectory(uploadsPath);

                var filePath = Path.Combine(uploadsPath, fileName);
                var imageBytes = Convert.FromBase64String(imageBase64);
                await System.IO.File.WriteAllBytesAsync(filePath, imageBytes);

                var fileUrl = $"/uploads/images/{fileName}";

                return Ok(new { success = true, imageUrl = fileUrl });
            }
            catch (FormatException ex)
            {
                // Ошибка при конвертации Base64
                return StatusCode(500, new { success = false, error = "Ошибка формата полученного изображения" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }
    }

    public class GenerateImageRequest
    {
        public string Prompt { get; set; }
    }
}
