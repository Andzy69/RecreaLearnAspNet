using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecreaLearnAspNet.Models;
using RecreaLearnAspNet.Services;

namespace RecreaLearnAspNet.Pages;

public class LessonModel : PageModel
{
    private readonly SupabaseCourseService _courseService;

    public LessonModel(SupabaseCourseService courseService)
    {
        _courseService = courseService;
    }

    public SupabaseCourse? Course { get; set; }
    public List<SupabaseLesson> Lessons { get; set; } = new();
    public SupabaseLesson? CurrentLesson { get; set; }
    public List<SupabaseProgress> Progress { get; set; } = new();

    public string CourseTitle { get; set; } = "Learning Materials";
    public string DatabaseMessage { get; set; } = "";

    public int CompletedCount => Progress.Count(x => x.IsCompleted);
    public int TotalXp => Progress.Sum(x => x.XpEarned);
    private long CategoryId => Course?.CategoryId ?? 0;

    public string DisplayLessonTitle => GetStageName(CurrentLesson?.LessonOrder ?? 1);

    public int CompletedLessons => Progress.Count(x => x.IsCompleted);

    public string LearnerRank
    {
        get
        {
            if (TotalXp >= 1500) return "Future Instructor";
            if (TotalXp >= 1000) return "Skill Mentor";
            if (TotalXp >= 500) return "Creative Contributor";
            return "Skill Explorer";
        }
    }

    public string NextRank
    {
        get
        {
            if (TotalXp >= 1500) return "Ready for Instructor Review";
            if (TotalXp >= 1000) return "Future Instructor";
            if (TotalXp >= 500) return "Skill Mentor";
            return "Creative Contributor";
        }
    }

    public int RankProgress
    {
        get
        {
            if (TotalXp >= 1500) return 100;
            if (TotalXp >= 1000) return (int)((TotalXp - 1000) / 500.0 * 100);
            if (TotalXp >= 500) return (int)((TotalXp - 500) / 500.0 * 100);
            return (int)(TotalXp / 500.0 * 100);
        }
    }

    public List<int> CompletedLessonIds =>
        Progress.Where(x => x.IsCompleted)
                .Select(x => x.LessonId)
                .ToList();

    public int ProgressPercent =>
        Lessons.Count == 0 ? 0 : (int)Math.Round((double)CompletedCount / Lessons.Count * 100);

    public bool Module2Unlocked => CompletedLessonIds.Count >= 4;

    public bool CourseCompleted => CompletedLessonIds.Count >= 6;

    public string SkillTrackName
    {
        get
        {
            return CategoryId switch
            {
                1 => "Conversation Builder Track",
                2 => "Visual Storytelling Track",
                3 => "Home Chef Track",
                4 => "Healthy Lifestyle Track",
                5 => "Creative Design Track",
                6 => "Creative Performance Track",
                _ => "Skill Builder Track"
            };
        }
    }

    public string GetStageName(int order)
    {
        return CategoryId switch
        {
            1 => order switch
            {
                1 => "Discover Basic Phrases",
                2 => "Master Pronunciation",
                3 => "Conversation Practice Lab",
                4 => "Language Checkpoint Quiz",
                5 => "Real Conversation Mission",
                6 => "Speaking Showcase",
                _ => "Language Journey"
            },

            2 => order switch
            {
                1 => "Discover Photography",
                2 => "Master Camera Settings",
                3 => "Photography Practice Lab",
                4 => "Photography Checkpoint Quiz",
                5 => "Photography Mission",
                6 => "Portfolio Showcase",
                _ => "Photography Journey"
            },

            3 => order switch
            {
                1 => "Kitchen Foundations",
                2 => "Cooking Techniques",
                3 => "Cooking Practice Lab",
                4 => "Cooking Checkpoint Quiz",
                5 => "Meal Planning Mission",
                6 => "Recipe Showcase",
                _ => "Cooking Journey"
            },

            4 => order switch
            {
                1 => "Fitness Foundations",
                2 => "Movement Techniques",
                3 => "Workout Practice Lab",
                4 => "Fitness Checkpoint Quiz",
                5 => "Workout Mission",
                6 => "Fitness Showcase",
                _ => "Fitness Journey"
            },

            5 => order switch
            {
                1 => "Discover Creative Expression",
                2 => "Master Tools, Colour, and Composition",
                3 => "Creative Practice Lab",
                4 => "Art Concept Checkpoint Quiz",
                5 => "Client Design Mission",
                6 => "Artwork Portfolio Showcase",
                _ => "Creative Journey"
            },

            6 => order switch
            {
                1 => "Music Foundations",
                2 => "Rhythm and Melody",
                3 => "Music Practice Lab",
                4 => "Music Checkpoint Quiz",
                5 => "Performance Mission",
                6 => "Music Showcase",
                _ => "Music Journey"
            },

            _ => order switch
            {
                1 => "Discover",
                2 => "Learn",
                3 => "Practice Lab",
                4 => "Checkpoint",
                5 => "Real-World Mission",
                6 => "Showcase Project",
                _ => "Skill Journey"
            }
        };
    }

