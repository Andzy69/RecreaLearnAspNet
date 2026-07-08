namespace RecreaLearnAspNet.Models;

public class Course
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Instructor { get; set; } = "";
    public string Image { get; set; } = "";
    public string Level { get; set; } = "";
    public int Students { get; set; }
    public decimal Rating { get; set; }
    public string Category { get; set; } = "";
    public string Description { get; set; } = "";
}

public class SkillCategory
{
    public string Name { get; set; } = "";
    public string Icon { get; set; } = "";
    public string ColorClass { get; set; } = "";
    public string Description { get; set; } = "";
}
