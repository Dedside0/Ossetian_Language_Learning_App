using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Adam.Models;
using Adam.Data;
using CoursePlatform.Web.Models;

namespace Adam.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        var courses = DataStore.GetCourses();
        return View(courses);
    }

    public IActionResult Lessons(int courseId)
    {
        var course = DataStore.GetCourse(courseId);
        if (course == null) return RedirectToAction("Index");
        return View(course);
    }

    public IActionResult Cards(int courseId, int lessonId)
    {
        var lesson = DataStore.GetLesson(courseId, lessonId);
        if (lesson == null) return RedirectToAction("Index");
        ViewBag.CourseId = courseId;
        ViewBag.LessonId = lessonId;
        return View(lesson.Cards);
    }

    public IActionResult Conjugation(int courseId, int lessonId)
    {
        var lesson = DataStore.GetLesson(courseId, lessonId);
        if (lesson == null) return RedirectToAction("Index");
        ViewBag.CourseId = courseId;
        ViewBag.LessonId = lessonId;
        return View(lesson.ConjugationQuestions);
    }

    [HttpPost]
    public IActionResult CreateCourse(string title, string description)
    {
        var course = DataStore.AddCourse(
            string.IsNullOrWhiteSpace(title) ? "Новый курс" : title,
            string.IsNullOrWhiteSpace(description) ? "Пользовательский курс" : description
        );
        return RedirectToAction("Constructor", new { courseId = course.Id, lessonId = course.Lessons[0].Id });
    }

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
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
