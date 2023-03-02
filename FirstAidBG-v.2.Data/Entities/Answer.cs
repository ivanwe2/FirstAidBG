using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstAidBG_v._2.Data.Entities
{
    public class Answer : BaseEntity
    {
        public string Text { get; set; }
        public DateTime DatePublished { get; set; }


        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }


        public int IdForQuestion { get; set; }
        //public Question Question { get; set; }
    }
}
