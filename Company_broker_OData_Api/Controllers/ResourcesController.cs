using CompanyBroker_DBS;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Company_broker_OData_Api.Controllers
{
    [ODataRoutePrefix("Resources")]
    public class ResourcesController : ODataController
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
        /// GET - GET - odata/resources
        /// </summary>
        /// <returns></returns>
        [EnableQuery]
        public async Task<IActionResult> GetResources()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //-- Uses the CompanyBrokeraccountEntity to access the database
            //-- Fetches the account list
            var resourceList = await db.CompanyResources.AsQueryable().ToListAsync();

            if (resourceList != null)
            {
                return Ok(resourceList);
            }
            else
            {
                return NotFound(false);
            }
        }

        /// <summary>
        /// Fetches all resources based by one CompanyId
        /// GET - odata/resources(5)
        /// </summary>
        /// <returns></returns>
        [EnableQuery]
        [ODataRoute("({resourceid})")]
        public async Task<IActionResult> GetResourcesByCompanyId([FromODataUri] int resourceid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //-- Uses the CompanyBrokeraccountEntity to access the database
            var responsdata = await db.CompanyResources.Where(c => c.ResourceId == resourceid).ToListAsync();

            if (responsdata != null)
            {
                return Ok(responsdata);
            }
            else
            {
                return NotFound(false);
            }
        }






        #endregion
    }
}
