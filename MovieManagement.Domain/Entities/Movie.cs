using System.Collections.Generic;

namespace MovieManagement.Domain.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        
        // List of Actors
        public List<Actor> Actors { get; set; } = new List<Actor>();
        
        // List of Genre (named 'Genre' as specified in prompt)
        public List<Genre> Genre { get; set; } = new List<Genre>();
    }
}
