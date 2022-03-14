using AirVinyl.API.DbContexts;
using AirVinyl.API.Helpers;
using AirVinyl.Entities;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirVinyl.Controllers
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }
    }

    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }


    public class PeopleController : ODataController
    {
        private readonly AirVinylDbContext _airVinylDbContext;

        public PeopleController(AirVinylDbContext airVinylDbContext)
        {
            _airVinylDbContext = airVinylDbContext
                ?? throw new ArgumentNullException(nameof(airVinylDbContext));
        }

        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            return Ok(await _airVinylDbContext.People.ToListAsync());
        }

        // People(1)
        // [EnableQuery]
        public IActionResult Get(int key)
        {
            //var person = _airVinylDbContext.People.Where(p => p.PersonId == key);
            var person = _airVinylDbContext.People.FirstOrDefault(p => p.PersonId == key);

            // if (!person.Any())
            // {
            //     return NotFound();
            // }

            if (person == null)
            {
                return NotFound();
            }

            //return Ok(SingleResult.Create(people));
            return Ok(person);
        }

        // [EnableQuery(MaxExpansionDepth = 3, MaxSkip = 10, MaxTop = 5, PageSize = 4)]
        // public IActionResult Get()
        // {
        //     return Ok(_airVinylDbContext.People);
        // }


        [HttpGet("odata/People({key})/Email")]
        [HttpGet("odata/People({key})/FirstName")]
        [HttpGet("odata/People({key})/LastName")]
        [HttpGet("odata/People({key})/DateOfBirth")]
        [HttpGet("odata/People({key})/Gender")]
        public async Task<IActionResult> GetPersonProperty(int key)
        {
            var person = await _airVinylDbContext.People
                .FirstOrDefaultAsync(p => p.PersonId == key);

            if (person == null)
            {
                return NotFound();
            }

            var propertyToGet = new Uri(HttpContext.Request.GetEncodedUrl()).Segments.Last();

            if (!person.HasProperty(propertyToGet))
            {
                return NotFound();
            }

            var propertyValue = person.GetValue(propertyToGet);

            if (propertyValue == null)
            {
                // null = no content
                return NoContent();
            }

            return Ok(propertyValue);
        }


        [HttpGet("odata/People({key})/Email/$value")]
        [HttpGet("odata/People({key})/FirstName/$value")]
        [HttpGet("odata/People({key})/LastName/$value")]
        [HttpGet("odata/People({key})/DateOfBirth/$value")]
        [HttpGet("odata/People({key})/Gender/$value")]
        public async Task<IActionResult> GetPersonPropertyRawValue(int key)
        {
            var person = await _airVinylDbContext.People
              .FirstOrDefaultAsync(p => p.PersonId == key);

            if (person == null)
            {
                return NotFound();
            }

            var url = HttpContext.Request.GetEncodedUrl();
            var propertyToGet = new Uri(url).Segments[^2].TrimEnd('/');

            if (!person.HasProperty(propertyToGet))
            {
                return NotFound();
            }

            var propertyValue = person.GetValue(propertyToGet);

            if (propertyValue == null)
            {
                // null = no content
                return NoContent();
            }

            return Ok(propertyValue.ToString());
        }

        // odata/People(key)/VinylRecords

        //[EnableQuery]
        [HttpGet("odata/People({key})/VinylRecords")]
        //[HttpGet("People({key})/Friends")]
        //[HttpGet("People({key})/Addresses")]
        public async Task<IActionResult> GetPersonCollectionProperty(int key)
        {
            var collectionPropertyToGet = new Uri(HttpContext.Request.GetEncodedUrl())
            .Segments.Last();

            var person = await _airVinylDbContext.People
                .Include(collectionPropertyToGet)
                .FirstOrDefaultAsync(p => p.PersonId == key);

            if (person == null)
            {
                return NotFound();
            }

            if (!person.HasProperty(collectionPropertyToGet))
            {
                return NotFound();
            }

            return Ok(person.GetValue(collectionPropertyToGet));
        }
        