    public string GetLessonTheme()
    {
        return CategoryId switch
        {
            1 => "Language Communication",
            2 => "Photography Storytelling",
            3 => "Kitchen Skill Building",
            4 => "Fitness and Movement",
            5 => "Creative Art Practice",
            6 => "Music Performance",
            _ => "Skill Learning"
        };
    }
    public string DisplayLessonDescription => GetLessonDescription(CurrentLesson?.LessonOrder ?? 1);

    public string GetLessonDescription(int order)
    {
        return CategoryId switch
        {
            1 => order switch
            {
                1 => "Learn basic greetings, daily words, pronunciation, and simple communication phrases.",
                2 => "Practise pronunciation, tone, sentence structure, and useful daily expressions.",
                3 => "Apply phrases in real conversation situations.",
                4 => "Test your beginner language understanding.",
                5 => "Complete a simple real-life speaking mission.",
                6 => "Create a short self-introduction or dialogue showcase.",
                _ => "Build beginner language confidence."
            },

            2 => order switch
            {
                1 => "Learn how photography uses light, composition, timing, and visual storytelling.",
                2 => "Understand aperture, shutter speed, ISO, focus, exposure, and image mood.",
                3 => "Practise choosing suitable camera settings for different photo situations.",
                4 => "Test your understanding of photography fundamentals.",
                5 => "Solve real-world photography scenarios and choose the best shot.",
                6 => "Create a mini photo portfolio and explain your creative choices.",
                _ => "Build photography confidence."
            },

            3 => order switch
            {
                1 => "Explore kitchen safety, basic tools, ingredient preparation, and cooking confidence.",
                2 => "Learn boiling, frying, seasoning, cutting, timing, and plating techniques.",
                3 => "Practise matching cooking methods to ingredients and meal situations.",
                4 => "Test your understanding of kitchen safety and cooking techniques.",
                5 => "Plan a realistic meal based on ingredients, time, and presentation.",
                6 => "Create a recipe showcase with ingredients, steps, and plating ideas.",
                _ => "Build practical cooking confidence."
            },

            4 => order switch
            {
                1 => "Learn safe warm-up habits, beginner movement, and fitness foundations.",
                2 => "Understand strength, flexibility, balance, recovery, and proper form.",
                3 => "Practise choosing exercises for different fitness goals.",
                4 => "Test your understanding of fitness safety and movement.",
                5 => "Create a beginner workout plan for a real-life scenario.",
                6 => "Design a simple 7-day fitness routine.",
                _ => "Build healthy fitness habits."
            },

            5 => order switch
            {
                1 => "Explore creativity, basic shapes, colour, and visual expression.",
                2 => "Learn drawing tools, colour planning, composition, and visual balance.",
                3 => "Practise applying tools and techniques in a creative activity.",
                4 => "Test your understanding of art and design concepts.",
                5 => "Solve a client-style creative design mission.",
                6 => "Create a final artwork portfolio and explain your process.",
                _ => "Build creative art confidence."
            },

            6 => order switch
            {
                1 => "Discover rhythm, melody, listening skills, and musical confidence.",
                2 => "Learn notes, chords, timing, rhythm, and practice routines.",
                3 => "Practise recognising rhythm and matching sound patterns.",
                4 => "Test your understanding of music fundamentals.",
                5 => "Prepare a short performance mission.",
                6 => "Create a mini song or performance showcase.",
                _ => "Build music confidence."
            },

            _ => "Build your skill through guided practice."
        };
    }
    public string GetLearningGoal()
    {
        var order = CurrentLesson?.LessonOrder ?? 1;

        return CategoryId switch
        {
            1 => order switch
            {
                1 => "Discover basic words, greetings, pronunciation, and cultural context.",
                2 => "Learn vocabulary patterns, sentence structure, and useful daily expressions.",
                3 => "Practise matching phrases to real conversation situations.",
                4 => "Check your understanding through a language checkpoint quiz.",
                5 => "Complete a real conversation mission using simple sentences.",
                6 => "Create a self-introduction or short dialogue project.",
                _ => "Build beginner communication confidence."
            },

            2 => order switch
            {
                1 => "Understand how photographers use light, timing, and composition to tell a visual story.",
                2 => "Learn how camera settings affect brightness, blur, sharpness, and image mood.",
                3 => "Practise choosing the correct camera setting for different photography situations.",
                4 => "Test your understanding of photography fundamentals through a checkpoint quiz.",
                5 => "Solve realistic photography scenarios like a beginner photographer.",
                6 => "Create a mini photography portfolio that shows your best learning outcome.",
                _ => "Improve your photography skill through guided practice."
            },

            3 => order switch
            {
                1 => "Discover kitchen safety, ingredients, preparation, and cooking confidence.",
                2 => "Learn basic cooking methods such as boiling, frying, seasoning, and plating.",
                3 => "Practise choosing suitable cooking techniques for different ingredients.",
                4 => "Test your kitchen knowledge through a checkpoint quiz.",
                5 => "Plan a real meal based on time, ingredients, nutrition, and presentation.",
                6 => "Create a final recipe showcase with steps, ingredients, and plating ideas.",
                _ => "Build practical cooking confidence."
            },

            4 => order switch
            {
                1 => "Discover safe fitness habits, warm-up routines, and beginner movement principles.",
                2 => "Learn how strength, flexibility, balance, and recovery work together.",
                3 => "Practise matching exercises to fitness goals and safety needs.",
                4 => "Check your understanding of fitness safety and body movement.",
                5 => "Build a realistic workout plan for a beginner scenario.",
                6 => "Create a 7-day fitness routine as your final project.",
                _ => "Build healthy and safe fitness habits."
            },

            5 => order switch
            {
                1 => "Explore how artists use creativity, tools, colour, and visual ideas to express meaning.",
                2 => "Understand drawing tools, colour choices, composition, and visual balance.",
                3 => "Practise applying creative tools and techniques through a guided activity.",
                4 => "Check your understanding of art concepts through a checkpoint quiz.",
                5 => "Solve a client-style design mission using creative decision-making.",
                6 => "Create a final artwork portfolio and explain your creative process.",
                _ => "Build creative confidence through guided practice."
            },

            6 => order switch
            {
                1 => "Discover rhythm, melody, listening skills, and musical confidence.",
                2 => "Learn basic notes, chords, timing, and practice routines.",
                3 => "Practise recognising rhythm and matching sounds to musical concepts.",
                4 => "Test your understanding of music fundamentals.",
                5 => "Solve a performance preparation mission.",
                6 => "Create a mini performance or short song project.",
                _ => "Build musical confidence through practice."
            },

            _ => "Build your skill through guided practice."
        };
    }

