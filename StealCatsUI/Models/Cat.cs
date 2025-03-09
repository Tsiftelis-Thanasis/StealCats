namespace StealCatsUI.Models
{
    public class CatEntity
    {
        public int id { get; set; }
        public string catId { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public string image { get; set; }
        public DateTime created { get; set; } = DateTime.UtcNow;

        public ICollection<TagEntity> tags { get; set; } = new List<TagEntity>();
    }

   
}