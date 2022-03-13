using System.ComponentModel.DataAnnotations;

namespace SimpleAirVinyl.Entities;

public class VinylRecord
{
    [Key]
    public int VinylRecordId { get; set; }

    [StringLength(150)]
    [Required]
    public string Title { get; set; } = default!;

    [StringLength(150)]
    [Required]
    public string Artist { get; set; } = default!;

    [StringLength(50)]
    public string CatalogNumber { get; set; } = default!;

    public int? Year { get; set; }

    public PressingDetail PressingDetail { get; set; } = default!;

    public int PressingDetailId { get; set; }

    public virtual Person Person { get; set; } = default!;

    public int PersonId { get; set; }
}