    public string GetProTip()
    {
        return CategoryId switch
        {
            1 => "Use the phrase in a real sentence. Language improves faster when used in context.",
            2 => "Do not only memorise camera settings. Always ask: what story should this photo tell?",
            3 => "Taste as you cook. Small adjustments in seasoning can change the whole dish.",
            4 => "Good form is more important than doing more repetitions.",
            5 => "Good artwork is built in stages: idea, sketch, refine, colour, shadow, highlight, and review.",
            6 => "Slow practice builds clean performance. Speed comes after accuracy.",
            _ => "Practise the skill in a real situation to improve faster."
        };
    }

    public string GetMiniMission()
    {
        var order = CurrentLesson?.LessonOrder ?? 1;

        return CategoryId switch
        {
            1 => order switch
            {
                1 => "Use 5 new words or greetings in a short example.",
                2 => "Practise pronunciation by reading 3 short phrases aloud.",
                3 => "Match phrases to daily conversation situations.",
                4 => "Review one language mistake and correct it.",
                5 => "Write a short real-life conversation.",
                6 => "Create a short self-introduction dialogue.",
                _ => "Use 5 new words or phrases in a real-life conversation example."
            },

            2 => order switch
            {
                1 => "Take one photo of an everyday object and explain what makes it interesting.",
                2 => "Compare two photos: one bright and one dark. Identify which setting may have caused the difference.",
                3 => "Choose camera settings for portrait, sports, and night photography situations.",
                4 => "Review one wrong quiz answer and explain the correct concept.",
                5 => "Act as a photographer and choose the best shot for a client scenario.",
                6 => "Prepare 3 photos and describe the story behind each one.",
                _ => "Apply this photography skill in a real situation."
            },

            3 => order switch
            {
                1 => "Prepare a simple ingredient list and explain why preparation matters.",
                2 => "Choose a suitable cooking method for vegetables, eggs, or rice.",
                3 => "Match cooking techniques to different food situations.",
                4 => "Review one cooking mistake and explain how to fix it.",
                5 => "Plan a simple balanced meal for one person.",
                6 => "Write a short recipe with ingredients, steps, and plating idea.",
                _ => "Apply this cooking skill in a small task."
            },

            4 => order switch
            {
                1 => "Create a 5-minute beginner warm-up routine.",
                2 => "Choose exercises for strength, flexibility, and balance.",
                3 => "Match exercises to fitness goals.",
                4 => "Review one fitness safety rule.",
                5 => "Create a beginner workout plan for a realistic scenario.",
                6 => "Design a 7-day beginner fitness routine.",
                _ => "Apply this fitness skill safely."
            },

            5 => order switch
            {
                1 => "Sketch a simple object using basic shapes.",
                2 => "Create a colour and composition plan for a simple artwork.",
                3 => "Choose the correct tool for sketching, colouring, and shading.",
                4 => "Review one art concept you found difficult.",
                5 => "Create a design plan based on a client request.",
                6 => "Produce one simple artwork and explain your creative process.",
                _ => "Apply this creative skill in a practical task."
            },

            6 => order switch
            {
                1 => "Listen to a short song and identify the rhythm or repeated pattern.",
                2 => "Practise one note, chord, or rhythm pattern slowly.",
                3 => "Match a sound or rhythm to its musical purpose.",
                4 => "Review one music concept you found difficult.",
                5 => "Plan a short performance preparation routine.",
                6 => "Create a mini performance or simple song idea.",
                _ => "Apply this music skill in a short practice."
            },

            _ => "Apply this skill in a real-life example."
        };
    }

