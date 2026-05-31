using Adam.Models;

namespace Adam.Data;

public static class DataStore
{
    private static List<Course> _courses = new();
    private static int _nextCourseId = 100;
    private static int _nextLessonId = 100;
    private static int _nextCardId = 1000;
    private static int _nextQuestionId = 5000;

    static DataStore()
    {
        InitializeDefaultCourses();
    }

    public static List<Course> GetCourses() => _courses;

    public static Course? GetCourse(int id) => _courses.FirstOrDefault(c => c.Id == id);

    public static Course AddCourse(string title, string description)
    {
        var course = new Course
        {
            Id = _nextCourseId++,
            Title = title,
            Description = description,
            Icon = "✏️",
            IsUserCreated = true,
            Lessons = new List<Lesson>
            {
                new Lesson
                {
                    Id = _nextLessonId++,
                    Title = "Урок 1",
                    TheoryHtml = "<p>Добавьте задания в этот урок с помощью конструктора карточек.</p>",
                    Cards = new List<Card>(),
                    ConjugationQuestions = new List<ConjugationQuestion>()
                }
            }
        };
        _courses.Add(course);
        return course;
    }

    public static Lesson? GetLesson(int courseId, int lessonId)
    {
        var course = GetCourse(courseId);
        return course?.Lessons.FirstOrDefault(l => l.Id == lessonId);
    }

