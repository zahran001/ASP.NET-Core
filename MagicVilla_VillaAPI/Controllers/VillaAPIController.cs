using MagicVilla_VillaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Data;
using System.Xml.Linq;
using System.Buffers.Text;
using Microsoft.AspNetCore.JsonPatch;
using MagicVilla_VillaAPI.Logging;
using Microsoft.EntityFrameworkCore;

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
        // ASP.NET Core does not support multiple constructors with dependency injection.

        /*
        
        // We will comment the default logger here and use the new logger which we implement.
        private readonly ILogging _logger;
        //Implementation of the ILogging interface
        public VillaAPIController(ILogging logger)
        {
            _logger = logger;
        }

        */



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

        /*
         
        We won't be using the VillaStore class anymore. We will be using the ApplicationDbContext class to interact with the database.
        We already added that to the container when we registered the DbContext in the Program.cs file.
        Since we have registered the service in the container, we can extract that using DI.
         
        */

        // Create a private readonly field of type ApplicationDbContext named _db.
        private readonly ApplicationDbContext _db;
        private readonly ILogging _logger;
        // Add a constructor that accepts an ApplicationDbContext parameter and assigns it to the _db field.
        public VillaAPIController(ILogging logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }
        /*
        The above code snippet declares a private readonly field _db of type ApplicationDbContext and assigns it through the constructor. 
        This setup allows the VillaAPIController to use the ApplicationDbContext instance for database operations, leveraging the dependency injection system in ASP.NET Core to provide the necessary configuration and lifecycle management.
        */


        // IEnumerable<Villa>  represents a collection of VillaDTO objects that can be enumerated using a foreach loop.
        // When we expose the data to the end user of the controller, we do not want to send the date - use VillaDTO
        [HttpGet]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            _logger.Log("Getting all villas", "");
            return Ok(_db.Villas.ToList());     
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
            var villa = _db.Villas.FirstOrDefault(u => u.Id == id);
            // We do NOT have to write any SQL, EF Core will automatically translate this into SQL.
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
            if(_db.Villas.FirstOrDefault(u=>u.Name.ToLower() == villaDTO.Name.ToLower()) != null)
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
            // fetch the Id for the new villa - NO LONGER NEEDED - EF Core will automatically generate the Id since it's an identity column.
            //villaDTO.Id = VillaStore.villaList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
            //VillaStore.villaList.Add(villaDTO);

            // It cannot implicitly convert VillaDTO to Villa - we have to do a manual conversion.
            // new () is shorthand for creating a new instance of the Villa class using its parameterless constructor.
            Villa model = new ()
            {
                Amenity = villaDTO.Amenity,
                Details = villaDTO.Details,
                Id = villaDTO.Id,
                ImageUrl = villaDTO.ImageUrl,
                Name = villaDTO.Name,
                Occupacy = villaDTO.Occupacy,
                Rate = villaDTO.Rate,
                Sqft = villaDTO.Sqft,
            };
            _db.Villas.Add(model);
            _db.SaveChanges(); // gather all the changes that it has to make and push it to the database.

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
            var villa = _db.Villas.FirstOrDefault(u => u.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            // When we retrieve it from the database, it will be of type Villa - no conversion needed
            _db.Villas.Remove(villa);
            _db.SaveChanges();
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

            //var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
            //villa.Name = villaDTO.Name;
            //villa.Occupacy = villaDTO.Occupacy;
            //villa.Sqft = villaDTO.Sqft;

            // EF Core will figure out what records to update based on the Id
            // Convert VillaDTO to Villa object
            Villa model = new()
            {
                Amenity = villaDTO.Amenity,
                Details = villaDTO.Details,
                Id = villaDTO.Id,
                ImageUrl = villaDTO.ImageUrl,
                Name = villaDTO.Name,
                Occupacy = villaDTO.Occupacy,
                Rate = villaDTO.Rate,
                Sqft = villaDTO.Sqft,
            };

            _db.Villas.Update(model);
            _db.SaveChanges();

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
            var villa = _db.Villas.AsNoTracking().FirstOrDefault(u => u.Id == id); // retrieve the actual villa
            // In the patch request, we don't get the complete object, we get only the properties that had to be updated. 

            // When we are applying here, the type is VillaDTO
            // We have to convert this Villa object to a VillaDTO object

            VillaDTO villaDTO = new()
            {
                Amenity = villa.Amenity,
                Details = villa.Details,
                Id = villa.Id,
                ImageUrl = villa.ImageUrl,
                Name = villa.Name,
                Occupacy = villa.Occupacy,
                Rate = villa.Rate,
                Sqft = villa.Sqft,
            };        
            if (villa == null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(villaDTO, ModelState);
            // Any changes that we have in PatchDTO, which is of type VillaDTO, will be applied to this local variable villaDTO.
            // Once we have that, then we can update the record.
            // In order to call the update in EF Core, we have to convert this VillaDTO object back to a Villa object.
            // We have to do the reverse of what we did in the beginning before we can update that.

            Villa model = new()
            {
                Amenity = villaDTO.Amenity,
                Details = villaDTO.Details,
                Id = villaDTO.Id,
                ImageUrl = villaDTO.ImageUrl,
                Name = villaDTO.Name,
                Occupacy = villaDTO.Occupacy,
                Rate = villaDTO.Rate,
                Sqft = villaDTO.Sqft,
            };

            _db.Villas.Update(model);
            _db.SaveChanges();

            // if there are any errors, store them in the ModelState
            if (!ModelState.IsValid)
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