namespace Course_web_project.Models
{
    public class Textures
    {
        public int ID { get; set; }
        public string texture_name { get; set; } = ""; // имя файла
        public string texture_type { get; set; } = ""; // тип текстуры - water, ground, wood...
        public string pbr_or_seamless { get; set; } = ""; // тип текстуры - PBR или SEAMLESS

        public List<Comments> Comments { get; set; } = new();
        public List<Users> Users { get; set; } = new();
        public Ratings? Ratings { get; set; }

        /*public int UsersId { get; set; } // внешний ключ
        public Users? User { get; set; } // навигационное свойство*/
    }
}
