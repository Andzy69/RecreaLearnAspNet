using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace RecreaLearnAspNet.Models;

[Table("course_submissions")]
public class SupabaseCourseSubmission : BaseModel
{
    [PrimaryKey("id", false)]
    public long Id { get; set; }

    [Column("instructor_id")]
    public string InstructorId { get; set; } = "";

    [Column("instructor_name")]
    public string InstructorName { get; set; } = "";

    [Column("title")]
    public string Title { get; set; } = "";

    [Column("category")]
    public string Category { get; set; } = "";

    [Column("level")]
    public string Level { get; set; } = "";

    [Column("description")]
    public string Description { get; set; } = "";

    [Column("image_url")]
    public string ImageUrl { get; set; } = "";

    [Column("status")]
    public string Status { get; set; } = "Pending";

    [Column("admin_feedback")]
    public string AdminFeedback { get; set; } = "";

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}