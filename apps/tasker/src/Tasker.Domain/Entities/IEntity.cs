using Tasker.Domain.Models;

namespace Tasker.Domain.Entities
{
    public class IEntity
    {
        public required string Id { get; set; }
        public required Event Created { get; set; }
        public Event? Modified { get; set; }
    }
}
