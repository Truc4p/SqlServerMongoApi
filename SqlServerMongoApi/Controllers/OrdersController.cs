using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using SqlServerMongoApi.Models;

namespace SqlServerMongoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly MongoDbContext _context;

        public OrdersController(MongoDbContext context)
        {
            _context = context;
        }

        // GET: api/orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            var orders = await _context.Orders.Find(order => true).ToListAsync();
            return Ok(orders);
        }

        // GET: api/orders/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(string id)
        {
            var order = await _context.Orders.Find(o => o.Id == id).FirstOrDefaultAsync();

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        // POST: api/orders
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            // Retrieve UserId from the cookie
            if (Request.Cookies.TryGetValue("UserId", out var userIdString) && int.TryParse(userIdString, out int userId))
            {
                order.UserId = userId;
                await _context.Orders.InsertOneAsync(order);
                return CreatedAtAction("GetOrder", new { id = order.Id }, order);
            }
            else
            {
                return BadRequest("UserId not found in cookies.");
            }
        }

        // DELETE: api/orders/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(string id)
        {
            var result = await _context.Orders.DeleteOneAsync(o => o.Id == id);
            if (result.DeletedCount == 0)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
