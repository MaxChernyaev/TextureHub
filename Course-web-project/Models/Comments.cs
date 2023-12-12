using System.ComponentModel.DataAnnotations.Schema;

namespace Course_web_project.Models
{
    public class Comments
    {
        public int ID { get; set; }
        public string? text_comment { get; set; }

        /*public Textures? Textures { get; set; }
          public Users? Users { get; set; }*/

        [ForeignKey("TexturesId")]
        public int TexturesId { get; set; } // внешний ключ
        public Textures? Textures { get; set; } // навигационное свойство

        [ForeignKey("UsersId")]
        public int UsersId { get; set; } // внешний ключ
        public Users? Users { get; set; } // навигационное свойство
    }
}
