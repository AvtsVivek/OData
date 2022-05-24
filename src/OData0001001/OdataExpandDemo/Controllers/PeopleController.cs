using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using OdataExpandDemo.Models;
using OdataExpandDemo.Services;

namespace OdataExpandDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PeopleController : ControllerBase
    {
        private readonly IPersonService studentService;

        public PeopleController(IPersonService studentService) =>
            this.studentService = studentService;

        [HttpGet]
        [EnableQuery]
        public ActionResult<IQueryable<Person>> GetAllPeople()
        {
            var retrievedStudents = studentService.RetrieveAllPeople();

            return Ok(retrievedStudents);
        }

		[HttpGet("person")]
		[EnableQuery]
		public ActionResult GetPerson()
		{
			var retrievedStudents = studentService.GetPeopleWithAccounts();

			return Ok(retrievedStudents);
		}

        [HttpGet("personquery")]
        [EnableQuery]
        public ActionResult<IQueryable<Person>> GetPersonQuery()
        {
            var retrievedStudents = studentService.GetPeopleWithAccountsQuery();

            return Ok(retrievedStudents);
        }
    }
}
