using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecreaLearnAspNet.Models;
using RecreaLearnAspNet.Services;

namespace RecreaLearnAspNet.Pages;

public class CourseDetailModel : PageModel
{
    private readonly SupabaseCourseService _courseService;

    public CourseDetailModel(SupabaseCourseService courseService)
    {
        _courseService = courseService;
    }

    public SupabaseCourse? Course { get; set; }

    public List<SupabaseLesson> Lessons { get; set; } = new();

    public string DatabaseMessage { get; set; } = "";

    public long CategoryId => Course?.CategoryId ?? 0;

    public string CategoryName =>
        Course == null ? "Course" : _courseService.GetCategoryName(Course.CategoryId);

    public bool IsLoggedIn =>
        !string.IsNullOrWhiteSpace(HttpContext.Session.GetString("UserId"));

    public string GetJourneyTitle()
    {
        return CategoryId switch
        {
            1 => "Conversation Builder Journey",
            2 => "Visual Storytelling Journey",
            3 => "Home Chef Skill Journey",
            4 => "Healthy Lifestyle Journey",
            5 => "Creative Design Journey",
            6 => "Creative Performance Journey",
            _ => "Skill Builder Journey"
        };
    }

    public List<string> GetJourneyPoints()
    {
        return CategoryId switch
        {
            1 => new List<string>
            {
                "🗣 Quest 1: Discover greetings, pronunciation, basic phrases, and culture.",
                "📚 Quest 2: Learn vocabulary, grammar patterns, and daily expressions.",
                "🎮 Quest 3: Practise matching phrases to real conversation situations.",
                "🧩 Quest 4: Complete a language checkpoint quiz.",
                "🌍 Quest 5: Complete a real conversation mission such as ordering food or introducing yourself.",
                "🏆 Quest 6: Create a self-introduction or short dialogue showcase."
            },

            2 => new List<string>
            {
                "📖 Quest 1: Discover how photography works and how cameras capture light.",
                "📷 Quest 2: Learn exposure, aperture, shutter speed, ISO, and focus control.",
                "🎮 Quest 3: Practise choosing the correct setting for portraits, action, and low-light scenes.",
                "🧩 Quest 4: Complete a photography checkpoint quiz with feedback.",
                "🏹 Quest 5: Solve real-world photography missions like a beginner photographer.",
                "🌟 Quest 6: Build a mini photo portfolio and explain your creative decisions."
            },

            3 => new List<string>
            {
                "🍳 Quest 1: Discover kitchen safety, ingredients, tools, and preparation habits.",
                "🔪 Quest 2: Learn cutting, seasoning, boiling, frying, timing, and plating techniques.",
                "🎮 Quest 3: Practise matching ingredients with the correct cooking method.",
                "🧩 Quest 4: Complete a kitchen knowledge checkpoint quiz.",
                "🥘 Quest 5: Plan a real meal based on ingredients, budget, time, and nutrition.",
                "🏆 Quest 6: Create a final recipe showcase with steps, presentation, and reflection."
            },

            4 => new List<string>
            {
                "💪 Quest 1: Discover safe warm-ups, beginner movement, and fitness goals.",
                "🧘 Quest 2: Learn strength, flexibility, endurance, balance, and recovery basics.",
                "🎮 Quest 3: Practise matching exercises to real fitness goals.",
                "🧩 Quest 4: Complete a fitness safety checkpoint quiz.",
                "🏃 Quest 5: Design a workout plan for a realistic beginner scenario.",
                "🏆 Quest 6: Create a 7-day fitness plan with goals, rest days, and reflection."
            },

            5 => new List<string>
            {
                "🎨 Quest 1: Discover creative expression, visual ideas, and basic art concepts.",
                "🖌 Quest 2: Learn tools, colour, composition, layering, and visual balance.",
                "🎮 Quest 3: Practise applying tools and techniques in a creative practice lab.",
                "🧩 Quest 4: Complete an art concept checkpoint quiz.",
                "🧑‍🎨 Quest 5: Solve a client-style design mission.",
                "🏆 Quest 6: Create a final artwork portfolio and explain your creative process."
            },

            6 => new List<string>
            {
                "🎵 Quest 1: Discover rhythm, melody, listening skills, and musical expression.",
                "🎸 Quest 2: Learn notes, chords, timing, tempo, and beginner practice habits.",
                "🎮 Quest 3: Practise recognising rhythm patterns and matching musical sounds.",
                "🧩 Quest 4: Complete a music theory checkpoint quiz.",
                "🎤 Quest 5: Prepare for a simple performance mission.",
                "🏆 Quest 6: Create a mini song, rhythm pattern, or short performance showcase."
            },

            _ => new List<string>
            {
                "📖 Quest 1: Discover the skill foundation.",
                "📚 Quest 2: Learn important concepts and techniques.",
                "🎮 Quest 3: Practise through guided activities.",
                "🧩 Quest 4: Complete a checkpoint quiz.",
                "🌍 Quest 5: Solve a real-world mission.",
                "🏆 Quest 6: Create a final showcase project."
            }
        };
    }

