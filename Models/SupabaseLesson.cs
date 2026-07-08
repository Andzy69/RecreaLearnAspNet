using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace RecreaLearnAspNet.Models
{
    [Table("lessons")]
    public class SupabaseLesson : BaseModel
    {
        [PrimaryKey("id", false)]
        public int Id { get; set; }

        [Column("course_id")]
        public int CourseId { get; set; }

        [Column("title")]
        public string Title { get; set; } = "";

        [Column("description")]
        public string Description { get; set; } = "";

        [Column("lesson_order")]
        public int LessonOrder { get; set; }

        [Column("lesson_type")]
        public string LessonType { get; set; } = "";

        [Column("xp_reward")]
        public int XpReward { get; set; }

        [Column("mission")]
        public string Mission { get; set; } = "";

        [Column("quick_learn")]
        public string QuickLearn { get; set; } = "";

        [Column("practice_task")]
        public string PracticeTask { get; set; } = "";

        [Column("learning_content")]
        public string LearningContent { get; set; } = "";

        [Column("example_content")]
        public string ExampleContent { get; set; } = "";

        [Column("practice_instruction")]
        public string PracticeInstruction { get; set; } = "";

        [Column("reflection_prompt")]
        public string ReflectionPrompt { get; set; } = "";

        [Column("video_url")]
        public string VideoUrl { get; set; } = "";

        [Column("article_title_1")]
        public string ArticleTitle1 { get; set; } = "";

        [Column("article_body_1")]
        public string ArticleBody1 { get; set; } = "";

        [Column("article_title_2")]
        public string ArticleTitle2 { get; set; } = "";

        [Column("article_body_2")]
        public string ArticleBody2 { get; set; } = "";

        [Column("article_title_3")]
        public string ArticleTitle3 { get; set; } = "";

        [Column("article_body_3")]
        public string ArticleBody3 { get; set; } = "";

        [Column("section_title_1")]
        public string SectionTitle1 { get; set; } = "";

        [Column("section_body_1")]
        public string SectionBody1 { get; set; } = "";

        [Column("section_image_1")]
        public string SectionImage1 { get; set; } = "";

        [Column("section_title_2")]
        public string SectionTitle2 { get; set; } = "";

        [Column("section_body_2")]
        public string SectionBody2 { get; set; } = "";

        [Column("section_image_2")]
        public string SectionImage2 { get; set; } = "";

        [Column("section_title_3")]
        public string SectionTitle3 { get; set; } = "";

        [Column("section_body_3")]
        public string SectionBody3 { get; set; } = "";

        [Column("section_image_3")]
        public string SectionImage3 { get; set; } = "";

        [Column("section_title_4")]
        public string SectionTitle4 { get; set; } = "";

        [Column("section_body_4")]
        public string SectionBody4 { get; set; } = "";

        [Column("section_image_4")]
        public string SectionImage4 { get; set; } = "";

        [Column("activity_prompt")]
        public string ActivityPrompt { get; set; } = "";

        [Column("activity_item_1")]
        public string ActivityItem1 { get; set; } = "";

        [Column("activity_item_2")]
        public string ActivityItem2 { get; set; } = "";

        [Column("activity_item_3")]
        public string ActivityItem3 { get; set; } = "";

        [Column("activity_answer_1")]
        public string ActivityAnswer1 { get; set; } = "";

        [Column("activity_answer_2")]
        public string ActivityAnswer2 { get; set; } = "";

        [Column("activity_answer_3")]
        public string ActivityAnswer3 { get; set; } = "";

        [Column("quiz_question")]
        public string QuizQuestion { get; set; } = "";

        [Column("quiz_option_a")]
        public string QuizOptionA { get; set; } = "";

        [Column("quiz_option_b")]
        public string QuizOptionB { get; set; } = "";

        [Column("quiz_option_c")]
        public string QuizOptionC { get; set; } = "";

        [Column("correct_option")]
        public string CorrectOption { get; set; } = "";

        [Column("quiz_question_2")]
        public string QuizQuestion2 { get; set; } = "";

        [Column("quiz_option_2a")]
        public string QuizOption2A { get; set; } = "";

        [Column("quiz_option_2b")]
        public string QuizOption2B { get; set; } = "";

        [Column("quiz_option_2c")]
        public string QuizOption2C { get; set; } = "";

        [Column("correct_option_2")]
        public string CorrectOption2 { get; set; } = "";

        [Column("quiz_question_3")]
        public string QuizQuestion3 { get; set; } = "";

        [Column("quiz_option_3a")]
        public string QuizOption3A { get; set; } = "";

        [Column("quiz_option_3b")]
        public string QuizOption3B { get; set; } = "";

        [Column("quiz_option_3c")]
        public string QuizOption3C { get; set; } = "";

        [Column("correct_option_3")]
        public string CorrectOption3 { get; set; } = "";

        [Column("quiz_answer")]
        public string QuizAnswer { get; set; } = "";
    }
}