using FirstAidBG_v._2.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace FirstAidBG_v._2.Business.Models
{
    public class AppUserModel
    {
        [Key]
        public int UserId { get; set; }
        public string Provider { get; set; }
        public string NameIdentifier { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        
        public string Roles { get; set; }
        public List<string> RoleList
        {
            get
            {
                return Roles?.Split(',').ToList() ?? new List<string>();
            }
        }

        public List<int> QuestionIds { get; set; }
        public List<int> AnswerIds { get; set; }
    }
}
