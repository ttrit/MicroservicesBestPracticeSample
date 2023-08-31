using MCR.App.Abstractions.EventBus;
using MCR.App.Commands;
using MCR.App.Models;
using MediatR;

namespace MCR.App.CommandHandlers
{
    internal sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, bool>
    {
        private readonly IEventBus _eventBus;

        public CreateProductCommandHandler()
        {
            
        }

        public async Task<bool> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new CreateProductCommand
            {
                Name = request.Name,
                Price = request.Price,
                Tags = request.Tags
            };

            await _eventBus.PublishAsync(new ProductCreatedEvent
            {
                Id = 123,
                Name = product.Name,
                Price = product.Price
            },
            cancellationToken);

            return true;
        }
    }
}
