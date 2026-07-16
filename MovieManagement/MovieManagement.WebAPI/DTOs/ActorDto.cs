using System.Collections.Generic;

namespace MovieManagement.WebAPI.DTOs
{
    public class ActorDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string BiographyDescription { get; set; } = string.Empty;
        public List<MovieDto> Movies { get; set; } = new List<MovieDto>();
    }
}
