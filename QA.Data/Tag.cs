namespace QA.Data
{
    public class Tag
    {
        public int CategoryId { get; set; }
        public int QuestionId { get; set; }

        public Question Question { get; set; }
        public Category Category { get; set; }
    }
}