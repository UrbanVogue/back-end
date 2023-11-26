using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Queries.GetOrder
{
    public class GetOrderQuery : IRequest<OrderVm>
    {
        public int Id { get; set; }

        public GetOrderQuery(int id)
        {
            Id = id;
        }
    }
}
