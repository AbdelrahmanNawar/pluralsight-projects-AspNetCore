using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Pages.Internal.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var result = _context.CelestialObjects.FirstOrDefault(c => c.Id == id);

            if (result is null)
                return NotFound();

            if (result.Id == result.OrbitedObjectId)
                result.Satellites.Add(result);

            return Ok(result);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var result = _context.CelestialObjects.Where(c => c.Name == name);

            if (result.Count() == 0)
                return NotFound();

            foreach (var item in result)
            {
                item.Satellites.Add(item);
            }

            return Ok(result);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _context.CelestialObjects.ToList();

            foreach (var item in result)
            {
                item.Satellites.Add(item);
            }

            return Ok(result);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CelestialObject celestial)
        {
            _context.CelestialObjects.Add(celestial);
            _context.SaveChanges();
            return CreatedAtRoute("GetById", new { celestial.Id }, celestial);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject celestial)
        {
            var result = _context.CelestialObjects.FirstOrDefault(c => c.Id == id);
            if (result is null)
                return NotFound();

            result.Name = celestial.Name;
            result.OrbitalPeriod = celestial.OrbitalPeriod;
            result.OrbitedObjectId = celestial.OrbitedObjectId;
            _context.CelestialObjects.Update(result);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var result = _context.CelestialObjects.FirstOrDefault(c => c.Id == id);
            if (result is null)
                return NotFound();

            result.Name = name;
            _context.CelestialObjects.Update(result);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete (int id)
        {
            var result = _context.CelestialObjects.FirstOrDefault(c => c.Id == id);
            if (result is null)
                return NotFound();

            _context.CelestialObjects.RemoveRange(result);
            _context.SaveChanges();
            return NoContent();
        }

    }
}
