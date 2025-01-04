using MagicVilla_VillaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Data;
using System.Xml.Linq;
using System.Buffers.Text;
using Microsoft.AspNetCore.JsonPatch;
using MagicVilla_VillaAPI.Logging;

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
        // We will comment the default logger here and use the new logger which we implement.
        private readonly ILogging _logger;
        //Implementation of the ILogging interface
        public VillaAPIController(ILogging logger)
        {
            _logger = logger;
        }

        /*
         
        // Use dependency injection for Implementation of the logger
        // Constructor - ctor + tab + tab (shortcut)

        // Ilogger is a generic interface that represents a type used to perform logging.
        private readonly ILogger<VillaAPIController> _logger;

        // private readonly ILogger<VillaAPIController> _logger;
        // Declares a private field _logger in the VillaAPIController class.
        // The readonly keyword ensures that the _logger field can only be assigned a value during initialization or in the constructor. This makes the field immutable after the object is constructed.
        // The type of the field, ILogger<VillaAPIController>, is an interface provided by ASP.NET Core's logging framework.
        // The generic parameter <VillaAPIController> specifies the source of the logs, helping to categorize logs by the class that generated them.

        // This is the constructor of the VillaAPIController class.
        // It accepts a parameter of type ILogger<VillaAPIController> named logger.
        public VillaAPIController(ILogger<VillaAPIController> logger)
        {
            _logger = logger;
            // Assigns the injected logger instance to the private _logger field. This makes the logger available for use throughout the controller.
        }

        */

        // IEnumerable<Villa>  represents a collection of VillaDTO objects that can be enumerated using a foreach loop.
        // When we expose the data to the end user of the controller, we do not want to send the date - use VillaDTO
        [HttpGet]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            _logger.Log("Getting all villas", "");
            return Ok(VillaStore.villaList);     
        }

        // In order to define that this HttpGet call expects an id parameter, we have to explicitly write the id parameter.
        // [HttpGet("id")] - works fine
        [HttpGet("{id:int}", Name ="GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // [ProducesResponseType(200, Type = typeof(VillaDTO))] - Use this if you want to specify the type of the response (in case you don't do it like the code below)
        
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            if(id == 0)
            {
                _logger.Log("Get Villa Error with Id " + id, "error");
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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDTO> CreateVilla([FromBody]VillaDTO villaDTO)
        {
            //// VillaDTO model
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            // Custom validation for unique villa name
            if(VillaStore.villaList.FirstOrDefault(u=>u.Name.ToLower() == villaDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Villa name must be unique"); // First parameter denotes the key name - has to be unique but can be empty
                return BadRequest(ModelState);
            }
            if (villaDTO == null)
            {
                return BadRequest(villaDTO);
            }
            // When we are creating a villa, the Id of the villa should be 0.
            if(villaDTO.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            // fetch the Id for the new villa
            villaDTO.Id = VillaStore.villaList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
            VillaStore.villaList.Add(villaDTO);
            // OrderByDescending sorts the list of villas in descending order based on their Id property.
            // This means the villa with the highest ID will be the first element in the sorted list.
            // OrderByDescending(u => u.Id) - Sorts the villaList in descending order based on the Id property of each villa(u represents a villa in the list).

            // return Ok(villaDTO);
            // When the resource is created, provide the URL where the resource can be accessed.
            // Provide a link so that they can call the GetVilla method with the id of the newly created villa.
            return CreatedAtRoute("GetVilla", new { id = villaDTO.Id }, villaDTO);
            // In order to find the resource where it was created, we need to call the GetVilla method.
            // Pass the id in a new object and pass the villaDTO object.
        }

        // ActionResult<T>: Strongly Typed - ActionResult<T> is a generic type that combines the flexibility of IActionResult with the type safety of specifying a return type.
        // IActionResult: When you use IActionResult, the return type of the method is not tied to any specific data type. You have full flexibility but lose compile-time type checking for your responses.
        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteVilla(int id)
        {
            if (id ==0)
            {
                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            VillaStore.villaList.Remove(villa);
            return NoContent();

        }

        // HttpPut will update the complete villa record
        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateVilla(int id, [FromBody] VillaDTO villaDTO)
        {
            if (villaDTO == null || id != villaDTO.Id)
            {
                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
            villa.Name = villaDTO.Name;
            villa.Occupacy = villaDTO.Occupacy;
            villa.Sqft = villaDTO.Sqft;

            return NoContent();
        }

        // In order to update only one property, we can use the HttpPatch method.
        // Add the Nuget package - Microsoft.AspNetCore.JsonPatch
        // https://jsonpatch.com/
        // Replace
        // "path": "/name"
        // path - property name that needs to be updated

        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
            patchDTO.ApplyTo(villa, ModelState);
            // if there are any errors, store them in the ModelState
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }


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

// The response types (ActionResult<IEnumerable<VillaDTO>> or ActionResult<VillaDTO>) and the usage of ProducesResponseType attributes in the code are designed to Define the API's response contract.
// The return type of the methods (ActionResult<T> or IActionResult) informs consumers of the API (and tools like Swagger) what data and HTTP status codes they can expect from each endpoint.

// The logger part in the VillaAPIController is implemented using the ILogger interface, which is part of the ASP.NET Core logging framework.