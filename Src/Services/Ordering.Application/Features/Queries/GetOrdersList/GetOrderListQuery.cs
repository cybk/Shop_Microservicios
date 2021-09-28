using MediatR;
using System.Collections.Generic;

namespace Ordering.Application.Features.Queries.GetOrdersList
{
    public class GetOrderListQuery : IRequest<List<OrdersViewModel>>
    {
        public string UserName { get; set; }

        public GetOrderListQuery(string userName)
        {
            UserName = userName;
        }
    }
}