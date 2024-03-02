using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestRESTAPI.Data;
using TestRESTAPI.Data.Models;
using TestRESTAPI.Models;

namespace TestRESTAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _db;

        public OrdersController(AppDbContext db)
        {
            _db = db;
        }
        [HttpGet("one/{orderId:int}")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            var order = await _db.Orders.Where(x => x.Id == orderId).FirstOrDefaultAsync();
            if (order != null)
            {
                dtoOrders dto = new()
                {
                    OrderId = order.Id,
                    OrderDate = order.CreatedDate,
                };
                if (order.OrderItems != null && order.OrderItems.Any())
                {
                    foreach (var item in order.OrderItems)
                    {
                        dtoOrdersItems dtoItem = new()
                        {
                            itemId = item.items.Id,
                            itemName = item.items.Name,
                            Price = item.items.Price,
                            quantity = 1
                        };
                        dto.items.Add(dtoItem);
                    }
                    return Ok(dto);
                }
            }
            return NotFound($"The Order Id {orderId} not Exists");
        }
        [HttpGet("[action]/{itemId:int}")]
        public async Task<IActionResult> GetOrderItemById(int itemId)
        {
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = _db.Orders.Include(t => t.OrderItems).ThenInclude(t => t.items).ToArray();
            return Ok(orders);
        }
        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody]dtoOrders order)
        {
            if(ModelState.IsValid)
            {
                Order mdl = new()
                {
                    CreatedDate = order.OrderDate,
                    OrderItems = new List<OrderItem>()
                };
                foreach (var item in order.items)
                {
                    OrderItem orderItem = new()
                    {
                        ItemId = item.itemId,
                        Price = item.Price,
                    };
                    mdl.OrderItems.Add(orderItem);
                }
                await _db.Orders.AddAsync(mdl);
                await _db.SaveChangesAsync();
                order.OrderId = mdl.Id;
                return Ok(order);
            }
            return BadRequest();
        }

    }
}
