using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAirVinyl.Entities;

public class SpecializedRecordStore : RecordStore
{
    public string Specialization { get; set; } = default!;
}
