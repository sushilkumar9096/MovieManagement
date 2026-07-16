using System.Collections.Generic;

namespace MovieManagement.Domain.Entities
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        
        // List of Movies associated with this Genre
        public List<Movie> Movies { get; set; } = new List<Movie>();
    }
}
