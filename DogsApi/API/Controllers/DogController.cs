using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace API.Controllers
{
    [ApiController]
    public class DogController
    {
        private readonly IDogService dogService;

        public DogController(IDogService dogService)
        {
            this.dogService = dogService;
        }
        //GET: dogs
        [HttpGet]
        [Route("dogs")]

        public async Task<ActionResult<IEnumerable<DogModel>>> GetUsers([FromQuery] string? attribute = null, [FromQuery] string? order = null, [FromQuery] int? pageNumber = null, [FromQuery] int? pageSize = null)
        {
            var filter = new FilterModel
            {
                Attribute = attribute,
                Order = order,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            return new ObjectResult(await dogService.GetAllAsync(filter));
        }

        //POST: dog
        [HttpPost]
        [Route("dog")]
        public async Task<ActionResult> PostUser([FromBody] DogModel model)
        {
            try
            {
                await dogService.AddAsync(model);
                return new OkResult();
            }
            catch (ValidationException exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }
    }
}
