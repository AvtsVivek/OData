using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using OData0001010_Gadgets.Api.Models;
using OData0001010_Gadgets.Api.Services;

namespace OData0001010_Gadgets.Api.Controllers
{
    [ApiController]
    [Route("gadget")]
    public class GadgetController : ControllerBase
    {
        private readonly IGadgetsService _gadgetService;

        private readonly OdataDbContext _odataDbContext;
        public GadgetController(OdataDbContext odataDbContext, IGadgetsService studentService)
        {
            _odataDbContext = odataDbContext;
            _gadgetService = studentService;
        }


        [HttpGet("get")]
        [EnableQuery]
        public ActionResult<IQueryable<Product>> Get()
        {
            IQueryable<Product> retrievedGadgets = _gadgetService.RetrieveAllGadgets();

            //IQueryable<Gadgets> retrievedGadgets = _odataDbContext.Gadgets.AsQueryable();

            return Ok(retrievedGadgets);
        }
    }
}
