using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechBookingAPI.Models.ORM;

namespace TechBookingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly TechDbContext _context;

        public CompaniesController(TechDbContext context)
        {
            _context = context;
        }

        // GET: api/Companies
        [HttpGet]
        public IActionResult GetCompanies()
        {
            var companies = _context.Companies.ToList();
            return Ok(companies);
           
        }
        // GET: api/Companies/5

        [HttpGet("{id}")]
        public IActionResult GetCompany(int id)
        {
            var company = _context.Companies.Find(id);
            if (company == null)
            {
                return NotFound();
            }
            return Ok(company);
        }

        [HttpPost]
        public IActionResult PostCompany(Company company)
        {
            _context.Companies.Add(company);
            _context.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }

        //DELETE: api/Companies/5
        [HttpDelete("{id}")]
        public IActionResult DeleteCompany(int id)
        {
            var company = _context.Companies.Find(id);
            if (company == null)
            {
                return NotFound();
            }
            _context.Companies.Remove(company);
            _context.SaveChanges();
            return Ok(company);
        }
        // PUT: api/Companies/5
        [HttpPut("{id}")]
        public IActionResult UpdateCompany(int id, [FromBody] Company updatedCompany)
        {
            if (id != updatedCompany.Id)
            {
                return BadRequest("Company id mismatch");
            }

            // İlgili şirketi veritabanından bul
            var company = _context.Companies.Find(id);
            if (company == null)
            {
                return NotFound();
            }

            // Şirketin alanlarını güncelle
            company.Name = updatedCompany.Name;
            company.Email = updatedCompany.Email;

            // Veritabanını güncelle
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyExists(id))
                {
                    return NotFound("Client not found");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool CompanyExists(int id)
        {
            return _context.Companies.Any(e => e.Id == id);
        }
    }
}
