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
    //[ODataRoutePrefix("Descriptions")]
    public class ResourceDescriptionController : ODataController
    {
        #region constructor
        //-- database context 
        private readonly CompanyBrokerEntities db;
        public ResourceDescriptionController(CompanyBrokerEntities context)
        {
            db = context;
        }
        #endregion

        #region Get methods
        /// <summary>
        /// Fetches the resource description based on the Resource ID
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        [EnableQuery]
        //[ODataRoute]
        public async Task<IActionResult> GetDescriptions()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var descriptionList = await db.ResourceDescriptions.AsQueryable().ToListAsync();
            
            if(descriptionList != null)
            {
                return Ok(descriptionList);
            }
            else
            {
                return NotFound();
            }

        }
        #endregion
    }
}
