using CompanyBroker_DBS;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Company_broker_OData_Api.Controllers
{
    [ODataRoutePrefix("Descriptions")]
    public class DescriptionsController : ODataController
    {
        #region constructor
        //-- database context 
        private readonly CompanyBrokerEntities db;
        public DescriptionsController(CompanyBrokerEntities context)
        {
            db = context;
        }
        #endregion

        #region Get methods
        /// <summary>
        /// Fetches the resource description based on the Resource ID
        /// GET - odata/descriptions
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        [EnableQuery]
        public async Task<IActionResult> GetDescriptions()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var descriptionList = await db.ResourceDescriptions.AsQueryable().ToListAsync();

            if (descriptionList != null)
            {
                return Ok(descriptionList);
            }
            else
            {
                return NotFound(false);
            }
        }

        /// <summary>
        /// Fetches an description based on descriptionId
        /// GET - odata/descriptions(4)
        /// </summary>
        /// <param name="descriptionId"></param>
        /// <returns></returns>
        [EnableQuery]
        [ODataRoute("({descriptionId})")]
        public async Task<IActionResult> GetDescription([FromODataUri] int descriptionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var description = await db.ResourceDescriptions.AsQueryable().FirstOrDefaultAsync(d => d.DescriptionId == descriptionId);

            if (description != null)
            {
                return Ok(description);
            }
            else
            {
                return NotFound(false);
            }
        }
        #endregion
    }
}
