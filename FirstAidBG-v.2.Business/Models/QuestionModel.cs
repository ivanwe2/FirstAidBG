using FirstAidBG_v._2.Data.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirstAidBG_v._2.Business.Models
{
    public class QuestionModel
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime DatePublished { get; set; }

        
        public int AppUserId { get; set; }
        public AppUser User { get; set; }
        //public int UserId { get; set; }
        public List<Answer> Answers { get; set; }
    }
}
