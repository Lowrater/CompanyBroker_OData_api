using CompanyBroker_DBS;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Company_broker_OData_Api.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    [ODataRoutePrefix("Resources")]
    public class ResourcesController : ControllerBase
    {
        #region constructor
        private readonly CompanyBrokerEntities db;

        public ResourcesController(CompanyBrokerEntities context)
        {
            db = context;
        }
        #endregion

        #region get methods
        /// <summary>
        /// Fetches all resources
        /// </summary>
        /// <returns></returns>
        [EnableQuery]
        [ODataRoute]
        public async Task<ActionResult<IList<CompanyResource>>> GetResources()
        {
            //-- Uses the CompanyBrokeraccountEntity to access the database
            //-- Fetches the account list
            var resourceList = db.CompanyResources.AsQueryable();

            return await resourceList.ToListAsync();
        }
        #endregion
    }
}
