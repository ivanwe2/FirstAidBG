using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstAidBG_v._2.Data.Seeder
{
    public class Seed 
    {
        
        public static async Task SeedUsersAsync(ApplicationDbContext context)
        {
            if (await context.AppUsers.AnyAsync()) return;

            var users = new List<Entities.AppUser>
            {
                new Entities.AppUser
                {
                    Id = 0,
                    Provider = "Cookies",
                    NameIdentifier = "Admin",
                    Username = "Admin",
                    Password = "firstaidbgadmin",
                    Email = "rusev37@gmail.com",
                    Roles = "Admin"
                                     
                },
                new Entities.AppUser
                {
                    Id = 1,
                    Provider = "Cookies",
                    NameIdentifier = "TestUser",
                    Username = "TestUser",
                    Password = "firstaidbgtestuser",
                    Email = "rusev37@gmail.com",
                    Roles = "Everyone"
                }               
            };

            await context.AppUsers.AddRangeAsync(users);
            await context.SaveChangesAsync();
        }

        public static async Task SeedQuestionsAsync(ApplicationDbContext context)
        {
            if (await context.Questions.AnyAsync()) return;

            var questions = new List<Entities.Question>
            {
                new Entities.Question
                {
                    Id = 0,
                    Text = "Тестов въпрос номер 1?",
                    DatePublished = DateTime.Now,
                    UserId = 0,

                },
                new Entities.Question
                {
                    Id = 1,
                    Text = "Тестов въпрос номер 2?",
                    DatePublished = DateTime.Now,
                    UserId = 1,
                }
            };

            await context.Questions.AddRangeAsync(questions);
            await context.SaveChangesAsync();
        }
        public static async Task SeedAnswersAsync(ApplicationDbContext context)
        {
            if (await context.Answers.AnyAsync()) return;

            var answers = new List<Entities.Answer>
            {
                new Entities.Answer
                {
                    Id = 0,
                    Text = "Тестов отговор номер 1?",
                    DatePublished = DateTime.Now,
                    AppUserId = 0,
                    IdForQuestion = 0,

                },
                new Entities.Answer
                {
                    Id = 1,
                    Text = "Тестов отговор номер 2?",
                    DatePublished = DateTime.Now,
                    AppUserId = 1,
                    IdForQuestion = 1,
                }
            };

            await context.Answers.AddRangeAsync(answers);
            await context.SaveChangesAsync();
        }

          
       
    }
}
