using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers
{
    //[Route("api/controller")] 
    [Route("api/VillaAPI")] //This will be the route for this controller
    [ApiController]
    public class VillaAPIController:ControllerBase
    {
        [HttpGet]//here httpget is the endpoint and we have to define httpget in top i.e right here
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDto>> GetVillas()
        {
            return Ok(VillaStore.villalist);
        }

        //if we want to get the one villa based on the id
        //[HttpGet("id:int")]
        [HttpGet("id")]//if we dont define the http verb,it defaults to HTTPGET
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
            if(id==0)
            {
                return BadRequest();
            }
            var villa = VillaStore.villalist.FirstOrDefault(u => u.Id == id);
            if(villa==null)
            {
                return NotFound();
            }

            return Ok(villa);
        }

    }
}
