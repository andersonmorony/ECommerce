using ECommerce.Api.Orders.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Orders.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderProvider orderProvider;

        public OrderController(IOrderProvider orderProvider)
        {
            this.orderProvider = orderProvider;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrdersAsync()
        {
            var result = await orderProvider.getOrdersAsync();

            if(result.isSuccess)
            {
                return Ok(result.Orders);
            }

            return NotFound();
        }
        [HttpGet]
        [Route("api/orderitems")]
        public async Task<IActionResult> GetOrderItemsAsync()
        {
            var result  = await orderProvider.getOrderItemsAsync();

            if(result.isSuccess)
            {
                return Ok(result.Orders);
            }
            return NotFound();
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderAsync( int id )
        {
            var result = await orderProvider.getOrderAsync(id);

            if(result.isSuccess)
            {
                return Ok(result.Order);
            }

            return NotFound();
        } 
    }
}
