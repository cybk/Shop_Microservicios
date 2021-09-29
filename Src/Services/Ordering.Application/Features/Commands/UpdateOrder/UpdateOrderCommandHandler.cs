using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;
using Ordering.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Commands.UpdateOrder
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
    {
        private readonly IAsyncRepository<Order> repository;
        private readonly ILogger<UpdateOrderCommandHandler> logger;
        private readonly IMapper mapper;

        public UpdateOrderCommandHandler(IAsyncRepository<Order> repository, ILogger<UpdateOrderCommandHandler> logger, IMapper mapper)
        {
            this.repository = repository;
            this.logger = logger;
            this.mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var orderToUpdate = await repository.GetByIdAsync(request.Id);

            if (orderToUpdate == null)
            {
                logger.LogError("La orden no existe en la base de datos");
            }

            mapper.Map(request, orderToUpdate, typeof(UpdateOrderCommand), typeof(Order));
            await repository.UpdateAsync(orderToUpdate);

            return Unit.Value;
        }
    }
}