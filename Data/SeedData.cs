using RecreaLearnAspNet.Models;

namespace RecreaLearnAspNet.Data;

public static class SeedData
{
    public static List<SkillCategory> Categories => new()
{
    new SkillCategory
    {
        Name = "Arts",
        Icon = "fas fa-palette",
        ColorClass = "cat-arts",
        Description = "Explore painting, drawing, sculpture, and creative expression."
    },

    new SkillCategory
    {
        Name = "Cooking",
        Icon = "fas fa-utensils",
        ColorClass = "cat-cooking",
        Description = "Master culinary arts from basic techniques to gourmet cuisine."
    },

    new SkillCategory
    {
        Name = "Fitness",
        Icon = "fas fa-dumbbell",
        ColorClass = "cat-fitness",
        Description = "Build strength, flexibility, and healthy habits."
    },

    new SkillCategory
    {
        Name = "Photography",
        Icon = "fas fa-camera",
        ColorClass = "cat-photo",
        Description = "Capture stunning images and develop your visual storytelling."
    },

    new SkillCategory
    {
        Name = "Music",
        Icon = "fas fa-music",
        ColorClass = "cat-music",
        Description = "Learn instruments, music theory, and composition."
    },

    new SkillCategory
    {
        Name = "Language Learning",
        Icon = "fas fa-globe",
        ColorClass = "cat-language",
        Description = "Learn new languages and expand your communication skills."
    },
};

    public static List<Course> Courses => new()
    {
        new Course { Id = 1, Title = "Mandarin Chinese for Beginners", Image = "https://images.unsplash.com/photo-1513258496099-48168024aec0?w=900", Level = "Beginner", Students = 1245, Rating = 4.8m, Category = "Language", Description = "Start speaking Mandarin through daily phrases, tone practice, and playful listening missions." },
        new Course { Id = 2, Title = "Photography Essentials", Image = "https://images.unsplash.com/photo-1516035069371-29a1b244cc32?w=900", Level = "Beginner", Students = 892, Rating = 4.9m, Category = "Photography", Description = "Learn camera settings, composition, lighting, and storytelling for beautiful images." },
        new Course { Id = 3, Title = "Korean Language Essentials",  Image = "https://images.unsplash.com/photo-1517154421773-0529f29ea451?w=900", Level = "Beginner", Students = 945, Rating = 4.9m, Category = "Language", Description = "Learn Hangul, pronunciation, basic grammar, and everyday Korean conversation." },
    };
}