    public string GetStageName(int lessonOrder)
    {
        return CategoryId switch
        {
            1 => lessonOrder switch
            {
                1 => "Discover Chinese Foundations",
                2 => "Master Core Vocabulary",
                3 => "Conversation Practice Lab",
                4 => "Language Checkpoint Quiz",
                5 => "Real-World Speaking Mission",
                6 => "Final Dialogue Showcase",
                _ => $"Quest {lessonOrder}"
            },

            2 => lessonOrder switch
            {
                1 => "Discover Photography",
                2 => "Master Camera Settings",
                3 => "Photography Practice Lab",
                4 => "Photography Checkpoint Quiz",
                5 => "Real-World Photography Mission",
                6 => "Photography Portfolio Showcase",
                _ => $"Quest {lessonOrder}"
            },

            3 => lessonOrder switch
            {
                1 => "Discover Kitchen Confidence",
                2 => "Master Ingredients and Cooking Methods",
                3 => "Cooking Technique Practice Lab",
                4 => "Kitchen Knowledge Checkpoint Quiz",
                5 => "Real-World Meal Planning Mission",
                6 => "Final Recipe Showcase",
                _ => $"Quest {lessonOrder}"
            },

            4 => lessonOrder switch
            {
                1 => "Discover Safe Fitness Foundations",
                2 => "Master Movement, Form, and Recovery",
                3 => "Workout Practice Lab",
                4 => "Fitness Safety Checkpoint Quiz",
                5 => "Personal Training Mission",
                6 => "Fitness Plan Showcase",
                _ => $"Quest {lessonOrder}"
            },

            5 => lessonOrder switch
            {
                1 => "Discover Creative Expression",
                2 => "Master Tools, Colour, and Composition",
                3 => "Creative Practice Lab",
                4 => "Art Concept Checkpoint Quiz",
                5 => "Client Design Mission",
                6 => "Artwork Portfolio Showcase",
                _ => $"Quest {lessonOrder}"
            },

            6 => lessonOrder switch
            {
                1 => "Discover Music Fundamentals",
                2 => "Master Rhythm and Melody",
                3 => "Music Practice Lab",
                4 => "Music Checkpoint Quiz",
                5 => "Performance Mission",
                6 => "Music Showcase Project",
                _ => $"Quest {lessonOrder}"
            },

            _ => $"Quest {lessonOrder}"
        };
    }

    public async Task<IActionResult> OnGetAsync(long id)
    {
        try
        {
            Course = await _courseService.GetCourseByIdAsync(id);

            if (Course == null)
            {
                return NotFound();
            }

            Lessons = await _courseService.GetLessonsByCourseAsync(id);
        }
        catch (Exception ex)
        {
            DatabaseMessage = $"Could not load this Supabase course: {ex.Message}";
        }

        return Page();
    }
}