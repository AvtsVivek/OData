using System.Linq;
using OData0001010_Gadgets.Api.Models;

namespace OData0001010_Gadgets.Api.Services
{
    public class GadgetsService : IGadgetsService
    {
        private readonly OdataDbContext _odataDbContext;

        public GadgetsService(OdataDbContext odataDbContext)
        {
            _odataDbContext = odataDbContext;
        }

        public IQueryable<Product> RetrieveAllGadgets()
        {
            return _odataDbContext.Gadgets.AsQueryable();
        }
    }
}
