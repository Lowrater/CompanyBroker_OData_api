using CompanyBroker_DBS;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Company_broker_OData_Api.Controllers
{
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
                return NotFound();
            }

        }
        #endregion
    }
}
