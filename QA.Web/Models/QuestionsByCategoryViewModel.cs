using QA.Data;

namespace QA.Web.Models
{
    public class QuestionsByCategoryViewModel
    {
        public Category Category { get; set; }
        public List<Question> Questions { get; set; }
    }
}
