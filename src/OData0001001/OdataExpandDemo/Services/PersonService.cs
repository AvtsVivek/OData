using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OdataExpandDemo.Models;

namespace OdataExpandDemo.Services
{
    public class PersonService : IPersonService
    {
        public List<Person> GetPeopleWithAccounts()
        {
            var person = new Person
            {
                Name = "Naveen",
                Id = 1,
                BankAccounts = new List<BankAccount>
                {
                    new BankAccount
                    {
                        Id = 1111,
                        BankName = "Bank 1"
                    },
                    new BankAccount
                    {
                        Id = 2222,
                        BankName = "Bank 2"
                    }
                }
            };
            var result = new List<Person>();
            result.Add(person);
            return result;
        }

        public IQueryable<Person> GetPeopleWithAccountsQuery()
        {
            var naveen = new Person
            {
                Name = "Naveen",
                Id = 1,
                BankAccounts = new List<BankAccount>
                {
                    new BankAccount
                    {
                        Id = 1111,
                        BankName = "Bank 1"
                    },
                    new BankAccount
                    {
                        Id = 2222,
                        BankName = "Bank 2"
                    }
                }
            };

            var bhuvana = new Person
            {
                Name = "Bhuvana",
                Id = 2,
                BankAccounts = new List<BankAccount>
                {
                    new BankAccount
                    {
                        Id = 1111,
                        BankName = "Bank 1"
                    },
                    new BankAccount
                    {
                        Id = 3333,
                        BankName = "Bank 3"
                    }
                }
            };


            var result = new List<Person>();
            result.Add(naveen);
            result.Add(bhuvana);
            return result.AsQueryable();
        }

        public IQueryable<Person> RetrieveAllPeople()
        {
            return new List<Person>
            {
                new Person
                {
                    Id = 1,
                    Name = "Vishu Goli",
                    Score = 200
                },
                new Person
                {
                    Id = 2,
                    Name = "Kailu Hu",
                    Score = 160
                },
                new Person
                {
                    Id = 3,
                    Name = "Sean Hobbs",
                    Score = 170
                },
                new Person
                {
                    Id = 4,
                    Name = "Vivek Koppula",
                    Score = 150
                },
                new Person
                {
                    Id = 5,
                    Name = "Aarogya",
                    Score = 120
                },
                new Person
                {
                    Id = 6,
                    Name = "Aakash",
                    Score = 220
                }
            }.AsQueryable();
        }
    }
}
