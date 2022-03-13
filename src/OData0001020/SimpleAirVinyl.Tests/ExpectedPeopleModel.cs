using AirVinyl.Entities;
using System;
using System.Collections.Generic;

namespace SimpleAirVinyl.Tests
{
    public class ExpectedPersonModel
    {
        public int PersonId { get; set; }

        public string Email { get; set; } = default!;

        public string FirstName { get; set; } = default!;

        public string LastName { get; set; } = default!;

        public DateTimeOffset DateOfBirth { get; set; }

        public string Gender { get; set; } = default!;

        public int NumberOfRecordsOnWishList { get; set; }

        public decimal AmountOfCashToSpend { get; set; }

        public ICollection<ExpectedVinylRecordModel> VinylRecords { get; set; } = new List<ExpectedVinylRecordModel>();
    }

    public class ExpectedVinylRecordCollectionModel
    {
        public ICollection<ExpectedVinylRecordModel> value { get; set; } = default!;
    }

    public class ExpectedPeopleModel
    {
        public ICollection<ExpectedPersonModel> value { get; set; } = default!;
    }

    public class ExpectedVinylRecordModel
    {
        public int VinylRecordId { get; set; }

        public string Title { get; set; } = default!;

        public string Artist { get; set; } = default!;

        public string CatalogNumber { get; set; } = default!;

        public int? Year { get; set; }

        public ExpectedPressingDetailModel PressingDetail { get; set; } = default!;

        public int PressingDetailId { get; set; }

        public virtual ExpectedPersonModel Person { get; set; } = default!;

        public int PersonId { get; set; }
    }

    public class ExpectedPressingDetailModel
    {
        public int PressingDetailId { get; set; }

        public int Grams { get; set; }

        public int Inches { get; set; }

        public string Description { get; set; } = default!;
    }

}
