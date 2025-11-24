using Tasker.Domain.Models;

namespace Tasker.Domain.Entities
{
    public class IEntity
    {
        public required string Id { get; set; }
        public required EventLog Created { get; set; }
        public EventLog? Modified { get; set; }
    }
}
