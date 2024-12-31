using MagicVilla_VillaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Data;
using System.Xml.Linq;

// After declaring a namespace, subsequent code belongs to that namespace unless explicitly overridden.
namespace MagicVilla_VillaAPI.Controllers
{
    // In order to make this a controller class, it has to derive from ControllerBase class.
    // ControllerBase contains the common methods for returning all the data and users that is related to the controller.
    // Use ControllerBase for APIs that do not need to return views.
    // Use Controller for APIs that need to return views (e.g., Razor Views or HTML pages).

    // [Route ("api/[controller]")] - This is a token replacement. The token [controller] is replaced by the name of the controller, which in this case is VillaAPI.

    [Route ("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        // IEnumerable<Villa>  represents a collection of VillaDTO objects that can be enumerated using a foreach loop.
        // When we expose the data to the end user of the controller, we do not want to send the date - use VillaDTO
        [HttpGet]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            return Ok(VillaStore.villaList);     
        }

        // In order to define that this HttpGet call expects an id parameter, we have to explicitly write the id parameter.
        // [HttpGet("id")] - works fine
        [HttpGet("{id:int}")]
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            if(id == 0)
            {
                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            return Ok(villa);
            // retrieves the first element from the VillaStore.villaList that matches the condition u.Id == id, or returns null if no such element is found.
        }
        // Why the Return Type Cannot Be IEnumerable<VillaDTO>
        // The return type of the GetVillas method cannot be IEnumerable<VillaDTO> because the method is supposed to return a single VillaDTO object, not a collection of VillaDTO objects.


    }
}
// Why DTO?
// In the controller here, we are directly using the Villa model.
// In production, we have dtos - dtos provide a wrapper between the entity or the database model and what is being exposed from the API.

// Debug-help:
// Whenever you cannot find a reasonable direction from the swagger endpoint, you can always go to the controller and see what the endpoint is.
// In this case, the endpoint is localhost:44350/api/villaAPI 
// Swagger + Code

// ActionResult:
// ActionResult is an abstract class that represents the result of an action method.
// The ActionResult type is a versatile return type in ASP.NET Core controllers. It allows you to return:
// 1. A specific result object (e.g., VillaDTO).
// 2. An HTTP response with a status code and optional data.