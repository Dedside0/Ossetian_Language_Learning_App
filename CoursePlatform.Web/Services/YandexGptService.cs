namespace CoursePlatform.Web.Services
{
    public class YandexGptService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _folderId;

        public YandexGptService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _folderId = Environment.GetEnvironmentVariable("YANDEX_PROJECT_ID");
        }

        public async Task<string> GenerateSentenceExampleAsync(string verb)
        {
            var client = _httpClientFactory.CreateClient("YandexAIClient");

            var requestBody = new
            {
                modelUri = $"gpt://{_folderId}/yandexgpt-lite/latest",
                completionOptions = new
                {
                    stream = false,
                    temperature = 0.5,
                    maxTokens = "2000"
                },
                messages = new[]
                {
                new { role = "system", text = "Ты — помощник учителя осетинского языка. Напиши ОДИН простой пример предложения с указанным глаголом на осетинском языке и его перевод на русский." },
                new { role = "user", text = $"Глагол: {verb}" }
            }
            };

            var response = await client.PostAsJsonAsync("foundationModels/v1/completion", requestBody);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<GptResponse>();
                return result?.Result?.Alternatives?.FirstOrDefault()?.Message?.Text ?? "Не удалось сгенерировать пример.";
            }

            return $"Ошибка API: {response.StatusCode}";
        }
    }
    // Классы для десериализации ответа YandexGPT
    public class GptResponse { public GptResult Result { get; set; } }
    public class GptResult { public List<GptAlternative> Alternatives { get; set; } }
    public class GptAlternative { public GptMessage Message { get; set; } }
    public class GptMessage { public string Text { get; set; } }
}
