using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstAidBG_v._2.Data.Entities
{
    public class AppUser:BaseEntity
    {
        public string Provider { get; set; }
        public string NameIdentifier { get; set; }

        [Required]
        public string Username { get; set; }


        [DataType(DataType.Password, ErrorMessage = "Паролата не е валидна!")]
        public string Password { get; set; }

        

        [DataType(DataType.EmailAddress, ErrorMessage = "Имейлът не е валиден!")]
        public string Email { get; set; }

        public string Roles { get; set; }
        public List<string> RoleList
        {
            get
            {
                return Roles?.Split(',').ToList() ?? new List<string>();
            }
        }

        public ICollection<Question> Questions { get; set; }
        public ICollection<Answer> Answers { get; set; }
    }
}
