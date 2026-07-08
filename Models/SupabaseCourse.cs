using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace RecreaLearnAspNet.Models;

[Table("courses")]
public class SupabaseCourse : BaseModel
{
    [PrimaryKey("id", false)]
    public long Id { get; set; }

    [Column("category_id")]
    public long CategoryId { get; set; }

    [Column("title")]
    public string Title { get; set; } = "";

    [Column("description")]
    public string Description { get; set; } = "";

    [Column("image_url")]
    public string ImageUrl { get; set; } = "";

    [Column("instructor_name")]
    public string InstructorName { get; set; } = "";

    [Column("level")]
    public string Level { get; set; } = "Beginner";

    [Column("rating")]
    public decimal Rating { get; set; } = 0;

    [Column("students_count")]
    public int StudentsCount { get; set; } = 0;

    [Column("lessons_count")]
    public int LessonsCount { get; set; } = 0;

    [Column("instructor_id")]
    public string? InstructorId { get; set; }

    [Column("status")]
    public string Status { get; set; } = "Pending";

    [Column("submitted_at")]
    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
}