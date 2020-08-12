using Company_broker_OData_Api.Models;
using CompanyBroker_DBS;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Company_broker_OData_Api.Controllers
{
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
                return NotFound();
            }

        }
        #endregion
    }
}
