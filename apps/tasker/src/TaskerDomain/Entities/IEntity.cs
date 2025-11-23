using TaskerDomain.Models;

namespace TaskerDomain.Entities
{
    public class IEntity
    {
        public required string Id { get; set; }
        public required Event Creator { get; set; }
        public Event? Modifier { get; set; }
    }
}
