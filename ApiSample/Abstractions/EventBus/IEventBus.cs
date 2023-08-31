namespace MCR.App.Abstractions.EventBus
{
    public interface IEventBus
    {
        Task PublishAsync<T>(T message, CancellationToken cancellation = default)
            where T : class;
    }
}