    public static Card AddCard(int courseId, int lessonId, string ossetianWord, string russianWord, string audioUrl)
    {
        var lesson = GetLesson(courseId, lessonId);
        if (lesson == null) throw new Exception("Lesson not found");

        var card = new Card
        {
            Id = _nextCardId++,
            OssetianWord = ossetianWord,
            RussianWord = russianWord,
            ImageUrl = $"https://via.placeholder.com/300x200?text={Uri.EscapeDataString(russianWord)}",
            AudioUrl = audioUrl
        };
        lesson.Cards.Add(card);
        return card;
    }
    public static ConjugationQuestion AddConjugationQuestion(int courseId, int lessonId, string russianSentence, string ossetianAnswer, string? hint)
    {
        var lesson = GetLesson(courseId, lessonId);
        if (lesson == null) throw new Exception("Lesson not found");

        var question = new ConjugationQuestion
        {
            Id = _nextQuestionId++,
            RussianSentence = russianSentence.Trim(),
            OssetianAnswer = ossetianAnswer.Trim(),
            Hint = hint?.Trim(),
            ImageUrl = null
        };

        lesson.ConjugationQuestions ??= new List<ConjugationQuestion>();

        lesson.ConjugationQuestions.Add(question);
        return question;
    }
    private static void InitializeDefaultCourses()
    {
        _courses = new List<Course>
        {
            new Course
            {
                Id = 1,
                Title = "Осетинские глаголы",
                Description = "Базовый курс осетинских глаголов. 47 карточек и тест на спряжение.",
                Icon = "🇬🇪",
                Lessons = new List<Lesson>
                {
                    new Lesson
                    {
                        Id = 1,
                        Title = "Урок 1. Основные глаголы",
                        TheoryHtml = GetTheoryHtml(),
                        Cards = GetDefaultCards(),
                        ConjugationQuestions = GetConjugationQuestions()
                    }
                }
            },
            new Course
            {
                Id = 2,
                Title = "Осетинские приветствия",
                Description = "Научитесь приветствовать и прощаться на осетинском языке.",
                Icon = "👋",
                Lessons = new List<Lesson>
                {
                    new Lesson
                    {
                        Id = 2,
                        Title = "Урок 1. Приветствия и прощания",
                        TheoryHtml = "<h4>Приветствия в осетинском языке</h4>" +
                            "<p>Осетинский язык имеет богатую систему приветствий, которые меняются в зависимости от времени суток и ситуации.</p>" +
                            "<ul><li><strong>Салам!</strong> — Привет! (неформальное)</li>" +
                            "<li><strong>Де бон хорз!</strong> — Добрый день!</li>" +
                            "<li><strong>Дæ изæр хорз!</strong> — Добрый вечер!</li></ul>",
                        Cards = new List<Card>
                        {
                            new Card { Id = 100, OssetianWord = "Салам", RussianWord = "Привет", ImageUrl = "https://via.placeholder.com/300x200/4CAF50/fff?text=Привет" },
                            new Card { Id = 101, OssetianWord = "Де бон хорз", RussianWord = "Добрый день", ImageUrl = "https://via.placeholder.com/300x200/2196F3/fff?text=Добрый+день" },
                            new Card { Id = 102, OssetianWord = "Дæ изæр хорз", RussianWord = "Добрый вечер", ImageUrl = "https://via.placeholder.com/300x200/9C27B0/fff?text=Добрый+вечер" },
                            new Card { Id = 103, OssetianWord = "Хорз бон", RussianWord = "Хорошего дня", ImageUrl = "https://via.placeholder.com/300x200/FF9800/fff?text=Хорошего+дня" },
                            new Card { Id = 104, OssetianWord = "Фæндараст", RussianWord = "Счастливого пути", ImageUrl = "https://via.placeholder.com/300x200/E91E63/fff?text=Счастливого+пути" },
                        },
                        ConjugationQuestions = new List<ConjugationQuestion>()
                    }
                }
            },
            new Course
            {
                Id = 3,
                Title = "Числа и счёт",
                Description = "Изучите числа от 1 до 20 на осетинском языке.",
                Icon = "🔢",
                Lessons = new List<Lesson>
                {
                    new Lesson
                    {
                        Id = 3,
                        Title = "Урок 1. Числа от 1 до 10",
                        TheoryHtml = "<h4>Числа в осетинском языке</h4>" +
                            "<p>Система счёта в осетинском языке основана на двадцатеричной системе, что является отличительной чертой иранских языков Кавказа.</p>" +
                            "<ul><li><strong>Иу</strong> — 1</li><li><strong>Дыууæ</strong> — 2</li><li><strong>Æртæ</strong> — 3</li></ul>",
                        Cards = new List<Card>
                        {
                            new Card { Id = 200, OssetianWord = "Иу", RussianWord = "Один", ImageUrl = "https://via.placeholder.com/300x200/F44336/fff?text=1" },
                            new Card { Id = 201, OssetianWord = "Дыууæ", RussianWord = "Два", ImageUrl = "https://via.placeholder.com/300x200/E91E63/fff?text=2" },
                            new Card { Id = 202, OssetianWord = "Æртæ", RussianWord = "Три", ImageUrl = "https://via.placeholder.com/300x200/9C27B0/fff?text=3" },
                            new Card { Id = 203, OssetianWord = "Цыппар", RussianWord = "Четыре", ImageUrl = "https://via.placeholder.com/300x200/673AB7/fff?text=4" },
                            new Card { Id = 204, OssetianWord = "Фондз", RussianWord = "Пять", ImageUrl = "https://via.placeholder.com/300x200/3F51B5/fff?text=5" },
                            new Card { Id = 205, OssetianWord = "Æхсæз", RussianWord = "Шесть", ImageUrl = "https://via.placeholder.com/300x200/2196F3/fff?text=6" },
                            new Card { Id = 206, OssetianWord = "Авд", RussianWord = "Семь", ImageUrl = "https://via.placeholder.com/300x200/03A9F4/fff?text=7" },
                            new Card { Id = 207, OssetianWord = "Аст", RussianWord = "Восемь", ImageUrl = "https://via.placeholder.com/300x200/00BCD4/fff?text=8" },
                            new Card { Id = 208, OssetianWord = "Фараст", RussianWord = "Девять", ImageUrl = "https://via.placeholder.com/300x200/009688/fff?text=9" },
                            new Card { Id = 209, OssetianWord = "Дæс", RussianWord = "Десять", ImageUrl = "https://via.placeholder.com/300x200/4CAF50/fff?text=10" },
                        },
                        ConjugationQuestions = new List<ConjugationQuestion>()
                    }
                }
            }
        };
    }

    private static string GetTheoryHtml()
    {
        return @"
<h4>Глаголы в осетинском языке</h4>
<p>Осетинский язык принадлежит к иранской группе индоевропейской языковой семьи. Глаголы в осетинском языке имеют сложную систему спряжения.</p>

<h5>Типы глаголов</h5>
<ul>
    <li><strong>Переходные глаголы</strong> — действие направлено на объект (например: <em>хæрын</em> — есть, <em>нуазын</em> — пить)</li>
    <li><strong>Непереходные глаголы</strong> — действие не направлено на объект (например: <em>цæуын</em> — идти, <em>бадын</em> — сидеть)</li>
</ul>

<h5>Прошедшее время</h5>
<p>В прошедшем времени глагол изменяется по лицам и числам. Основные окончания:</p>
<table class='table table-sm'>
    <thead><tr><th>Лицо</th><th>Ед. число</th><th>Мн. число</th></tr></thead>
    <tbody>
        <tr><td>1-е</td><td>-тон</td><td>-там</td></tr>
        <tr><td>2-е</td><td>-тай</td><td>-тат</td></tr>
        <tr><td>3-е</td><td>-та</td><td>-той</td></tr>
    </tbody>
</table>

<h5>Примеры спряжения</h5>
<p><strong>Хæрын</strong> (есть) в прошедшем времени:</p>
<ul>
    <li>Æз бахордтон — Я поел</li>
    <li>Ды бахордтай — Ты поел</li>
    <li>Уый бахордта — Он поел</li>
</ul>
";
    }

