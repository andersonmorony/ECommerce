using AutoMapper;
using ECommerce.Api.Orders.Db;
using ECommerce.Api.Orders.Interfaces;
using ECommerce.Api.Orders.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Orders.Providers
{
    public class OrdersProvider : IOrderProvider
    {
        private readonly OrdersDbContext dbContext;
        private readonly ILogger logger;
        private readonly IMapper mapper;

        public OrdersProvider(OrdersDbContext dbContext, ILogger<OrdersProvider> logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;

            SeedData();

        }

        private void SeedData()
        {

            if(!dbContext.Orders.Any())
            {
                dbContext.Orders.Add(new Db.Order() { Id = 1, CustomerId = 1, OrderDate = new DateTime(), Total = 1, Items = null });
                dbContext.Orders.Add(new Db.Order() { Id = 2, CustomerId = 1, OrderDate = new DateTime(), Total = 1, Items = null });
                dbContext.Orders.Add(new Db.Order() { Id = 3, CustomerId = 1, OrderDate = new DateTime(), Total = 1, Items = null });
                dbContext.SaveChanges();
            }
        }

        public async Task<(bool isSuccess, Models.Order Order, string ErrorMessage)> getOrderAsync(int id)
        {
           try
            {
                var order = await dbContext.Orders.FirstOrDefaultAsync(order => order.Id == id);

                if (order != null)
                {
                    var result = mapper.Map<Db.Order, Models.Order>(order);

                    return (true, result, null);
                }

                return (false, null, "Not found");

            } catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool isSuccess, IEnumerable<Models.Order> Orders, string ErrorMessage)> getOrdersAsync()
        {
            try
            {
                var orders = await dbContext.Orders.ToListAsync();

                if (orders != null && orders.Any())
                {
                    var result = mapper.Map<IEnumerable<Db.Order>, IEnumerable<Models.Order>>(orders);

                    return (true, result, null);
                }

                return (false, null, "Not found");

            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}
