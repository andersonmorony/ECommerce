using ECommerce.Api.Orders.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Orders.Interfaces
{
    public interface IOrderProvider
    {
        Task<(bool isSuccess, IEnumerable<Order> Orders, string ErrorMessage)> getOrdersAsync();
        Task<(bool isSuccess, IEnumerable<OrderItem> Orders, string ErrorMessage)> getOrderItemsAsync();
        Task<(bool isSuccess, Order Order, string ErrorMessage)> getOrderAsync(int id);

    }
}
