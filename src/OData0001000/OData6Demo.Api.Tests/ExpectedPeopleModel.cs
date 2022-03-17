using System;

namespace OData6Demo.Api.Tests
{
    public class StudentModel
    {
        //public int Id { get; set; }
        //public string ProductName { get; set; } = default!;
        //public string Brand { get; set; } = default!;
        //public decimal Cost { get; set; } = default!;
        //public string Type { get; set; } = default!;

        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public int Score { get; set; }

    }
}
