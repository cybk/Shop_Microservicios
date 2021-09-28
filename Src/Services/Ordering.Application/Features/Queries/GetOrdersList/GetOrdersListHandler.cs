using AutoMapper;
using MediatR;
using Ordering.Application.Contracts.Persistence;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Queries.GetOrdersList
{
    public class GetOrdersListHandler : IRequestHandler<GetOrderListQuery, List<OrdersViewModel>>
    {
        public readonly IAsyncRepository<Order> repository;
        private readonly IMapper mapper;

        public GetOrdersListHandler(IAsyncRepository<Order> repo, IMapper map) 
            => (repository, mapper) = (repo, map);



        public async Task<List<OrdersViewModel>> Handle(GetOrderListQuery request, CancellationToken cancellationToken)
        {
            var orders = await repository.GetAsync(o => o.UserName == request.UserName);
            return mapper.Map<List<OrdersViewModel>>(orders);
        }
    }
}
