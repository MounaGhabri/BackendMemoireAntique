using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projet.Data;
using Projet.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Projet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class catController2 : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public catController2(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/<catController2>

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> Get()
        {
            var categories = await _context.Categories.ToListAsync();
            return Ok(categories);
        }

        /*
        public IEnumerable<string> Get()
        {
            var values = new List<string> { "value1", "value2" };
            return (IEnumerable<string>)(values);

            //return new string[] { "value1", "value2" };
        }
        */
        // GET api/<catController2>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<catController2>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<catController2>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<catController2>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
