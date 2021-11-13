using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("{id:int}", Name ="GetById")]
        public IActionResult GetById(int id)
        {
            var celestial_obj = _context.CelestialObjects
                .FirstOrDefault(co => co.Id == id);
            if (celestial_obj == null)
            {
                return NotFound();
            }

            celestial_obj.Satellites = _context.CelestialObjects
                .Where(o => o.OrbitedObjectId == celestial_obj.Id)
                .ToList();

            return Ok(celestial_obj);
        }


        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestial_objs = _context.CelestialObjects
                .Where(co => co.Name == name)
                .ToList();
            if (!celestial_objs?.Any() ?? true)
            {
                return NotFound();
            }

            foreach (var obj in celestial_objs)
            {
                obj.Satellites = _context.CelestialObjects
                .Where(o => o.OrbitedObjectId == obj.Id)
                .ToList();
            }

            return Ok(celestial_objs);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var celestial_objs = _context.CelestialObjects;
            foreach (var obj in celestial_objs)
            {
                obj.Satellites = _context.CelestialObjects
                .Where(o => o.OrbitedObjectId == obj.Id)
                .ToList();
            }
          return Ok(celestial_objs);
        }

        [HttpPost]
        public IActionResult Create([FromBody]CelestialObject celestialObject)
        {
            _context.Add(celestialObject);
            _context.SaveChanges();
            return CreatedAtRoute("GetById", new { id = celestialObject.Id }, celestialObject);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject updatedCelestialObject)
        {
            var celestialObj = _context.CelestialObjects
                .FirstOrDefault(o => o.Id == id);
            if (celestialObj == null) return NotFound();
            celestialObj.Name = updatedCelestialObject.Name;
            celestialObj.OrbitalPeriod = updatedCelestialObject.OrbitalPeriod;
            celestialObj.OrbitedObjectId = updatedCelestialObject.OrbitedObjectId;
            _context.Update(celestialObj);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var celestialObj = _context.CelestialObjects
                .FirstOrDefault(o => o.Id == id);
            if (celestialObj == null) return NotFound();
            celestialObj.Name = name;
            _context.Update(celestialObj);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var celestial_objs = _context.CelestialObjects
                .Where(co => co.Id == id || co.OrbitedObjectId == id)
                .ToList();
            if (!celestial_objs?.Any() ?? true) return NotFound();
            _context.RemoveRange(celestial_objs);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
