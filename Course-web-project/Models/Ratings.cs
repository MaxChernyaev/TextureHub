namespace Course_web_project.Models
{
    public class Ratings
    {
        public int ID { get; set; }
        public float Rating { get; set; }

        /*public Textures? Textures { get; set; }
          public Users? Users { get; set; }*/

        public int TexturesId { get; set; } // внешний ключ
        public Textures? Textures { get; set; } // навигационное свойство
    }
}