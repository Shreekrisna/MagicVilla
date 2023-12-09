using MagicVilla_VillaAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/VillaAPI")] //This will be the route for this controller
    [ApiController]
    public class VillaAPIController:ControllerBase
    {
        public IEnumerable<Villa> GetVillas()
        {
            return new List<Villa>
            {
                new Villa{Id=1,Name="Pool View"},
                new Villa{Id=2,Name="Beach View"}
            };
        }

    }
}
