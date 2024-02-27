using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestRESTAPI.Data;
using TestRESTAPI.Data.Models;

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
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = _db.Orders.ToArray();
            return Ok(orders);
        }
        [HttpPost]
        public async Task<IActionResult> AddOrder(Order order)
        {
            return Ok(order);
        }

    }
}