//
//
//        [HttpGet("odata/People({key})/VinylRecords")]
//        [EnableQuery]
//        public IActionResult GetVinylRecordsForPerson(int key)
//        {
//            var person = _airVinylDbContext.People.FirstOrDefault(p => p.PersonId == key);
//            if (person == null)
//            {
//                return NotFound();
//            }
//
//            return Ok(_airVinylDbContext.VinylRecords
//                .Include("DynamicVinylRecordProperties")
//                .Where(v => v.Person.PersonId == key));
//        }
//
//        [HttpGet("odata/People({key})/VinylRecords({vinylRecordKey})")]
//        [EnableQuery]
//        public IActionResult GetVinylRecordForPerson(int key, int vinylRecordKey)
//        {
//            var person = _airVinylDbContext.People.FirstOrDefault(p => p.PersonId == key);
//            if (person == null)
//            {
//                return NotFound();
//            }
//
//            var vinylRecord = _airVinylDbContext.VinylRecords.Include("DynamicVinylRecordProperties")
//                .Where(v => v.Person.PersonId == key
//                && v.VinylRecordId == vinylRecordKey);
//
//            if (!vinylRecord.Any())
//            {
//                return NotFound();
//            }
//
//            return Ok(SingleResult.Create(vinylRecord));
//        }
//
       [HttpPost("odata/People")]
       public async Task<IActionResult> CreatePerson([FromBody] Person person)
       {
           if (!ModelState.IsValid)
           {
               return BadRequest(ModelState);
           }
           // add the person to the People collection
           _airVinylDbContext.People.Add(person);
           await _airVinylDbContext.SaveChangesAsync();
           // return the created person 
           return Created(person);
       }

        [HttpPut("odata/People({key})")]
        public async Task<IActionResult> UpdatePerson(int key, [FromBody] Person person)
        {
            // PUT is not the preferred way of updating an entity.
            // It's a full update, which means by definition that if we omit certain fields
            // in the request body, these fields should be put to their default values,
            // and that is rarely what you want.
            // PATCH is the preferred way


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var currentPerson = await _airVinylDbContext.People
              .FirstOrDefaultAsync(p => p.PersonId == key);

            if (currentPerson == null)
            {
                return NotFound();

                // Alternative: if the person isn't found: Upsert.
                // That's a combination of insert and update.
                // This principle means that rather than returning a 404 when the person doesn't exist yet,
                // the update action can also create that person,
                // and it should then return a 201 Created containing the newly created person.
                // This must only be used if the responsibility for creating the key isn't at 
                // server-level.  In our case, we're using auto-increment fields,
                // so this isn't allowed - code is for illustration purposes only!
                //if (currentPerson == null)
                //{
                //    // the key from the URI is the key we should use
                //    person.PersonId = key;
                //    _airVinylDbContext.People.Add(person);
                //    await _airVinylDbContext.SaveChangesAsync();
                //    return Created(person);
                //}

            }

            person.PersonId = currentPerson.PersonId;
            _airVinylDbContext.Entry(currentPerson).CurrentValues.SetValues(person);
            await _airVinylDbContext.SaveChangesAsync();

            return NoContent();
            // Or
            // return Ok(currentPerson);
        }

              
        // Services should support Patch as the preferred means of updating an entity. 
        // Through Patch, we ensure that only those values specified in the request body are updated. 
        // You can think of this as a change set. 
        // Thus, the content in the request payload is merged with the entity's 
        // current state applying the update only to those components specified in the request body. 
        // So the request is sent to the URI of the entity we want to update. 
        // So we're getting our people controller and we want to send the patch 
        // request to odata/People with the person's key between rounded brackets.
        /// <summary>
        ///  Patch is not currently working properly. So go with Put
        /// </summary>
        /// <param name="key"></param>
        /// <param name="delta"></param>
        /// <returns></returns>
        

        [HttpPatch("odata/People({key})")]
        public async Task<IActionResult> PartiallyUpdatePerson(int key,
            [FromBody] Delta<Person> delta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var currentPerson = await _airVinylDbContext.People
                           .FirstOrDefaultAsync(p => p.PersonId == key);

            if (currentPerson == null)
            {
                return NotFound();
            }

            delta.CopyChangedValues(currentPerson);

            //delta.Patch(currentPerson);

            await _airVinylDbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("odata/People({key})")]
        public async Task<IActionResult> DeleteOnePerson(int key)
        {
            var currentPerson = await _airVinylDbContext.People
                .FirstOrDefaultAsync(p => p.PersonId == key);

            if (currentPerson == null)
            {
                return NotFound();
            }

            _airVinylDbContext.People.Remove(currentPerson);
            await _airVinylDbContext.SaveChangesAsync();
            return NoContent();
        }
