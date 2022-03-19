using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace SimpleAirVinyl.Controllers
{
    //[Route("odata")]
    public class VinylRecordsController : ODataController
    {
        private readonly AirVinylDbContext _simpleAirVinylDbContext;

        public VinylRecordsController(AirVinylDbContext simpleAirVinylDbContext)
        {
            _simpleAirVinylDbContext = simpleAirVinylDbContext
                ?? throw new ArgumentNullException(nameof(simpleAirVinylDbContext));
        }

        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(_simpleAirVinylDbContext.VinylRecords);
        }

        //[HttpGet("VinylRecords({key})")]
        //public async Task<IActionResult> GetOneVinylRecord(int key)
        //{
        //    var vinylRecord = await _airVinylDbContext.VinylRecords
        //        .FirstOrDefaultAsync(p => p.VinylRecordId == key);

        //    if (vinylRecord == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(vinylRecord);
        //}
    }
}
