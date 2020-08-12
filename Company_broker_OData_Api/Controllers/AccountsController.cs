using Company_broker_OData_Api.Models;
using CompanyBroker_DBS;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Company_broker_OData_Api.Controllers
{
    public class AccountsController : ODataController
    {
        #region constructor and DBS data
        //-- database context 
        private readonly CompanyBrokerEntities db;

        public AccountsController(CompanyBrokerEntities context)
        {
            db = context;
        }

        #endregion

        #region Password generators
        private readonly Random random = new Random();

        private byte[] GetHash(string s, byte[] salt)
        {
            using (var ha = HashAlgorithm.Create("SHA256"))
                return ha.ComputeHash(salt.Concat(Encoding.UTF8.GetBytes(s)).ToArray());
        }

        private byte[] GenerateSalt(int size)
        {
            var salt = new byte[size];
            random.NextBytes(salt);
            return salt;
        }
        #endregion


        #region Get Methods

        /// <summary>
        /// Fetches all accounts, through a model to not contain sensitive data like passwords.
        /// </summary>
        /// <returns></returns>
        [EnableQuery]
        public async Task<IActionResult> GetAccounts()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //-- Uses the CompanyBrokeraccountEntity to access the database
            //-- Filtered by AccountResponse for sensitive data
            var responseList = await db.CompanyAccounts.AsQueryable().Select(a => new AccountResponse(a)).ToListAsync();

            if (responseList != null)
            {
                return Ok(responseList);
            }
            else
            {
                return NotFound();
            }
        }


        ///// <summary>
        ///// Gets an account based on username
        ///// </summary>
        ///// <param name="username"></param>
        ///// <returns></returns>
        [EnableQuery]
        //[ODataRoute("({username})")]
        public async Task<IActionResult> GetAccount([FromODataUri] string username)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //-- Uses the CompanyBrokeraccountEntity to access the database  
            //-- Fetches the account list
            var responseData = await db.CompanyAccounts.AsQueryable().FirstOrDefaultAsync(a => a.Username == username);

            //-- Returns the results
            if (responseData != null)
            {
                return Ok(new AccountResponse(responseData));
            }
            else
            {
                return NotFound();
            }
        }
        #endregion



        #region Post methods
        /// <summary>
        /// Creates an account from the content recieved from the user / application, to an existing company
        /// </summary>
        /// <param name="accountRequest"></param>
        /// <returns></returns>
        [AcceptVerbs("POST")]
        [EnableQuery]
        public async Task<IActionResult> CreateAccount([FromBody] AccountRequest accountRequest)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool resultProcess = false;
            //-- generating the salt
            var salt = GenerateSalt(32);

            //-- Verify account data
            if (accountRequest != null)
            {
                //-- Creates the new account
                var user = new CompanyAccount
                {
                    CompanyId = accountRequest.CompanyId,
                    Username = accountRequest.Username,
                    Email = accountRequest.Email,
                    PasswordSalt = salt,
                    PasswordHash = GetHash(accountRequest.Password, salt),
                    Active = accountRequest.Active
                };

                //-- adds a new user to the CompanyAccounts table
                db.CompanyAccounts.Add(user);
                //-- Saves the changes to the database
                await db.SaveChangesAsync();

                resultProcess = true;
            }

            //-- Returns the user wished to be created
            return Ok(resultProcess);
        }
        #endregion


        #region Put Methods

        #endregion

    }
}
