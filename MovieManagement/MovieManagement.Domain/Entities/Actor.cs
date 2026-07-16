using System.Collections.Generic;

namespace MovieManagement.Domain.Entities
{
    public class Actor
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        
        // 1-to-1 biography reference
        public Biography? Biography { get; set; }
        
        // List of Movies
        public List<Movie> Movies { get; set; } = new List<Movie>();
    }
}
