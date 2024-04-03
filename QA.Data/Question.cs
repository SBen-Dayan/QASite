using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QA.Data
{
    public class Question
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DatePosted { get; set; }
        //public int Likes { get; set; }
        public int UserId { get; set; }

        public List<Tag> Tags { get; set; } = new();
        public List<Answer> Answers { get; set; }
        public User User { get; set; } = new();
    }
}
