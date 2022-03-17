using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OData6Demo.Api.Models;

namespace OData6Demo.Api.Services
{
    public class StudentService : IStudentService
    {
        public IQueryable<Student> RetrieveAllStudents()
        {
            return new List<Student>
            {
                new Student
                {
                    Id = Guid.NewGuid(),
                    Name = "Vishu Goli",
                    Score = 200
                },
                new Student
                {
                    Id = Guid.NewGuid(),
                    Name = "Kailu Hu",
                    Score = 160
                },
                new Student
                {
                    Id = Guid.NewGuid(),
                    Name = "Sean Hobbs",
                    Score = 170
                },
                new Student
                {
                    Id = Guid.NewGuid(),
                    Name = "Vivek Koppula",
                    Score = 150
                },
                new Student
                {
                    Id = Guid.NewGuid(),
                    Name = "Aarogya",
                    Score = 120
                },
                new Student
                {
                    Id = Guid.NewGuid(),
                    Name = "Aakash",
                    Score = 220
                }
            }.AsQueryable();
        }
    }
}
