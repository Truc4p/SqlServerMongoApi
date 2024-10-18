using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SqlServerMongoApi.Models;

namespace SqlServerMongoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.Include(u => u.Addresses).ToListAsync();
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.Include(u => u.Addresses).FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            // Store the UserId in a cookie
            Response.Cookies.Append("UserId", user.Id.ToString(), new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });

            return user;
        }

        // PUT: api/users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/users
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        [HttpPost("/add-address/{id}")]
        public async Task<IActionResult> AddAddress(int id, Address address)
        {
            if (!UserExists(id)){
                return BadRequest("User does not exist");
            }

            var user = await _context.Users.FindAsync(id);
            user.Addresses.Add(address);
            await _context.SaveChangesAsync();

            return Ok("Address added");
        }



        [HttpPost("/create-user-with-address")]
        public async Task<IActionResult> CreateUserWithAddress(CreateUserWithAddressRequest request)
        {
            var newAddress = new Address
            {
                Name = request.Address
            };

            var newUser = new User
            {
                Username = request.Username,
                Password = request.Password,
                Addresses = new List<Address> { newAddress }
            };  

            _context.Addresses.Add(newAddress);
            _context.Users.Add(newUser);

  
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = newUser.Id }, newUser);
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }

    public class CreateUserWithAddressRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
    }
}
