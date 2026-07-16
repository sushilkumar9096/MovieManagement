using System.Collections.Generic;

namespace MovieManagement.WebAPI.DTOs
{
    public class MovieCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int ActorId { get; set; }
        public List<int> GenreIds { get; set; } = new List<int>();
    }
}
