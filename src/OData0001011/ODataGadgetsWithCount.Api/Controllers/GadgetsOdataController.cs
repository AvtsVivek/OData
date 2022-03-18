using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using OData0001010_Gadgets.Api.Services;

namespace OData0001010_Gadgets.Api.Controllers
{
    public class GadgetsOdataController : ControllerBase
    {
        private readonly IGadgetsService _gadgetService;

        private readonly OdataDbContext _odataDbContext;
        public GadgetsOdataController(OdataDbContext odataDbContext, IGadgetsService studentService)
        {
            _odataDbContext = odataDbContext;
            _gadgetService = studentService;
        }

        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(_odataDbContext.Gadgets.AsQueryable());
        }
    }
}
