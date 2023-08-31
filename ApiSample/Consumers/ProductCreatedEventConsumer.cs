﻿using MassTransit;
using MCR.App.Models;

namespace MCR.App.Consumers
{
    public sealed class ProductCreatedEventConsumer : IConsumer<ProductCreatedEvent>
    {
        private readonly ILogger<ProductCreatedEventConsumer> _logger;

        public ProductCreatedEventConsumer(ILogger<ProductCreatedEventConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<ProductCreatedEvent> context)
        {
            _logger.LogInformation("Product created: {@Product}", context.Message);

            return Task.CompletedTask;
        }
    }
}
