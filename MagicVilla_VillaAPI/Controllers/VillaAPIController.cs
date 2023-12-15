using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers
{
    //[Route("api/controller")] 
    [Route("api/VillaAPI")] //This will be the route for this controller
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        [HttpGet]//here httpget is the endpoint and we have to define httpget in top i.e right here
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDto>> GetVillas()
        {
            return Ok(VillaStore.villalist);
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
                return BadRequest();
            }
            var villa = VillaStore.villalist.FirstOrDefault(u => u.Id == id);
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
            if (VillaStore.villalist.FirstOrDefault(u => u.Name.ToLower() == obj.Name.ToLower()) != null)
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
            obj.Id = VillaStore.villalist.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
            VillaStore.villalist.Add(obj);
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

            var villa = VillaStore.villalist.FirstOrDefault(u=>u.Id == id);
            if(villa==null)
            {
                return NotFound();
            }
            VillaStore.villalist.Remove(villa);
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

            var villa=VillaStore.villalist.FirstOrDefault(u=>u.Id == id);
            villa.Name=obj.Name;
            villa.Sqrft=obj.Sqrft;
            villa.Occupancy=obj.Occupancy;
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
            var villa = VillaStore.villalist.FirstOrDefault(u => u.Id == id);
            if(villa==null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(villa, ModelState);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }


    }
}
