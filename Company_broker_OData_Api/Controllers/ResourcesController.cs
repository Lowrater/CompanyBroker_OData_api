using CompanyBroker_DBS;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Company_broker_OData_Api.Controllers
{

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
                return NotFound();
            }
        }

        /// <summary>
        /// Fetches all resources based by one CompanyId
        /// </summary>
        /// <returns></returns>
        [EnableQuery]
        //[ODataRoute("({companyid})")]
        public async Task<IActionResult> GetResourcesByCompanyId([FromODataUri] int companyId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //-- Uses the CompanyBrokeraccountEntity to access the database
            var responsdata = await db.CompanyResources.Where(c => c.CompanyId == companyId).ToListAsync();

            if (responsdata != null)
            {
                return Ok(responsdata);
            }
            else
            {
                return NotFound();
            }
        }

        #endregion
    }
}