    private static List<Card> GetDefaultCards()
    {
        return new List<Card>
        {
            new Card { Id = 1, OssetianWord = "Хæрын", RussianWord = "Есть (кушать)", VerbType = "Переходный", ImageUrl = "https://via.placeholder.com/300x200/FF5722/fff?text=Есть" },
            new Card { Id = 2, OssetianWord = "Нуазын", RussianWord = "Пить", VerbType = "Переходный", ImageUrl = "https://via.placeholder.com/300x200/2196F3/fff?text=Пить" },
            new Card { Id = 3, OssetianWord = "Цæуын", RussianWord = "Идти", VerbType = "Непереходный", ImageUrl = "https://via.placeholder.com/300x200/4CAF50/fff?text=Идти" },
            new Card { Id = 4, OssetianWord = "Бадын", RussianWord = "Сидеть", VerbType = "Непереходный", ImageUrl = "https://via.placeholder.com/300x200/9C27B0/fff?text=Сидеть" },
            new Card { Id = 5, OssetianWord = "Лæууын", RussianWord = "Стоять", VerbType = "Непереходный", ImageUrl = "https://via.placeholder.com/300x200/FF9800/fff?text=Стоять" },
            new Card { Id = 6, OssetianWord = "Хуыссын", RussianWord = "Спать", VerbType = "Непереходный", ImageUrl = "https://via.placeholder.com/300x200/607D8B/fff?text=Спать" },
            new Card { Id = 7, OssetianWord = "Дзурын", RussianWord = "Говорить", VerbType = "Непереходный", ImageUrl = "https://via.placeholder.com/300x200/E91E63/fff?text=Говорить" },
            new Card { Id = 8, OssetianWord = "Хъусын", RussianWord = "Слушать", VerbType = "Переходный", ImageUrl = "https://via.placeholder.com/300x200/00BCD4/fff?text=Слушать" },
            new Card { Id = 9, OssetianWord = "Кæсын", RussianWord = "Смотреть", VerbType = "Непереходный", ImageUrl = "https://via.placeholder.com/300x200/8BC34A/fff?text=Смотреть" },
            new Card { Id = 10, OssetianWord = "Фыссын", RussianWord = "Писать", VerbType = "Переходный", ImageUrl = "https://via.placeholder.com/300x200/3F51B5/fff?text=Писать" },
            new Card { Id = 11, OssetianWord = "Кæнын", RussianWord = "Делать", VerbType = "Переходный", ImageUrl = "https://via.placeholder.com/300x200/F44336/fff?text=Делать" },
            new Card { Id = 12, OssetianWord = "Зонын", RussianWord = "Знать", VerbType = "Переходный", ImageUrl = "https://via.placeholder.com/300x200/009688/fff?text=Знать" },
            new Card { Id = 13, OssetianWord = "Уарзын", RussianWord = "Любить", VerbType = "Переходный", ImageUrl = "https://via.placeholder.com/300x200/E91E63/fff?text=Любить" },
            new Card { Id = 14, OssetianWord = "Амондзын", RussianWord = "Учить (кого-то)", VerbType = "Переходный", ImageUrl = "https://via.placeholder.com/300x200/673AB7/fff?text=Учить" },
            new Card { Id = 15, OssetianWord = "Ахуыр кæнын", RussianWord = "Учиться", VerbType = "Непереходный", ImageUrl = "https://via.placeholder.com/300x200/3F51B5/fff?text=Учиться" },
            new Card { Id = 16, OssetianWord = "Æрбацæуын", RussianWord = "Приходить", VerbType = "Непереходный", ImageUrl = "https://via.placeholder.com/300x200/4CAF50/fff?text=Приходить" },
            new Card { Id = 17, OssetianWord = "Ацæуын", RussianWord = "Уходить", VerbType = "Непереходный", ImageUrl = "https://via.placeholder.com/300x200/FF5722/fff?text=Уходить" },
            new Card { Id = 18, OssetianWord = "Лæсын", RussianWord = "Ползти", VerbType = "Непереходный", ImageUrl = "https://via.placeholder.com/300x200/795548/fff?text=Ползти" },
            new Card { Id = 19, OssetianWord = "Тæхын", RussianWord = "Летать", VerbType = "Непереходный", ImageUrl = "https://via.placeholder.com/300x200/03A9F4/fff?text=Летать" },
            new Card { Id = 20, OssetianWord = "Ленк кæнын", RussianWord = "Плавать", VerbType = "Непереходный", ImageUrl = "https://via.placeholder.com/300x200/00BCD4/fff?text=Плавать" },
            new Card { Id = 21, OssetianWord = "Уайын", RussianWord = "Бежать", VerbType = "Непереходный", ImageUrl = "https://via.placeholder.com/300x200/FF9800/fff?text=Бежать" },
            new Card { Id = 22, OssetianWord = "Скъæфын", RussianWord = "Хватать", VerbType = "Переходный", ImageUrl = "https://via.placeholder.com/300x200/F44336/fff?text=Хватать" },
            new Card { Id = 23, OssetianWord = "Æвæрын", RussianWord = "Класть", VerbType = "Переходный", ImageUrl = "https://via.placeholder.com/300x200/607D8B/fff?text=Класть" },
            new Card { Id = 24, OssetianWord = "Исын", RussianWord = "Брать", VerbType = "Переходный", ImageUrl = "https://via.placeholder.com/300x200/9E9E9E/fff?text=Брать" },
            new Card { Id = 25, OssetianWord = "Дæттын", RussianWord = "Давать", VerbType = "Переходный", ImageUrl = "https://via.placeholder.com/300x200/8BC34A/fff?text=Давать" },
            new Card { Id = 26, OssetianWord = "Фидын", RussianWord = "Платить", VerbType = "Переходный", ImageUrl = "https://via.placeholder.com/300x200/FFC107/fff?text=Платить" },
            new Card { Id = 27, OssetianWord = "Æлхæнын", RussianWord = "Покупать", VerbType = "Переходный", ImageUrl = "https://via.placeholder.com/300x200/FF5722/fff?text=Покупать" },
            new Card { Id = 28, OssetianWord = "Уæй кæнын", RussianWord = "Продавать", VerbType = "Переходный", ImageUrl = "https://via.placeholder.com/300x200/795548/fff?text=Продавать" },
            new Card { Id = 29, OssetianWord = "Сафын", RussianWord = "Терять", VerbType = "Переходный", ImageUrl = "https://via.placeholder.com/300x200/9C27B0/fff?text=Терять" },
            new Card { Id = 30, OssetianWord = "Ссарын", RussianWord = "Находить", VerbType = "Переходный", ImageUrl = "https://via.placeholder.com/300x200/4CAF50/fff?text=Находить" },
            new Card { Id = 31, OssetianWord = "Уадзын", RussianWord = "Оставлять", VerbType = "Переходный", ImageUrl = "https://via.placeholder.com/300x200/607D8B/fff?text=Оставлять" },
            new Card { Id = 32, OssetianWord = "Æмбарын", RussianWord = "Понимать", VerbType = "Переходный", ImageUrl = "https://via.placeholder.com/300x200/2196F3/fff?text=Понимать" },
            new Card { Id = 33, OssetianWord = "Ферох кæнын", RussianWord = "Забывать", VerbType = "Переходный", ImageUrl = "https://via.placeholder.com/300x200/9E9E9E/fff?text=Забывать" },
            new Card { Id = 34, OssetianWord = "Хъæбæр кæнын", RussianWord = "Кричать", VerbType = "Непереходный", ImageUrl = "https://via.placeholder.com/300x200/F44336/fff?text=Кричать" },
            new Card { Id = 35, OssetianWord = "Зарын", RussianWord = "Петь", VerbType = "Непереходный", ImageUrl = "https://via.placeholder.com/300x200/E91E63/fff?text=Петь" },
            new Card { Id = 36, OssetianWord = "Кафын", RussianWord = "Танцевать", VerbType = "Непереходный", ImageUrl = "https://via.placeholder.com/300x200/AB47BC/fff?text=Танцевать" },
            new Card { Id = 37, OssetianWord = "Хæдзаронд кæнын", RussianWord = "Готовить (еду)", VerbType = "Переходный", ImageUrl = "https://via.placeholder.com/300x200/FF9800/fff?text=Готовить" },
            new Card { Id = 38, OssetianWord = "Æхсын", RussianWord = "Мыть", VerbType = "Переходный", ImageUrl = "https://via.placeholder.com/300x200/03A9F4/fff?text=Мыть" },
            new Card { Id = 39, OssetianWord = "Дарын", RussianWord = "Держать", VerbType = "Переходный", ImageUrl = "https://via.placeholder.com/300x200/795548/fff?text=Держать" },
            new Card { Id = 40, OssetianWord = "Тæрсын", RussianWord = "Бояться", VerbType = "Непереходный", ImageUrl = "https://via.placeholder.com/300x200/455A64/fff?text=Бояться" },
            new Card { Id = 41, OssetianWord = "Худын", RussianWord = "Смеяться", VerbType = "Непереходный", ImageUrl = "https://via.placeholder.com/300x200/FFC107/fff?text=Смеяться" },
            new Card { Id = 42, OssetianWord = "Кæуын", RussianWord = "Плакать", VerbType = "Непереходный", ImageUrl = "https://via.placeholder.com/300x200/2196F3/fff?text=Плакать" },
            new Card { Id = 43, OssetianWord = "Хъыгæ кæнын", RussianWord = "Обижаться", VerbType = "Непереходный", ImageUrl = "https://via.placeholder.com/300x200/9C27B0/fff?text=Обижаться" },
            new Card { Id = 44, OssetianWord = "Курын", RussianWord = "Просить", VerbType = "Переходный", ImageUrl = "https://via.placeholder.com/300x200/009688/fff?text=Просить" },
            new Card { Id = 45, OssetianWord = "Дзæгъæлы кæнын", RussianWord = "Ломать", VerbType = "Переходный", ImageUrl = "https://via.placeholder.com/300x200/F44336/fff?text=Ломать" },
            new Card { Id = 46, OssetianWord = "Аразын", RussianWord = "Строить", VerbType = "Переходный", ImageUrl = "https://via.placeholder.com/300x200/4CAF50/fff?text=Строить" },
            new Card { Id = 47, OssetianWord = "Фæзмын", RussianWord = "Подражать", VerbType = "Переходный", ImageUrl = "https://via.placeholder.com/300x200/673AB7/fff?text=Подражать" },
        };
    }

