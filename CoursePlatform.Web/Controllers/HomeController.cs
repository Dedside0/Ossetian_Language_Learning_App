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
    public IActionResult AddCard(int courseId, int lessonId, string ossetianWord, string russianWord)
    {
        DataStore.AddCard(courseId, lessonId, ossetianWord, russianWord);
        return RedirectToAction("Constructor", new { courseId, lessonId });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
