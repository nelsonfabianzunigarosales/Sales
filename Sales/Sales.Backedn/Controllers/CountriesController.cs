using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sales.Backedn.Data;
using Sales.Shared.Entities;
using System.Runtime.InteropServices;

namespace Sales.Backedn.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly DataContext _context;
        public CountriesController(DataContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> PostAsync(Country country)
        {
            _context.Add(country);
            await _context.SaveChangesAsync();
            return Ok(country);
        }
        [HttpGet]
        public async Task<IActionResult> GetAsync()//parameter by body 
        {
            return Ok(await _context.Countries.ToListAsync());
        }

        [HttpGet("{id}")]        
        public async Task<IActionResult> GetAsync(int id)//parameter by route 
        {
            var country = await _context.Countries.FirstOrDefaultAsync(c => c.Id == id);
            if(country==null)
            {
                return NotFound();
            }
            return Ok(country);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, Country country)//parameter by route 
        {
            var currentCountry = await _context.Countries.FirstOrDefaultAsync(c => c.Id == id);
            if (currentCountry == null)
            {
                return NotFound();
            }
            currentCountry.Name = country.Name;
            _context.Add(currentCountry);
            await _context.SaveChangesAsync();
            return Ok(currentCountry);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)//parameter by route 
        {
            var country = await _context.Countries.FirstOrDefaultAsync(c => c.Id == id);
            if (country == null)
            {
                return NotFound();
            }
            
            _context.Remove(country);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
