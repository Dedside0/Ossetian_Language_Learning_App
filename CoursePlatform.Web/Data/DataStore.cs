using Adam.Models;

namespace Adam.Data;

public static class DataStore
{
    private static List<Course> _courses = new();
    private static int _nextCourseId = 100;
    private static int _nextLessonId = 100;
    private static int _nextCardId = 1000;
    private static int _nextQuestionId = 5000;

    private static int _nextListeningTaskId = 5000;

    static DataStore()
    {
        InitializeDefaultCourses();
    }

    public static List<Course> GetCourses() => _courses;

    public static Course? GetCourse(int id) => _courses.FirstOrDefault(c => c.Id == id);

    public static Lesson? GetLesson(int courseId, int lessonId)
    {
        var course = GetCourse(courseId);
        return course?.Lessons.FirstOrDefault(l => l.Id == lessonId);
    }

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
                    TheoryHtml = "<p>Добавьте задания в этот урок с помощью конструктора.</p>",
                    Cards = new List<Card>(),
                    ConjugationQuestions = new List<ConjugationQuestion>(),
                    // Инициализируем пустой список для аудирования в новом курсе
                    ListeningTasks = new List<ListeningTask>()
                }
            }
        };
        _courses.Add(course);
        return course;
    }
    public static void UpdateLessonTheory(int courseId, int lessonId, string theoryHtml)
    {
        var lesson = GetLesson(courseId, lessonId);
        if (lesson != null)
        {
            lesson.TheoryHtml = theoryHtml ?? string.Empty;
        }
    }
    public static Lesson? GetLesson(int courseId, int lessonId)
    {
        var course = GetCourse(courseId);
        return course?.Lessons.FirstOrDefault(l => l.Id == lessonId);
    }

    public static Card AddCard(int courseId, int lessonId, string ossetianWord, string russianWord, string audioUrl, string imageUrl)
    {
        var lesson = GetLesson(courseId, lessonId);
        if (lesson == null) throw new Exception("Lesson not found");

        var card = new Card
        {
            Id = _nextCardId++,
            OssetianWord = ossetianWord,
            RussianWord = russianWord,
            ImageUrl = imageUrl,
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
                        ConjugationQuestions = GetConjugationQuestions(),
                        // Задания на аудирование для Глаголов
                        ListeningTasks = new List<ListeningTask>
                        {
                            new ListeningTask { Id = 5001, AudioUrl = "audio/verbs/ba_khordton.mp3", AudioDecoding = "Æз бахордтон", RussianTranslation = "Я поел" },
                            new ListeningTask { Id = 5002, AudioUrl = "audio/verbs/ba_nuaztai.mp3", AudioDecoding = "Ды дон бануазтай", RussianTranslation = "Ты пил воду" },
                            new ListeningTask { Id = 5003, AudioUrl = "audio/verbs/atsydi.mp3", AudioDecoding = "Уый ацыди", RussianTranslation = "Он ушёл" },
                            new ListeningTask { Id = 5004, AudioUrl = "audio/verbs/dzyrdtoi.mp3", AudioDecoding = "Уыдон дзырдтой", RussianTranslation = "Они говорили" }
                        }
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
                            new Card { Id = 100, OssetianWord = "Салам", RussianWord = "Привет", ImageUrl = "\\CardImages\\hi.avif" },
                            new Card { Id = 101, OssetianWord = "Дæ бон хорз", RussianWord = "Добрый день", ImageUrl = "\\CardImages\\goodafternoon.avif" },
                            new Card { Id = 102, OssetianWord = "Дæ изæр хорз", RussianWord = "Добрый вечер", ImageUrl = "\\CardImages\\goodevening.avif" },
                            new Card { Id = 103, OssetianWord = "Хорз бон", RussianWord = "Хорошего дня", ImageUrl = "\\CardImages\\bye.jpg" },
                            new Card { Id = 104, OssetianWord = "Фæндараст", RussianWord = "Счастливого пути", ImageUrl = "\\CardImages\\happyroad.png" },
                        },
                        ConjugationQuestions = new List<ConjugationQuestion>(),
                        // Задания на аудирование для Приветствий
                        ListeningTasks = new List<ListeningTask>
                        {
                            new ListeningTask { Id = 5101, AudioUrl = "audio/greetings/salam.mp3", AudioDecoding = "Салам", RussianTranslation = "Привет" },
                            new ListeningTask { Id = 5102, AudioUrl = "audio/greetings/de_bon_horz.mp3", AudioDecoding = "Де бон хорз", RussianTranslation = "Добрый день" },
                            new ListeningTask { Id = 5103, AudioUrl = "audio/greetings/da_izar_horz.mp3", AudioDecoding = "Дæ изæр хорз", RussianTranslation = "Добрый вечер" },
                            new ListeningTask { Id = 5104, AudioUrl = "audio/greetings/fandarast.mp3", AudioDecoding = "Фæндараст", RussianTranslation = "Счастливого пути" }
                        }
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
                        Title = "Урок 1. Цифры от 1 до 9",
                        TheoryHtml = "<h4>Цифры в осетинском языке</h4>" +
                            "<p>Система счёта в осетинском языке основана на двадцатеричной системе, что является отличительной чертой иранских языков Кавказа.</p>" +
                            "<ul><li><strong>Иу</strong> — 1</li><li><strong>Дыууæ</strong> — 2</li><li><strong>Æртæ</strong> — 3</li></ul>",
                        Cards = new List<Card>
                        {
                            new Card { Id = 200, OssetianWord = "Иу", RussianWord = "Один", ImageUrl = "\\CardImages\\1.jpg" },
                            new Card { Id = 201, OssetianWord = "Дыууæ", RussianWord = "Два", ImageUrl = "\\CardImages\\2.jpg" },
                            new Card { Id = 202, OssetianWord = "Æртæ", RussianWord = "Три", ImageUrl = "\\CardImages\\3.jpg" },
                            new Card { Id = 203, OssetianWord = "Цыппар", RussianWord = "Четыре", ImageUrl = "\\CardImages\\4.jpg" },
                            new Card { Id = 204, OssetianWord = "Фондз", RussianWord = "Пять", ImageUrl = "\\CardImages\\5.jpg" },
                            new Card { Id = 205, OssetianWord = "Æхсæз", RussianWord = "Шесть", ImageUrl = "\\CardImages\\6.jpg" },
                            new Card { Id = 206, OssetianWord = "Авд", RussianWord = "Семь", ImageUrl = "\\CardImages\\7.jpg" },
                            new Card { Id = 207, OssetianWord = "Аст", RussianWord = "Восемь", ImageUrl = "\\CardImages\\8.jpg" },
                            new Card { Id = 208, OssetianWord = "Фараст", RussianWord = "Девять", ImageUrl = "\\CardImages\\9.jpg" },
                        },
                        ConjugationQuestions = new List<ConjugationQuestion>(),
                        // Задания на аудирование для Чисел
                        ListeningTasks = new List<ListeningTask>
                        {
                            new ListeningTask { Id = 5201, AudioUrl = "audio/numbers/iu.mp3", AudioDecoding = "Иу", RussianTranslation = "Один" },
                            new ListeningTask { Id = 5202, AudioUrl = "audio/numbers/dyuuae.mp3", AudioDecoding = "Дыууæ", RussianTranslation = "Два" },
                            new ListeningTask { Id = 5203, AudioUrl = "audio/numbers/aertae.mp3", AudioDecoding = "Æртæ", RussianTranslation = "Три" },
                            new ListeningTask { Id = 5204, AudioUrl = "audio/numbers/daes.mp3", AudioDecoding = "Дæс", RussianTranslation = "Десять" }
                        }
                    }
                }
            }
        };
    }

    // Генерация дефолтных заданий для глаголов
    private static List<ListeningTask> GetDefaultListeningTasksForVerbs()
    {
        return new List<ListeningTask>
        {
            new ListeningTask
            {
                Id = 500,
                AudioUrl = "audio/axwaryn.mp3",
                AudioDecoding = "Æз бахордтон",
                RussianTranslation = "Я поел"
            },
            new ListeningTask
            {
                Id = 501,
                AudioUrl = "audio/dzuuryn.mp3",
                AudioDecoding = "Уыдон дзырдтой",
                RussianTranslation = "Они говорили"
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
            new Card { Id = 1, OssetianWord = "Хæрын", RussianWord = "Есть (кушать)", VerbType = "Переходный", ImageUrl = "\\CardImages\\eat.avif" },
            new Card { Id = 2, OssetianWord = "Нуазын", RussianWord = "Пить", VerbType = "Переходный", ImageUrl = "\\CardImages\\drink.avif" },
            new Card { Id = 3, OssetianWord = "Цæуын", RussianWord = "Идти", VerbType = "Непереходный", ImageUrl = "\\CardImages\\walking.avif" },
            new Card { Id = 4, OssetianWord = "Бадын", RussianWord = "Сидеть", VerbType = "Непереходный", ImageUrl = "\\CardImages\\sit.avif" },
            new Card { Id = 5, OssetianWord = "Лæууын", RussianWord = "Стоять", VerbType = "Непереходный", ImageUrl = "\\CardImages\\stand.png" },
            new Card { Id = 6, OssetianWord = "Хуыссын", RussianWord = "Спать", VerbType = "Непереходный", ImageUrl = "\\CardImages\\sleep.avif" },
            new Card { Id = 7, OssetianWord = "Дзурын", RussianWord = "Говорить", VerbType = "Непереходный", ImageUrl = "\\CardImages\\talking.avif" },
            new Card { Id = 8, OssetianWord = "Хъусын", RussianWord = "Слушать", VerbType = "Переходный", ImageUrl = "\\CardImages\\listen.png" },
            new Card { Id = 9, OssetianWord = "Кæсын", RussianWord = "Смотреть", VerbType = "Непереходный", ImageUrl = "\\CardImages\\watch.avif" },
            new Card { Id = 10, OssetianWord = "Фыссын", RussianWord = "Писать", VerbType = "Переходный", ImageUrl = "\\CardImages\\write.avif" },
            new Card { Id = 11, OssetianWord = "Кæнын", RussianWord = "Делать", VerbType = "Переходный", ImageUrl = "\\CardImages\\do.avif" },
            new Card { Id = 12, OssetianWord = "Зонын", RussianWord = "Знать", VerbType = "Переходный", ImageUrl = "\\CardImages\\know.avif" },
            new Card { Id = 13, OssetianWord = "Уарзын", RussianWord = "Любить", VerbType = "Переходный", ImageUrl = "\\CardImages\\love.avif" },
            new Card { Id = 14, OssetianWord = "Амондзын", RussianWord = "Учить (кого-то)", VerbType = "Переходный", ImageUrl = "\\CardImages\\teach.png" },
            new Card { Id = 15, OssetianWord = "Ахуыр кæнын", RussianWord = "Учиться", VerbType = "Непереходный", ImageUrl = "\\CardImages\\study.avif" },
            new Card { Id = 16, OssetianWord = "Æрбацæуын", RussianWord = "Приходить", VerbType = "Непереходный", ImageUrl = "\\CardImages\\come.jpg" },
            new Card { Id = 17, OssetianWord = "Ацæуын", RussianWord = "Уходить", VerbType = "Непереходный", ImageUrl = "\\CardImages\\leave.jpg" },
            new Card { Id = 18, OssetianWord = "Лæсын", RussianWord = "Ползти", VerbType = "Непереходный", ImageUrl = "\\CardImages\\crouch.avif" },
            new Card { Id = 19, OssetianWord = "Тæхын", RussianWord = "Летать", VerbType = "Непереходный", ImageUrl = "\\CardImages\\fly.png" },
            new Card { Id = 20, OssetianWord = "Ленк кæнын", RussianWord = "Плавать", VerbType = "Непереходный", ImageUrl = "\\CardImages\\swim.avif" },
            new Card { Id = 21, OssetianWord = "Уайын", RussianWord = "Бежать", VerbType = "Непереходный", ImageUrl = "\\CardImages\\run.avif" },
            new Card { Id = 22, OssetianWord = "Скъæфын", RussianWord = "Хватать", VerbType = "Переходный", ImageUrl = "\\CardImages\\grab.avif" },
            new Card { Id = 23, OssetianWord = "Æвæрын", RussianWord = "Класть", VerbType = "Переходный", ImageUrl = "\\CardImages\\put.avif" },
            new Card { Id = 24, OssetianWord = "Исын", RussianWord = "Брать", VerbType = "Переходный", ImageUrl = "\\CardImages\\take.avif" },
            new Card { Id = 25, OssetianWord = "Дæттын", RussianWord = "Давать", VerbType = "Переходный", ImageUrl = "\\CardImages\\give.avif" },
            new Card { Id = 26, OssetianWord = "Фидын", RussianWord = "Платить", VerbType = "Переходный", ImageUrl = "\\CardImages\\pay.avif" },
            new Card { Id = 27, OssetianWord = "Æлхæнын", RussianWord = "Покупать", VerbType = "Переходный", ImageUrl = "\\CardImages\\buy.avif" },
            new Card { Id = 28, OssetianWord = "Уæй кæнын", RussianWord = "Продавать", VerbType = "Переходный", ImageUrl = "\\CardImages\\sell.avif" },
            new Card { Id = 29, OssetianWord = "Сафын", RussianWord = "Терять", VerbType = "Переходный", ImageUrl = "\\CardImages\\loose.png" },
            new Card { Id = 30, OssetianWord = "Ссарын", RussianWord = "Находить", VerbType = "Переходный", ImageUrl = "\\CardImages\\find.jpg" },
            new Card { Id = 31, OssetianWord = "Уадзын", RussianWord = "Оставлять", VerbType = "Переходный", ImageUrl = "\\CardImages\\giveup.avif" },
            new Card { Id = 32, OssetianWord = "Æмбарын", RussianWord = "Понимать", VerbType = "Переходный", ImageUrl = "\\CardImages\\understand.png" },
            new Card { Id = 33, OssetianWord = "Ферох кæнын", RussianWord = "Забывать", VerbType = "Переходный", ImageUrl = "\\CardImages\\forgot.avif" },
            new Card { Id = 34, OssetianWord = "Хъæбæр кæнын", RussianWord = "Кричать", VerbType = "Непереходный", ImageUrl = "\\CardImages\\scream.jpg" },
            new Card { Id = 35, OssetianWord = "Зарын", RussianWord = "Петь", VerbType = "Непереходный", ImageUrl = "\\CardImages\\sing.avif" },
            new Card { Id = 36, OssetianWord = "Кафын", RussianWord = "Танцевать", VerbType = "Непереходный", ImageUrl = "\\CardImages\\dance.avif" },
            new Card { Id = 37, OssetianWord = "Хæдзаронд кæнын", RussianWord = "Готовить (еду)", VerbType = "Переходный", ImageUrl = "\\CardImages\\cook.avif" },
            new Card { Id = 38, OssetianWord = "Æхсын", RussianWord = "Мыть", VerbType = "Переходный", ImageUrl = "\\CardImages\\wash.avif" },
            new Card { Id = 39, OssetianWord = "Дарын", RussianWord = "Держать", VerbType = "Переходный", ImageUrl = "\\CardImages\\keep.png" },
            new Card { Id = 40, OssetianWord = "Тæрсын", RussianWord = "Бояться", VerbType = "Непереходный", ImageUrl = "\\CardImages\\fear.jpg" },
            new Card { Id = 41, OssetianWord = "Худын", RussianWord = "Смеяться", VerbType = "Непереходный", ImageUrl = "\\CardImages\\laugh.avif" },
            new Card { Id = 42, OssetianWord = "Кæуын", RussianWord = "Плакать", VerbType = "Непереходный", ImageUrl = "\\CardImages\\cry.avif" },
            new Card { Id = 43, OssetianWord = "Хъыгæ кæнын", RussianWord = "Обижаться", VerbType = "Непереходный", ImageUrl = "\\CardImages\\angry.avif" },
            new Card { Id = 44, OssetianWord = "Курын", RussianWord = "Просить", VerbType = "Переходный", ImageUrl = "\\CardImages\\please.avif" },
            new Card { Id = 45, OssetianWord = "Дзæгъæлы кæнын", RussianWord = "Ломать", VerbType = "Переходный", ImageUrl = "\\CardImages\\break.avif" },
            new Card { Id = 46, OssetianWord = "Аразын", RussianWord = "Строить", VerbType = "Переходный", ImageUrl = "\\CardImages\\build.avif" },
            new Card { Id = 47, OssetianWord = "Фæзмын", RussianWord = "Подражать", VerbType = "Переходный", ImageUrl = "\\CardImages\\simulate.avif" }
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
