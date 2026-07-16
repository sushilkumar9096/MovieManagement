using System.Collections.Generic;

namespace MovieManagement.Domain.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        
        // Actor foreign key and reference
        public int ActorId { get; set; }
        public Actor? Actor { get; set; }
        
        // List of Genre (named 'Genre' as specified in prompt)
        public List<Genre> Genre { get; set; } = new List<Genre>();
    }
}
