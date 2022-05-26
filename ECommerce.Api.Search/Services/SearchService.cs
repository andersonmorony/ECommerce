using ECommerce.Api.Search.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Search.Services
{
    public class SearchService : ISearchService
    {
        private readonly IOrdersService ordersService;
        private readonly IProductsService productsService;
        private readonly ICustomerService customerService;

        public SearchService(IOrdersService ordersService, IProductsService productsService, ICustomerService customerService)
        {
            this.ordersService = ordersService;
            this.productsService = productsService;
            this.customerService = customerService;
        }
        public async Task<(bool IsSuccess, dynamic SearchResults)> SearchAsync(int customerId)
        {
            var ordersResult = await ordersService.GetOrderAsync(customerId);
            var productsResult = await productsService.GetProductsAsync();
            
            if (ordersResult.IsSuccess)
            {
                var customer = await customerService.GetCustomerAsync(ordersResult.Orders.CustomerId);
                ordersResult.Orders.CustomerName = customer.IsSuccess ? customer.Customer.Name : "Customer information is not avaliable";

                foreach (var item in ordersResult.Orders.Items)
                {
                    item.ProductName = productsResult.IsSuccess ? productsResult.Products.FirstOrDefault(p => p.Id == item.ProductId).Name : "Product Information is not avaliable";
                }

                var result = new
                {
                    Orders = ordersResult.Orders
                };
                return (true, result);
            }
            return (false, null);
        }
    }
}
