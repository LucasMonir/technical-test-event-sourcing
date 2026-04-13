namespace TechnicalTest.Domain.Models
{
    public class ProjectionCheckpoint
    {
        public string Name { get; set; } = default!;
        public long LastProcessedEventId { get; set; }
    }
}
