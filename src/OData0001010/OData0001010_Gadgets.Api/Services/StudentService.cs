using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OData0001010_Gadgets.Api;
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

        public IQueryable<Gadgets> RetrieveAllGadgets()
        {
            return _odataDbContext.Gadgets.AsQueryable();
        }
    }
}
