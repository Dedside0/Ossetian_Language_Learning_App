using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Adam.Models;
using Adam.Data;
using CoursePlatform.Web.Models;

namespace Adam.Controllers;

public class HomeController : Controller
{
    // Отображение главной страницы со списком всех курсов
    public IActionResult Index()
    {
        var courses = DataStore.GetCourses();
        return View(courses);
    }

    // Отображение списка уроков для конкретного курса
    public IActionResult Lessons(int courseId)
    {
        var course = DataStore.GetCourse(courseId);
        if (course == null) return RedirectToAction("Index");
        return View(course);
    }

    // Просмотр обучающих карточек слов для выбранного урока
    public IActionResult Cards(int courseId, int lessonId)
    {
        var lesson = DataStore.GetLesson(courseId, lessonId);
        if (lesson == null) return RedirectToAction("Index");
        ViewBag.CourseId = courseId;
        ViewBag.LessonId = lessonId;
        return View(lesson.Cards);
    }

    // Страница с тестом на спряжение осетинских глаголов
    public IActionResult Conjugation(int courseId, int lessonId)
    {
        var lesson = DataStore.GetLesson(courseId, lessonId);
        if (lesson == null) return RedirectToAction("Index");
        ViewBag.CourseId = courseId;
        ViewBag.LessonId = lessonId;
        return View(lesson.ConjugationQuestions);
    }

    // GET: Отображение страницы прохождения заданий на аудирование
    public IActionResult Listening(int courseId, int lessonId)
    {
        var lesson = DataStore.GetLesson(courseId, lessonId);
        if (lesson == null) return RedirectToAction("Index");

        ViewBag.CourseId = courseId;
        ViewBag.LessonId = lessonId;

        return View(lesson.ListeningTasks);
    }

    // POST: Валидация ответов пользователя на задания по аудированию
    [HttpPost]
    public IActionResult VerifyListeningAnswers(int courseId, int lessonId, Dictionary<int, string> userAnswers)
    {
        var lesson = DataStore.GetLesson(courseId, lessonId);
        if (lesson == null) return RedirectToAction("Index");

        // Словарь для хранения результатов проверки: Key = ID задания, Value = (IsCorrect, Правильный ответ)
        var results = new Dictionary<int, (bool IsCorrect, string CorrectText)>();

        foreach (var task in lesson.ListeningTasks)
        {
            userAnswers.TryGetValue(task.Id, out var userText);

            // Очистка строк от лишних пробелов и приведение к нижнему регистру
            string cleanUser = (userText ?? "").Trim().ToLower();
            string cleanCorrect = (task.AudioDecoding ?? "").Trim().ToLower();

            // Толерантность к раскладке: заменяем русскую 'е' на осетинскую 'æ'
            cleanUser = cleanUser.Replace("е", "æ");
            cleanCorrect = cleanCorrect.Replace("е", "æ");

            bool isCorrect = cleanUser == cleanCorrect;
            results[task.Id] = (isCorrect, task.AudioDecoding);
        }

        ViewBag.CourseId = courseId;
        ViewBag.LessonId = lessonId;
        ViewBag.ValidationResults = results; // Передаем результаты во View для подсветки ошибок

        return View("Listening", lesson.ListeningTasks);
    }

    // POST: Создание нового пользовательского курса
    [HttpPost]
    public IActionResult CreateCourse(string title, string description)
    {
        var course = DataStore.AddCourse(
            string.IsNullOrWhiteSpace(title) ? "Новый курс" : title,
            string.IsNullOrWhiteSpace(description) ? "Пользовательский курс" : description
        );
        return RedirectToAction("Constructor", new { courseId = course.Id, lessonId = course.Lessons[0].Id });
    }

    // Страница конструктора для редактирования контента урока
    public IActionResult Constructor(int courseId, int lessonId)
    {
        var lesson = DataStore.GetLesson(courseId, lessonId);
        if (lesson == null) return RedirectToAction("Index");
        ViewBag.CourseId = courseId;
        ViewBag.LessonId = lessonId;
        return View(lesson);
    }
    [HttpPost]
    public IActionResult UpdateTheory(int courseId, int lessonId, string theoryHtml)
    {
        DataStore.UpdateLessonTheory(courseId, lessonId, theoryHtml);
        return RedirectToAction("Constructor", new { courseId, lessonId });
    }
    [HttpPost]
    public IActionResult AddCard(int courseId, int lessonId, string ossetianWord, string russianWord, string audioUrl, string imageUrl)
    {
        if (string.IsNullOrWhiteSpace(ossetianWord) || string.IsNullOrWhiteSpace(russianWord))
        {
            return RedirectToAction("Constructor", new { courseId, lessonId });
        }

        DataStore.AddCard(courseId, lessonId, ossetianWord, russianWord, audioUrl, imageUrl);
        return RedirectToAction("Constructor", new { courseId, lessonId });
    }
    [HttpPost]
    public IActionResult AddConjugationQuestion(int courseId, int lessonId, string russianSentence, string ossetianAnswer, string? hint)
    {
        if (string.IsNullOrWhiteSpace(russianSentence) || string.IsNullOrWhiteSpace(ossetianAnswer))
        {
            // Здесь можно добавить вывод ошибки через TempData, если поля пустые
            return RedirectToAction("Constructor", new { courseId, lessonId });
        }

        try
        {
            DataStore.AddConjugationQuestion(courseId, lessonId, russianSentence, ossetianAnswer, hint);
        }
        catch (Exception ex)
        {
            // Обработка ошибки, если урок не найден
        }

        return RedirectToAction("Constructor", new { courseId, lessonId });
    }
    [HttpPost]
    public IActionResult AddListeningTask(int courseId, int lessonId, string audioUrl, string audioDecoding, string? russianTranslation)
    {
        if (string.IsNullOrWhiteSpace(audioUrl) || string.IsNullOrWhiteSpace(audioDecoding))
        {
            TempData["Error"] = "Пожалуйста, заполните аудиофайл и расшифровку";
            return RedirectToAction("Constructor", new { courseId, lessonId });
        }

        DataStore.AddListeningTask(courseId, lessonId, audioUrl, audioDecoding, russianTranslation);
        TempData["Success"] = "Задание на аудирование успешно добавлено";
        return RedirectToAction("Constructor", new { courseId, lessonId });
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}