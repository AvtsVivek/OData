using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace OData0001010_Gadgets.Api.Tests
{
    public class ExpectedODataGadgetModel
    {
        [JsonPropertyName("@odata.count")]
        public int ODataCount { get; set; } = default!;
        [JsonPropertyName("@odata.context")]
        public string ODataContext { get; set; } = default!;

        public List<ExpectedGadgetModel> value { get; set; } = default!;
    }
    public class ExpectedGadgetModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = default!;
        public string Brand { get; set; } = default!;
        public decimal Cost { get; set; } = default!;
        public string Type { get; set; } = default!;
    }
}
