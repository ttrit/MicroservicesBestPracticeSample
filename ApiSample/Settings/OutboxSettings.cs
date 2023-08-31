using System.ComponentModel.DataAnnotations;

namespace MCR.App.Settings
{
    public class OutboxSettings
    {
        [Required]
        public int IntervalInSeconds { get; init; }

        [Range(1, 100)]
        public int BatchSize { get; init; }

        [Range(1, 5)]
        public int RetryThreshold { get; init; }
    }
}
