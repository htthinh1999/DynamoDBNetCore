using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DynamoDBNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IDynamoDBContext _context;
        public CustomersController(
            IDynamoDBContext context)
        {
            _context = context;
        }

        // GET: api/<UsersController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = await _context.ScanAsync<Customer>(default).GetRemainingAsync();
            return Ok(response);
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var response = await _context.LoadAsync<Customer>(new Customer
            {
                CustomerId = id
            });
            if (response == null) return NotFound();
            return Ok(response);
        }

        // POST api/<UsersController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] string username)
        {
            var customers = await _context.ScanAsync<Customer>(new List<ScanCondition>()
            {
                new ScanCondition(nameof(Customer.Username), ScanOperator.Equal, username)
            }).GetRemainingAsync();
            if (customers.Any()) return BadRequest($"Customer with Username {username} Already Exists");
            await _context.SaveAsync(new Customer
            {
                Username = username
            });
            return NoContent();
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] string value)
        {
            var response = await _context.LoadAsync<Customer>(id);
            return Ok(response);
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var customer = await _context.LoadAsync<Customer>(id);
            await _context.DeleteAsync(customer);
            return NoContent();
        }
    }
}
