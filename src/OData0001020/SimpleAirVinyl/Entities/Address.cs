using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleAirVinyl.Entities;

// Address is an owned type (no key) - used to be called complex type in EF.
[Owned]
public class Address
{
    [StringLength(200)]
    public string Street { get; set; } = default!;

    [StringLength(100)]
    public string City { get; set; } = default!;

    [StringLength(10)]
    public string PostalCode { get; set; } = default!;

    [StringLength(100)]
    public string Country { get; set; } = default!;

    public int RecordStoreId { get; set; }
}