//
//        [HttpPost("odata/People({key})/VinylRecords")]
//        public async Task<IActionResult> CreateVinylRecordForPerson(int key,
//        [FromBody] VinylRecord vinylRecord)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }
//
//            // does the person exist?
//            var person = await _airVinylDbContext.People
//                .FirstOrDefaultAsync(p => p.PersonId == key);
//            if (person == null)
//            {
//                return NotFound();
//            }
//
//            // link the person to the VinylRecord (also avoids an invalid person 
//            // key on the passed-in record - key from the URI wins)
//            vinylRecord.Person = person;
//
//            // add the VinylRecord
//            _airVinylDbContext.VinylRecords.Add(vinylRecord);
//            await _airVinylDbContext.SaveChangesAsync();
//
//            // return the created VinylRecord 
//            return Created(vinylRecord);
//        }
//
//
//        [HttpPatch("odata/People({key})/VinylRecords({vinylRecordKey})")]
//        public async Task<IActionResult> PartiallyUpdateVinylRecordForPerson(int key,
//            int vinylRecordKey,
//            [FromBody] Delta<VinylRecord> patch)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }
//
//            // does the person exist?
//            var person = await _airVinylDbContext.People
//                .FirstOrDefaultAsync(p => p.PersonId == key);
//            if (person == null)
//            {
//                return NotFound();
//            }
//
//            // find a matching vinyl record  
//            var currentVinylRecord = await _airVinylDbContext.VinylRecords
//                .Include("DynamicVinylRecordProperties")
//                .FirstOrDefaultAsync(p => p.VinylRecordId == vinylRecordKey 
//                && p.Person.PersonId == key);
//
//            // return NotFound if the VinylRecord isn't found
//            if (currentVinylRecord == null)
//            {
//                return NotFound();
//            }
//
//            // apply patch
//            patch.Patch(currentVinylRecord);
//            await _airVinylDbContext.SaveChangesAsync();
//
//            return NoContent();
//        }
//
//
//        [HttpDelete("odata/People({key})/VinylRecords({vinylRecordKey})")]
//        public async Task<IActionResult> DeleteVinylRecordForPerson(int key,
//          int vinylRecordKey)
//        {
//            var currentPerson = await _airVinylDbContext.People
//                .FirstOrDefaultAsync(p => p.PersonId == key);
//            if (currentPerson == null)
//            {
//                return NotFound();
//            }
//
//            // find a matching vinyl record  
//            var currentVinylRecord = await _airVinylDbContext.VinylRecords
//                .FirstOrDefaultAsync(p => p.VinylRecordId == vinylRecordKey 
//                && p.Person.PersonId == key);
//
//            if (currentVinylRecord == null)
//            {
//                return NotFound();
//            }
//
//            _airVinylDbContext.VinylRecords.Remove(currentVinylRecord);
//            await _airVinylDbContext.SaveChangesAsync();
//
//            // return No Content
//            return NoContent();
//        }
    }
}