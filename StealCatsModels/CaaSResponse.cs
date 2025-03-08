namespace StealCatsModels
{
    public class CaaSResponse
    {
        public string id { get; set; }
        public string? url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public List<Breed> breeds { get; set; } = new();
    }
}