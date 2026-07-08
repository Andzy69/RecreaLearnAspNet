using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace RecreaLearnAspNet.Models;

[Table("categories")]
public class SupabaseCategory : BaseModel
{
    [PrimaryKey("id", false)]
    public long Id { get; set; }

    [Column("name")]
    public string Name { get; set; } = "";

    [Column("slug")]
    public string Slug { get; set; } = "";

    [Column("description")]
    public string Description { get; set; } = "";

    [Column("icon_name")]
    public string IconName { get; set; } = "";

    [Column("color_class")]
    public string ColorClass { get; set; } = "";
}
