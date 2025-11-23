using TaskerDomain.Models;

namespace TaskerDomain.Entities
{
    public class IEntity
    {
        public required string Id { get; set; }
        public required Event Created { get; set; }
        public Event? Modified { get; set; }
    }
}
