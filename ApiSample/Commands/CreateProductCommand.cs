using MediatR;

namespace MCR.App.Commands
{
    public class CreateProductCommand : IRequest<bool>
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Tags { get; set; }
    }
}
