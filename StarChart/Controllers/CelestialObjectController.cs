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
            var celestial_obj = _context.CelestialObjects
                .FirstOrDefault(co => co.Name == name);
            if (celestial_obj == null)
            {
                return NotFound();
            }

            celestial_obj.Satellites = _context.CelestialObjects
                .Where(o => o.OrbitedObjectId == celestial_obj.Id)
                .ToList();

            return Ok(celestial_obj);
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
    }
}