    private static List<ConjugationQuestion> GetConjugationQuestions()
    {
        return new List<ConjugationQuestion>
        {
            new ConjugationQuestion
            {
                Id = 1,
                RussianSentence = "Я поел",
                OssetianAnswer = "Æз бахордтон",
                Hint = "хæрын → бахордтон (1 л., ед.ч.)"
            },
            new ConjugationQuestion
            {
                Id = 2,
                RussianSentence = "Ты пил воду",
                OssetianAnswer = "Ды дон бануазтай",
                Hint = "нуазын → бануазтай (2 л., ед.ч.)"
            },
            new ConjugationQuestion
            {
                Id = 3,
                RussianSentence = "Он ушёл",
                OssetianAnswer = "Уый ацыди",
                Hint = "цæуын → ацыди (3 л., ед.ч.)"
            },
            new ConjugationQuestion
            {
                Id = 4,
                RussianSentence = "Мы сидели",
                OssetianAnswer = "Мах бадтам",
                Hint = "бадын → бадтам (1 л., мн.ч.)"
            },
            new ConjugationQuestion
            {
                Id = 5,
                RussianSentence = "Они говорили",
                OssetianAnswer = "Уыдон дзырдтой",
                Hint = "дзурын → дзырдтой (3 л., мн.ч.)"
            },
            new ConjugationQuestion
            {
                Id = 6,
                RussianSentence = "Я написал письмо",
                OssetianAnswer = "Æз фыстон чиныг",
                Hint = "фыссын → фыстон (1 л., ед.ч.)"
            },
            new ConjugationQuestion
            {
                Id = 7,
                RussianSentence = "Ты смотрел",
                OssetianAnswer = "Ды кастай",
                Hint = "кæсын → кастай (2 л., ед.ч.)"
            },
            new ConjugationQuestion
            {
                Id = 8,
                RussianSentence = "Она сделала",
                OssetianAnswer = "Уый скодта",
                Hint = "кæнын → скодта (3 л., ед.ч.)"
            },
            new ConjugationQuestion
            {
                Id = 9,
                RussianSentence = "Вы слушали",
                OssetianAnswer = "Сымах хъуыстат",
                Hint = "хъусын → хъуыстат (2 л., мн.ч.)"
            },
            new ConjugationQuestion
            {
                Id = 10,
                RussianSentence = "Я знал",
                OssetianAnswer = "Æз зыдтон",
                Hint = "зонын → зыдтон (1 л., ед.ч.)"
            }
        };
    }
}
