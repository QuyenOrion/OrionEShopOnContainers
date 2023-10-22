using OrionEShopOnContainer.Services.Ordering.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrionEShopOnContainer.Services.Ordering.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderingContext _context;

        public OrderRepository(OrderingContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IUnitOfWork UnitOfWork => _context;

        public Order Add(Order order)
        {
            return _context.Add(order).Entity;
        }

        public async Task<Order> GetAsync(int orderId)
        {
            var order = _context.Orders.Include(o => o.Address).FirstOrDefault(o => o.Id == orderId);

            if(order == null)
            {
                order = _context.Orders.Local.FirstOrDefault(o => o.Id == orderId);
            }
            else
            {
                await _context.Entry(order).Collection(o => o.OrderItems).LoadAsync();
                await _context.Entry(order).Reference(o => o.OrderStatus).LoadAsync();
            }

            return order;
        }

        public void Update(Order order)
        {
            _context.Entry(order).State = EntityState.Modified;
        }
    }
}
