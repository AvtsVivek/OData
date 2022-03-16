namespace OData0001010_Gadgets.Api.Tests
{
    public class ExpectedGadgetModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = default!;
        public string Brand { get; set; } = default!;
        public decimal Cost { get; set; } = default!;
        public string Type { get; set; } = default!;
    }
}
