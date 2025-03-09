namespace StealCatsUI.Models
{
    public class TagEntity
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateTime created { get; set; } = DateTime.UtcNow;
        public ICollection<CatEntity> cats { get; set; } = new List<CatEntity>();
    }
}
