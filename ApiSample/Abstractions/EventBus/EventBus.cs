using MassTransit;

namespace MCR.App.Abstractions.EventBus
{
    internal sealed class EventBus : IEventBus
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public EventBus(IPublishEndpoint publishEndpoint)
        {   
            _publishEndpoint = publishEndpoint;
        }

        public Task PublishAsync<T>(T message, CancellationToken cancellation = default)
            where T : class =>
            _publishEndpoint.Publish<T>(message, cancellation);
    }
}