    public async Task<IActionResult> OnGetAsync(long courseId, int? lessonId)
    {
        var guard = RequireLogin();
        if (guard != null) return guard;

        await LoadLessonAsync(courseId, lessonId);

        if (CurrentLesson != null &&
            CurrentLesson.LessonOrder >= 5 &&
            !Module2Unlocked)
        {
            return RedirectToPage("/Lesson", new
            {
                courseId,
                lessonId = Lessons.First().Id
            });
        }

        return Page();
    }

    public async Task<IActionResult> OnPostCompleteAsync(long courseId, int lessonId)
    {
        var guard = RequireLogin();
        if (guard != null) return guard;

        await LoadLessonAsync(courseId, lessonId);

        if (CurrentLesson != null)
        {
            await _courseService.SaveLessonProgressAsync(
                HttpContext.Session.GetString("UserId")!,
                courseId,
                CurrentLesson
            );
        }

        await LoadLessonAsync(courseId, lessonId);

        var nextLesson = Lessons
            .Where(x => x.LessonOrder > CurrentLesson!.LessonOrder)
            .OrderBy(x => x.LessonOrder)
            .FirstOrDefault();

        if (nextLesson != null)
        {
            if (nextLesson.LessonOrder >= 5 && !Module2Unlocked)
            {
                return RedirectToPage("/Lesson", new { courseId, lessonId });
            }

            return RedirectToPage("/Lesson", new
            {
                courseId,
                lessonId = nextLesson.Id
            });
        }

        return RedirectToPage("/Lesson", new { courseId, lessonId });
    }

    public bool IsCompleted(int lessonId)
    {
        return Progress.Any(x => x.LessonId == lessonId && x.IsCompleted);
    }

    public bool IsLocked(SupabaseLesson lesson)
    {
        return lesson.LessonOrder >= 5 && !Module2Unlocked;
    }

    private IActionResult? RequireLogin()
    {
        if (!string.IsNullOrWhiteSpace(HttpContext.Session.GetString("UserId")))
            return null;

        var returnUrl = $"{HttpContext.Request.Path}{HttpContext.Request.QueryString}";
        return RedirectToPage("/Login", new { returnUrl });
    }

    private async Task LoadLessonAsync(long courseId, int? lessonId)
    {
        try
        {
            var userId = HttpContext.Session.GetString("UserId")!;

            Course = await _courseService.GetCourseByIdAsync(courseId);
            Lessons = await _courseService.GetLessonsByCourseAsync(courseId);

            Progress = await _courseService.GetProgressByCourseAsync(
                userId,
                courseId
            );

            CourseTitle = Course?.Title ?? $"Course #{courseId}";

            if (Lessons.Any())
            {
                CurrentLesson = lessonId == null
                    ? Lessons.First()
                    : Lessons.FirstOrDefault(x => x.Id == lessonId);

                CurrentLesson ??= Lessons.First();
            }
            foreach (var lesson in Lessons)
            {
                lesson.Title = GetStageName(lesson.LessonOrder);
            }

            if (CurrentLesson != null)
            {
                CurrentLesson.Title = GetStageName(CurrentLesson.LessonOrder);
            }
        }
        catch (Exception ex)
        {
            DatabaseMessage = $"Could not load Supabase lesson data: {ex.Message}";
        }
    }
}