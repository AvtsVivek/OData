using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace SimpleAirVinyl.Controllers
{

    public class PeopleController : ODataController
    {
        private readonly AirVinylDbContext _simpleAirVinylDbContext;

        public PeopleController(AirVinylDbContext simpleAirVinylDbContext)
        {
            _simpleAirVinylDbContext = simpleAirVinylDbContext
                ?? throw new ArgumentNullException(nameof(simpleAirVinylDbContext));
        }

        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(_simpleAirVinylDbContext.People);
        }
    }
}