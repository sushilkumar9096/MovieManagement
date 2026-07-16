namespace MovieManagement.Domain.Entities
{
    public class Biography
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        
        // Actor foreign key and reference
        public int ActorId { get; set; }
        public Actor? Actor { get; set; }
    }
}
