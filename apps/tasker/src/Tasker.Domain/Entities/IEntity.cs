using Tasker.Domain.Models;

namespace Tasker.Domain.Entities
{
    public class IEntity
    {
        public required string Id { get; set; }
        public required AppEvent Created { get; set; }
        public AppEvent? Modified { get; set; }
    }
}
