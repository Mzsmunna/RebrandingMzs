namespace Tasker.Presentation
{
    public abstract class IEntity
    {
        public string Creator { get; set; } = string.Empty;
        public string Modifier { get; set; } = string.Empty;
        public DateTime? CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedOn { get; set; }
    }
}
