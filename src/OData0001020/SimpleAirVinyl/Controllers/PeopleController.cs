using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

namespace SimpleAirVinyl.Controllers
{
    public class PeopleController : Controller
    {
        private readonly AirVinylDbContext _simpleAirVinylDbContext;

        public PeopleController(AirVinylDbContext simpleAirVinylDbContext)
        {
            _simpleAirVinylDbContext = simpleAirVinylDbContext
                ?? throw new ArgumentNullException(nameof(simpleAirVinylDbContext));
        }

        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            return Ok(await _simpleAirVinylDbContext.People.ToListAsync());
        }

    }
}
