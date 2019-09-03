using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Capstone5_GCCar_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Capstone5_GCCar_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : Controller
    {
        private readonly CarDbContext _context;
        public CarController(CarDbContext context)
        {
            _context = context;

            if(_context.Car.Count() == 0)
            {
                _context.Car.Add(new Car { Make = "Ford", Model = "Focus", Year = 1996, Color = "Black" });
            }
        }
        // Get: api/Car/
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Car>>> GetCars()
        {
            var carList = await _context.Car.ToListAsync();
            return carList;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Car>> GetCarById(int id)
        {
            var car = await _context.Car.FindAsync(id);
            if(car == null)
            {
                return NotFound();
            }
            else
            {
                return car;
            }
        }
        [HttpPost]
        public async Task<ActionResult<Car>> PostCar(Car newCar)
        {
            _context.Car.Add(newCar);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCarById),new { id = newCar.Id },newCar);

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Car>> PutCar(int id, Car updateCar)
        {
            if (id != updateCar.Id)
            {
                return BadRequest();
            }
            _context.Entry(updateCar).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Car>> DeleteCar(int id)
        {
            var car = await _context.Car.FindAsync(id);
            if (car != null)
            {
                return NotFound();
            }
            _context.Car.Remove(car);
            await _context.SaveChangesAsync();
            return NoContent();
        }


    }
}