using CoursePlatform.Web.Services;
using Microsoft.AspNetCore.HttpOverrides; // ИСПРАВЛЕНО: добавили пространство имен

namespace CoursePlatform.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DotNetEnv.Env.Load();
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpClient("YandexAIClient", client =>
            {
                client.BaseAddress = new Uri("https://llm.api.cloud.yandex.net/");
                var apiKey = Environment.GetEnvironmentVariable("YANDEX_API_KEY");
                client.DefaultRequestHeaders.Add("Authorization", $"Api-Key {apiKey}");
            });
            builder.Services.AddScoped<YandexGptService>();
            builder.Services.AddScoped<YandexArtService>();
            var app = builder.Build();

            // ИСПРАВЛЕНО: Настройка обработки заголовков от Nginx. 
            // Должна стоять на самом верху конвейера!
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days.
                app.UseHsts();
            }
            else
            {
                // ИСПРАВЛЕНО: Перенаправление на HTTPS работает ТОЛЬКО при локальной разработке
                app.UseHttpsRedirection();
            }

            app.UseRouting();
            app.UseStaticFiles();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
