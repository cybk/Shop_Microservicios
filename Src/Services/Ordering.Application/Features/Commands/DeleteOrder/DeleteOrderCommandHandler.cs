using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;
using Ordering.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Commands.DeleteOrder
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
    {
        private readonly IAsyncRepository<Order> repository;
        private readonly ILogger<DeleteOrderCommand> logger;

        public DeleteOrderCommandHandler(IAsyncRepository<Order> repository, ILogger<DeleteOrderCommand> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var orderToUpdate = await repository.GetByIdAsync(request.Id);

            if (orderToUpdate != null)
            {
                logger.LogError("La orden no existe");
            }

            await repository.DeleteAsync(orderToUpdate);
            return Unit.Value;
        }
    }
}