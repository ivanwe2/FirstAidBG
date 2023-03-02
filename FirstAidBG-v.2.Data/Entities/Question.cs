using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstAidBG_v._2.Data.Entities
{
    public class Question:BaseEntity
    {
        public string Text { get; set; }
        public DateTime DatePublished { get; set; }


        public int UserId { get; set; }
        public AppUser User { get; set; }
        
        public ICollection<Answer> Answers { get; set; }
    }
}
