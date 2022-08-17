using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperHeroAPI.Data;

namespace SuperHeroAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperHeroController : Controller
    {
        private static List<SuperHero> heroes = new List<SuperHero> {
                new SuperHero { Id = 1, Name = "Spider", FirstName = "Peter", LastName= "Park", Place = "NY"},
                new SuperHero{ Id = 2, Name = "IronMan", FirstName = "Tony", LastName= "Stark", Place = "NY"}
            };

        public readonly DataContext _context;

        public SuperHeroController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<SuperHero>>> Get()
        {
            //return Ok(heroes);    
            return Ok(await _context.Heroes.ToListAsync());    
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SuperHero>> Get(int id)
        {
            var hero = await _context.Heroes.FindAsync(id);
            if (hero == null)
                return BadRequest("Hero Not Found");
            return Ok(hero);
        }

        //[HttpGet("{name}")]
        //public async Task<ActionResult<SuperHero>> Get(string name)
        //{
        //    var hero = await _context.Heroes.FindAsync(name);
        //    if (hero == null)
        //        return BadRequest("Hero Not Found");
        //    return Ok(hero);
        //}

        [HttpPost]
        public async Task<ActionResult<List<SuperHero>>> AddHero(SuperHero hero)
        {
            //heroes.Add(hero);
            _context.Heroes.AddAsync(hero);
            await _context.SaveChangesAsync();

            return Ok(await _context.Heroes.ToListAsync());
        }

        [HttpPut]
        public async Task<ActionResult<List<SuperHero>>> UpdateHero(SuperHero request)
        {
            //var hero = heroes.Find(h => h.Id == request.Id);
            var dbHero = await _context.Heroes.FindAsync(request.Id) ;
            if (dbHero == null)
                return BadRequest("Hero Not Found");

            dbHero.Name = request.Name;
            dbHero.FirstName = request.FirstName;
            dbHero.LastName = request.LastName;
            dbHero.Place = request.Place;

            await _context.SaveChangesAsync();

            return Ok(await _context.Heroes.ToListAsync());

            //return Ok(heroes);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<SuperHero>>> DeleteHero(int id)
        {
            //var hero = heroes.Find(h => h.Id == id);
            var hero = await _context.Heroes.FindAsync(id);
            if (hero == null)
                return BadRequest("Hero Not Found");

            _context.Heroes.Remove(hero);

            await _context.SaveChangesAsync();


            return Ok(await _context.Heroes.ToListAsync());
        }
    }
}
