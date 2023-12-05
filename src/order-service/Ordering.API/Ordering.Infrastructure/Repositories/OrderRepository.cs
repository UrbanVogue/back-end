using Microsoft.EntityFrameworkCore;
using Ordering.Application.Contracts.Persistence;
using Ordering.Domain.Entities;
using Ordering.Infrastructure.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Repositories
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(OrderContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserName(string userName)
        {
            var orderList = await _dbContext.Orders
                                .Where(o => o.UserName == userName).Include(o => o.Items)
                                .ToListAsync();
            return orderList;
        }

        public override async Task<Order> GetByIdAsync(int id)
        {
            var order = await _dbContext.Orders
                                .Where(o => o.Id == id).Include(o => o.Items)
                                .FirstOrDefaultAsync();
            return order;
        }
    }
}
