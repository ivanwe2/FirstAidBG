using FirstAidBG_v._2.Data.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirstAidBG_v._2.Business.Models
{
    public class AnswerModel
    {
        [Key]
        public int AnswerId { get; set; }
        public string Text { get; set; }
        public DateTime DatePublished { get; set; }

        
        public int AppUserId { get; set; }
   
        public int QuestionId { get; set; }
        
    }
}
