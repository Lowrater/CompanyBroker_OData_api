using Company_broker_OData_Api.Models;
using CompanyBroker_DBS;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Company_broker_OData_Api.Controllers
{
    [ODataRoutePrefix("Companies")]
    public class CompaniesController : ODataController
    {

        #region constructor
        //-- database context 
        private readonly CompanyBrokerEntities db;

        public CompaniesController(CompanyBrokerEntities context)
        {
            db = context;
        }
        #endregion

        #region get methods
        /// <summary>
        /// Returns all companies from the database in a list, through a company model without the balance
        /// GET odata/companies
        /// </summary>
        /// <returns></returns>
        [EnableQuery]
        public async Task<IActionResult> GetCompanies()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //-- Uses the CompanyBrokerCompaniesEntities to connect to the database
            //-- Fetches all companies                
            var responsdata = await db.Companies.AsQueryable().Select(c => new CompanyResponse(c)).ToListAsync();

            if (responsdata != null)
            {
                return Ok(responsdata);
            }
            else
            {
                return NotFound(false);
            }
        }

        /// <summary>
        /// Returns an company based on a companyid number
        /// GET odata/companies(3)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [EnableQuery]
        [ODataRoute("({companyid})")]
        public async Task<IActionResult> GetCompany([FromODataUri] int companyid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //-- Uses the CompanyBrokerCompaniesEntities to connect to the database
            //-- Fetches all companies                
            var responsdata = await db.Companies.AsQueryable().FirstOrDefaultAsync(c => c.CompanyId == companyid);

            if (responsdata != null)
            {
                return Ok(new CompanyResponse(responsdata));
            }
            else
            {
                return NotFound(false);
            }
        }





        #endregion
    }
}
