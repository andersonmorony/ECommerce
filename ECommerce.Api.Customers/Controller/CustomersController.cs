using ECommerce.Api.Customers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Customers.Controller
{
    [ApiController]
    [Route("api/customers")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomersProvider _customersProvide;

        public CustomersController(ICustomersProvider customersProvide)
        {
            this._customersProvide = customersProvide;
        }
        [HttpGet]
        public async Task<IActionResult> GetCustomersAsync()
        {
            var result = await _customersProvide.getCustomersAsync();

            if (result.IsSuccess)
            {
                return Ok(result.customers);
            }

            return NotFound();
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerAsync(int id)
        {
            var result = await _customersProvide.getCustomerAsync(id);

            if(result.IsSuccess)
            {
                return Ok(result.customer);
            }

            return NotFound();
        }
    }
}
