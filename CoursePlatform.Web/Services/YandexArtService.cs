namespace CoursePlatform.Web.Services
{
    public class YandexArtService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _folderId;

        public YandexArtService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _folderId = Environment.GetEnvironmentVariable("YANDEX_PROJECT_ID");
        }

        public async Task<string> GenerateMinimalistImageAsync(string actionDescription)
        {
            var client = _httpClientFactory.CreateClient("YandexAIClient");

            // 1. Отправляем запрос на генерацию
            var requestBody = new
            {
                modelUri = $"art://{_folderId}/yandexart/latest",
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
            var initResponse = await client.PostAsJsonAsync("https://llm.api.cloud.yandex.net/foundationModels/v1/imageGenerationAsync", requestBody);

            if (!initResponse.IsSuccessStatusCode) return null;

            var initResult = await initResponse.Content.ReadFromJsonAsync<ArtInitResponse>();
            string operationId = initResult?.Id;

            // 2. Пулleader: проверяем статус операции в цикле (максимум 20 секунд)
            for (int i = 0; i < 10; i++)
            {
                await Task.Delay(2000);

                var statusResponse = await client.GetAsync($"https://llm.api.cloud.yandex.net/operations/{operationId}");
                if (!statusResponse.IsSuccessStatusCode) continue;

                var statusResult = await statusResponse.Content.ReadFromJsonAsync<ArtStatusResponse>();

                if (statusResult != null && statusResult.Done)
                {
                    return statusResult.Response?.Image;
                }
            }

            return null; 
        }
    }
    // Классы для десериализации ответа YandexART
    public class ArtInitResponse { public string Id { get; set; } }
    public class ArtStatusResponse
    {
        public bool Done { get; set; }
        public ArtResponseContent Response { get; set; }
    }
    public class ArtResponseContent { public string Image { get; set; } }
}
