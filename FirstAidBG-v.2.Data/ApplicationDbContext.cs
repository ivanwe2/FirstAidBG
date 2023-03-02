using FirstAidBG_v._2.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FirstAidBG_v._2.Data
{
    public class ApplicationDbContext : DbContext
    {
        //      
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
            
        }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Answer>()
                .HasOne(b => b.AppUser)
                .WithMany(b => b.Answers)
                .HasForeignKey(b => b.AppUserId);
            //modelBuilder.Entity<Answer>()
            //    .HasOne(b => b.Question)
            //    .WithMany(b => b.Answers)
            //    .HasForeignKey(b => b.QuestionId);

            modelBuilder.Entity<Question>()
               .HasOne(b => b.User)
               .WithMany(b => b.Questions)
               .HasForeignKey(b => b.UserId);

            //modelBuilder.Entity<Question>()
            //   .HasMany(b => b.Answers);
               

            base.OnModelCreating(modelBuilder);
        }
    }
}
