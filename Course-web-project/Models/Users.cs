using System.Text.Json.Serialization;

namespace Course_web_project.Models
{
    public class Users
    {
        public int ID { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }

        public List<Textures> Textures { get; set; } = new();

        /*public int TexturesId { get; set; } // внешний ключ
        public Textures? Texture { get; set; } // навигационное свойство*/
    }
}
