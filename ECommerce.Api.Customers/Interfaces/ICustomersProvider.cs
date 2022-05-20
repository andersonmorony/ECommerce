using ECommerce.Api.Customers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Customers.Interfaces
{
    public interface ICustomersProvider
    {
        Task<(bool IsSuccess, IEnumerable<Customer> customers, string ErrorMessage)> getCustomersAsync();
        Task<(bool IsSuccess, Customer customer, string ErrorMessage)> getCustomerAsync(int id);

    }
}
