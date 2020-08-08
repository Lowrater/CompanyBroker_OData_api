using Company_broker_OData_Api.Models;
using CompanyBroker_DBS;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
//- Guides: https://weblogs.asp.net/ricardoperes/asp-net-core-odata-part-1

namespace Company_broker_OData_Api.Controllers
{
    //[Route("odata/[controller]")]
    //[ApiController]
    [ODataRoutePrefix("Accounts")]
    public class AccountController : ODataController
    {
        #region Database context
        //-- database context 
        private readonly CompanyBrokerEntities db;

        public AccountController(CompanyBrokerEntities context)
        {
            db = context;
        }

        #endregion

        //[HttpGet]
        [EnableQuery(PageSize = 50)]
        [ODataRoute]
        public ActionResult<IList<CompanyAccount>> GetAccounts()
        {
            //-- Uses the CompanyBrokeraccountEntity to access the database
            //-- Filtered by AccountResponse for sensitive data
            //var fetchedData = db.CompanyAccounts.AsQueryable();

            return Ok(db.CompanyAccounts.ToList().AsQueryable());
        }

        #region Get Methods
        /// <summary>
        /// Fetches all accounts, through a model to not contain sensitive data like passwords.
        /// </summary>
        /// <returns></returns>
        //[HttpGet]
        ////[EnableQuery(PageSize = 50)]
        //[EnableQuery]
        //public async Task<ActionResult<IList<AccountResponse>>> GetAccounts()
        //{
        //    //-- Uses the CompanyBrokeraccountEntity to access the database
        //    //-- Filtered by AccountResponse for sensitive data
        //    return Ok((await db.CompanyAccounts.ToListAsync()).Select(a => new AccountResponse(a)).ToList());
        //}

        ///// <summary>
        ///// Fetches all accounts, through a model to not contain sensitive data like passwords.
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet]
        ////[EnableQuery(PageSize = 50)]
        //[EnableQuery]
        //public IEnumerable<string> Get()
        //{
        //    return new List<string>
        //    {
        //        "A",
        //        "B",
        //        "C"
        //    };


        //}


        ///// <summary>
        ///// Fetches all accounts, through a model to not contain sensitive data like passwords.
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet]
        //[EnableQuery(PageSize = 50)]
        ////[ODataRoute("Accounts")]
        //public async Task<IList<AccountResponse>> GetAccountsAsync()
        //{
        //    //-- Uses the CompanyBrokeraccountEntity to access the database
        //    using (db)
        //    {
        //        return (await db.CompanyAccounts.ToListAsync()).Select(a => new AccountResponse(a)).ToList();
        //    }
        //}




        ///// <summary>
        ///// Gets an account based on username
        ///// </summary>
        ///// <param name="username"></param>
        ///// <returns></returns>
        //[HttpGet]
        //[EnableQuery]
        //public async Task<AccountResponse> GetAccountAsync([FromODataUri] string username)
        //{
        //    //-- Uses the CompanyBrokeraccountEntity to access the database
        //    using (db)
        //    {
        //        //-- Fetches the account list
        //        var responseData = await db.CompanyAccounts.FirstOrDefaultAsync(a => a.Username == username);

        //        //-- Returns the results
        //        if (responseData != null)
        //        {
        //            return new AccountResponse(responseData);
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //}


        #endregion
    }
}
