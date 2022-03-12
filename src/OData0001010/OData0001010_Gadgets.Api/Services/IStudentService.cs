using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OData0001010_Gadgets.Api.Models;

namespace OData0001010_Gadgets.Api.Services
{
    public interface IGadgetsService
    {
        IQueryable<Gadgets> RetrieveAllGadgets();
    }
}
