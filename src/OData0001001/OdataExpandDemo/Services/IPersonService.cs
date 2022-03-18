using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OdataExpandDemo.Models;

namespace OdataExpandDemo.Services
{
    public interface IPersonService
    {
        IQueryable<Person> RetrieveAllPeople();
        List<Person> GetPeopleWithAccounts();
        //List<Person> GetPersonWithAccounts();
    }
}
