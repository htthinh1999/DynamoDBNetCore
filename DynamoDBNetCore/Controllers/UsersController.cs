using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DynamoDBNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly DynamoDBContext _amazonDynamoDb;
        public CustomersController(
            DynamoDBContext amazonDynammoDb)
        {
            _amazonDynamoDb = amazonDynammoDb;
        }

        // GET: api/<UsersController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = await _amazonDynamoDb.ScanAsync<Customer>(
                new List<ScanCondition>() { }
            ).GetRemainingAsync();
            return Ok(response);
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var response = await _amazonDynamoDb.ScanAsync<Customer>(new List<ScanCondition>()
            {
                new ScanCondition(nameof(Customer.CustomerId), ScanOperator.Equal, id)
            }).GetRemainingAsync();
            return Ok(response);
        }

        // POST api/<UsersController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] string username)
        {
            await _amazonDynamoDb.SaveAsync(new Customer
            {
                Username = username
            });
            return Ok();
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public void Put(string id, [FromBody] string value)
        {
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
        }
    }
}
