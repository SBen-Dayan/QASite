using Microsoft.EntityFrameworkCore;

namespace QA.Data
{
    public class QaRepository
    {
        private readonly string _connectionString;

        public QaRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Question> GetQuestions()
        {
            return GetQuestionsInternal();
        }

        public List<Question> GetQuestions(int categoryId)
        {
            return GetQuestionsInternal(categoryId);
        }

        private List<Question> GetQuestionsInternal(int? categoryId = null)
        {
            using var context = new QaDataContext(_connectionString);
            var questions = context.Questions
                 .Include(q => q.Answers)
                 .Include(q => q.User)
                 .Include(q => q.Tags).ThenInclude(t => t.Category);
                
            if (categoryId != null)
            {
                return questions.Where(q => q.Tags.Any(t => t.CategoryId == categoryId)).ToList();
            }

            return questions.ToList();
        }

        public Question GetQuestion(int id)
        {
            using var context = new QaDataContext(_connectionString);
            return context.Questions
                .Include(q => q.User)
                .Include(q => q.Tags).ThenInclude(t => t.Category)
                .Include(q => q.Answers).ThenInclude(a => a.User)
                .FirstOrDefault(q => q.Id == id);
        }

        public void InsertQuestion(Question question, List<string> tagNames)
        {
            using var context = new QaDataContext(_connectionString);
            context.Questions.Add(question);
            context.SaveChanges();

            foreach (var name in tagNames)
            {
                var lowerName = name.ToLower();
                //var categoryId = context.Tags.FirstOrDefault(t => t.Category.Name == lowerName)?.CategoryId ?? AddCategory(lowerName);
                var categoryId = context.Categories.Where(c => c.Name == lowerName).Select(c => c.Id).FirstOrDefault();
                if (categoryId == default)
                {
                    categoryId = AddCategory(lowerName);
                }

                context.Tags.Add(new()
                {
                    CategoryId = categoryId,
                    QuestionId = question.Id
                });

            }
            context.SaveChanges();
        }

        private int AddCategory(string name)
        {
            using var context = new QaDataContext(_connectionString);
            var category = new Category { Name = name };
            context.Categories.Add(category);
            context.SaveChanges();
            return category.Id;
        }

        public void InsertAnswer(Answer answer)
        {
            using var context = new QaDataContext(_connectionString);
            context.Answers.Add(answer);
            context.SaveChanges();
        }

        public Category GetCategory(int id)
        {
            using var context = new QaDataContext(_connectionString);
            return context.Categories.FirstOrDefault(c => c.Id == id);
        }

    }
}
