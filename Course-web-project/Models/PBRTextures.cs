namespace Course_web_project.Models
{
    public class PBRTextures
    {
        public int ID { get; set; }
        public string texture_name { get; set; } = ""; // имя файла
        public string archive_name { get; set; } = ""; // путь до архива
        public string texture_type { get; set; } = ""; // тип текстуры - water, ground, wood...
    }
}
