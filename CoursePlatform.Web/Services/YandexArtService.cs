namespace CoursePlatform.Web.Services
{
    public class YandexArtService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<YandexArtService> _logger;
        private readonly string _folderId;

        public YandexArtService(IHttpClientFactory httpClientFactory, ILogger<YandexArtService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _folderId = Environment.GetEnvironmentVariable("YANDEX_PROJECT_ID");

            _logger.LogInformation($"YandexArtService initialized with FolderId: {_folderId}");
        }

        public async Task<string> GenerateMinimalistImageAsync(string actionDescription)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("YandexAIClient");

                // 1. Отправляем запрос на генерацию
                var requestBody = new
                {
                    modelUri = $"art://{_folderId}/yandex-art/latest",
                    generationOptions = new
                    {
                        mimeType = "image/jpeg",
                        seed = new Random().Next(1, 100000)
                    },
                    messages = new[]
                    {
                        new { weight = "1.0", text = $"Minimalist flat vector illustration, {actionDescription}, solid light background, simple shapes, clear concept, no small details, studio light" }
                    }
                };

                _logger.LogInformation("Sending generation request to YandexART API");
                var initResponse = await client.PostAsJsonAsync("https://llm.api.cloud.yandex.net/foundationModels/v1/imageGenerationAsync", requestBody);

                if (!initResponse.IsSuccessStatusCode)
                {
                    var errorContent = await initResponse.Content.ReadAsStringAsync();
                    _logger.LogError($"YandexART API error: {initResponse.StatusCode}, Content: {errorContent}");
                    return null;
                }

                var initResult = await initResponse.Content.ReadFromJsonAsync<ArtInitResponse>();
                string operationId = initResult?.Id;

                if (string.IsNullOrEmpty(operationId))
                {
                    _logger.LogError("Failed to get operation ID from YandexART response");
                    return null;
                }

                _logger.LogInformation($"Generation started, operation ID: {operationId}");

                // 2. Проверяем статус операции (увеличим количество попыток)
                for (int i = 0; i < 15; i++) // 15 попыток = 30 секунд
                {
                    await Task.Delay(2000);

                    var statusResponse = await client.GetAsync($"https://llm.api.cloud.yandex.net/operations/{operationId}");
                    if (!statusResponse.IsSuccessStatusCode)
                    {
                        _logger.LogWarning($"Status check failed: {statusResponse.StatusCode}");
                        continue;
                    }

                    var statusResult = await statusResponse.Content.ReadFromJsonAsync<ArtStatusResponse>();

                    if (statusResult != null && statusResult.Done)
                    {
                        if (string.IsNullOrEmpty(statusResult.Response?.Image))
                        {
                            _logger.LogError("Operation completed but no image was generated");
                            return null;
                        }

                        _logger.LogInformation("Image generated successfully");
                        return statusResult.Response.Image;
                    }
                }

                _logger.LogError($"Timeout waiting for image generation. Operation ID: {operationId}");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating image");
                return null;
            }
        }
    }

    // Классы для десериализации ответа YandexART
    public class ArtInitResponse { public string Id { get; set; } }

    public class ArtStatusResponse
    {
        public bool Done { get; set; }
        public ArtResponseContent Response { get; set; }
        public ArtError Error { get; set; }
    }

    public class ArtResponseContent { public string Image { get; set; } }

    public class ArtError { public string Message { get; set; } }
}
