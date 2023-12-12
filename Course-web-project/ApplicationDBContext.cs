using Course_web_project.Models;
using Microsoft.EntityFrameworkCore;

namespace Course_web_project
{
    public class ApplicationDBContext : DbContext
    {
        // конструктор
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options)
        {
            //Database.EnsureCreated();   // создаем базу данных при первом обращении
        }

        // функция, которая получает или устанавливает данные - работает с БД
        public DbSet<Textures> Textures { get; set; } = null!;
        /*public DbSet<PBRTextures> PBRTextures { get; set; } = null!;*/
        public DbSet<Users> Users { get; set; } = null!;
        public DbSet<Ratings> Ratings { get; set; } = null!;
        public DbSet<Comments> Comments { get; set; } = null!;
        /*public DbSet<Favorites> Favorites { get; set; } = null!;*/

        // функция создания модели, внутри можно описать данные, которые будут помещены в БД
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
