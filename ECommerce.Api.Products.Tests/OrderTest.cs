using AutoMapper;
using ECommerce.Api.Orders.Db;
using ECommerce.Api.Orders.Profiles;
using ECommerce.Api.Orders.Providers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ECommerce.Api.Products.Tests
{
    public class OrderTest
    {
        [Fact]
        public async Task GetOrdersShouldReturnAllOrders()
        {
            //Dbcontext
            var options = new DbContextOptionsBuilder<OrdersDbContext>().UseInMemoryDatabase(nameof(GetOrdersShouldReturnAllOrders)).Options;
            var dbContext = new OrdersDbContext(options);
            createOrders(dbContext);

            //Profile
            var orderProfile = new OrderProfile();
            var config = new MapperConfiguration(provider => provider.AddProfile(orderProfile));
            var mapper = new Mapper(config);

            //Provider
            var orderProvider = new OrdersProvider(dbContext, null, mapper);
            var ordersResult = await orderProvider.getOrdersAsync();

            Assert.True(ordersResult.isSuccess);
            Assert.True(ordersResult.Orders.Any());
            Assert.Null(ordersResult.ErrorMessage);

        }

        private void createOrders(OrdersDbContext dbContext)
        {
            for (int i = 1; i <= 10; i++)
            {
                dbContext.Orders.Add(new Order()
                {
                    Id = i,
                    CustomerId = i + 1,
                    OrderDate = DateTime.Now,
                    Total = i * 8,
                    Items = new List<OrderItem>() {
                        new OrderItem() { Id = i, OrderId = i, ProductId = i , Quantity = i * 100, UnitPrice = (decimal)i*3 },
                        new OrderItem() { Id = i + 100, OrderId = i, ProductId = i , Quantity = i * 50, UnitPrice = (decimal)i*3 },
                        new OrderItem() { Id = i + 200, OrderId = i, ProductId = i , Quantity = i * 40, UnitPrice = (decimal)i*3 },
                        new OrderItem() { Id = i + 300, OrderId = i, ProductId = i , Quantity = i * 7, UnitPrice = (decimal)i*3 }
                    }
                });
            }
            dbContext.SaveChanges();
        }
    }
}
