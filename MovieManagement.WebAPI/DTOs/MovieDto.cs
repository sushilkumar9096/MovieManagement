using System.Collections.Generic;

namespace MovieManagement.WebAPI.DTOs
{
    public class MovieDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<ActorShortDto> Actors { get; set; } = new List<ActorShortDto>();
        public List<string> Genres { get; set; } = new List<string>();
    }
}
