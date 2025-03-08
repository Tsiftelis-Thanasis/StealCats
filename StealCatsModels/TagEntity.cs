namespace StealCatsModels
{
    public class TagEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public ICollection<CatEntity> Cats { get; set; } = new List<CatEntity>();
    }
}