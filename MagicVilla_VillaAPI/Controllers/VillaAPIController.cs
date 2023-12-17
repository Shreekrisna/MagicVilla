using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Controllers
{
    //[Route("api/controller")] 
    [Route("api/VillaAPI")] //This will be the route for this controller
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        //private readonly ILogger<VillaAPIController> _logger;

        //public VillaAPIController(ILogger<VillaAPIController> logger)//using Dependency Injection
        //{
        //    _logger = logger;
        //}

        private readonly ApplicationDbContext _db;
        public VillaAPIController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]//here httpget is the endpoint and we have to define httpget in top i.e right here
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDto>> GetVillas()
        {
            //_logger.LogInformation("Getting all Villas");
            return Ok(_db.Villas.ToList());
        }

        //if we want to get the one villa based on the id
        //[HttpGet("id:int")]
        [HttpGet("{id}", Name = "GetVilla")]//if we dont define the http verb,it defaults to HTTPGET
                                            //To define multiple response that can be produced we will use producesresponsetype attribute
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(200)]
        //[ProducesResponseType(404)]
        //[ProducesResponseType(400)]

        public ActionResult<VillaDto> GetVilla(int id)//return type will be actionresult of type villaDto
        {
            //Validation
            if (id == 0)
            {
               // _logger.LogError("Get villa Error with id" + id); 
                return BadRequest();
            }
            var villa = _db.Villas.FirstOrDefault(u => u.Id == id);
            if (villa == null)
            {
                return NotFound();
            }

            return Ok(villa);//villa
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDto> CreateVilla([FromBody] VillaDto obj)
        {
            //if(!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            //Custom validation and add them to model state
            if (_db.Villas.FirstOrDefault(u => u.Name.ToLower() == obj.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Name already exists");
                return BadRequest(ModelState);//modelstate
            }



            if (obj == null)
            {
                return BadRequest(obj);
            }
            if (obj.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            Villa model = new()
            {
                Name = obj.Name,
                Id = obj.Id,
                Amenity = obj.Amenity,
                Details = obj.Details,
                ImageUrl = obj.ImageUrl,
                Occupancy = obj.Occupancy,
                Rate = obj.Rate,
                Sqrft = obj.Sqrft,

            };
            _db.Villas.Add(model);
            _db.SaveChanges();
            
            // return Ok(obj);
            return CreatedAtRoute("GetVilla", new { id = obj.Id }, obj);
        }

        

        [HttpDelete("{id}",Name="DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult DeleteVilla(int id)
        {
            if(id==0)
            {
                return BadRequest();
            }

            var villa = _db.Villas.FirstOrDefault(u=>u.Id == id);
            if(villa==null)
            {
                return NotFound();
            }
            _db.Villas.Remove(villa);
            _db.SaveChanges();
            return NoContent();
                   
        }

        [HttpPut("{id}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        [ProducesResponseType(StatusCodes.Status200OK)]

        public IActionResult UpdateVilla(int id, [FromBody] VillaDto obj)
        {
            if(obj==null || id!=obj.Id)
            {
                return BadRequest();
            }

            //var villa=VillaStore.villalist.FirstOrDefault(u=>u.Id == id);
            //villa.Name=obj.Name;
            //villa.Sqrft=obj.Sqrft;
            //villa.Occupancy=obj.Occupancy;
            Villa model = new()
            {
                Name = obj.Name,
                Id = obj.Id,
                Amenity = obj.Amenity,
                Details = obj.Details,
                ImageUrl = obj.ImageUrl,
                Occupancy = obj.Occupancy,
                Rate = obj.Rate,
                Sqrft = obj.Sqrft,

            };
            _db.Villas.Update(model);
            _db.SaveChanges();
            return Ok(); 
        }

        [HttpPatch("{id}",Name ="UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult UpdatePartialVilla(int id,JsonPatchDocument<VillaDto> patchDTO)
        {
            if(patchDTO==null || id==0)
            {
                return BadRequest();
            }
            var villa = _db.Villas.AsNoTracking().FirstOrDefault(u => u.Id == id);
            VillaDto obj = new()
            {
                Name = villa.Name,
                Id = villa.Id,
                Amenity = villa.Amenity,
                Details = villa.Details,
                ImageUrl = villa.ImageUrl,
                Occupancy = villa.Occupancy,
                Rate = villa.Rate,
                Sqrft = villa.Sqrft,

            };
            if (villa==null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(obj, ModelState);
            Villa model = new()
            {
                Name = obj.Name,
                Id = obj.Id,
                Amenity = obj.Amenity,
                Details = obj.Details,
                ImageUrl = obj.ImageUrl,
                Occupancy = obj.Occupancy,
                Rate = obj.Rate,
                Sqrft = obj.Sqrft,

            };
            _db.Villas.Update(model);
            _db.SaveChanges();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }


    }
}
