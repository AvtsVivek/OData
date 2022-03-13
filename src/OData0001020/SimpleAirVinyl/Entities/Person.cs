using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SimpleAirVinyl.Entities;

public class Person
{
    [Key]
    public int PersonId { get; set; }

    [StringLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string FirstName { get; set; } = default!;

    [Required]
    [StringLength(50)]
    public string LastName { get; set; } = default!;

    [Required]
    [DataType(DataType.Date)]
    public DateTimeOffset DateOfBirth { get; set; }

    [Required]
    public Gender Gender { get; set; }

    public int NumberOfRecordsOnWishList { get; set; }

    public decimal AmountOfCashToSpend { get; set; }

    public ICollection<VinylRecord> VinylRecords { get; set; } = new List<VinylRecord>();
}
