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
    [ODataRoutePrefix("Accounts")]
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
        /// GET - odata/accounts
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
                return NotFound(false);
            }
        }


        ///// <summary>
        ///// Gets an account based on userid
        ///GET - odata/accounts(5)
        ///// </summary>
        ///// <param name="username"></param>
        ///// <returns></returns>
        [EnableQuery]
        [ODataRoute("({userid})")]
        public async Task<IActionResult> GetAccount([FromODataUri] int userid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //-- Uses the CompanyBrokeraccountEntity to access the database  
            //-- Fetches the account 
            var responseData = await db.CompanyAccounts.AsQueryable().FirstOrDefaultAsync(a => a.UserId == userid);

            //-- Returns the results
            if (responseData != null)
            {
                return Ok(new AccountResponse(responseData));
            }
            else
            {
                return NotFound(false);
            }
        }

        ///// <summary>
        ///// Gets an account based on userid
        ///GET - odata/accounts('mts')
        ///// </summary>
        ///// <param name="username"></param>
        ///// <returns></returns>
        //[EnableQuery]
        //[ODataRoute("({username})")]
        //public async Task<IActionResult> GetAccount([FromODataUri] string username)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    //-- Uses the CompanyBrokeraccountEntity to access the database  
        //    //-- Fetches the account 
        //    var responseData = await db.CompanyAccounts.AsQueryable().FirstOrDefaultAsync(a => a.Username == username);

        //    //-- Returns the results
        //    if (responseData != null)
        //    {
        //        return Ok(new AccountResponse(responseData));
        //    }
        //    else
        //    {
        //        return NotFound(false);
        //    }
        //}
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

                //-- Returns the user wished to be created
                return Ok(true);
            }
            else
            {
                //-- Returns the user wished to be created
                return NotFound(false);
            }
        }
        #endregion


        #region Put Methods
        /// <summary>
        /// Updates the user account informations
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs("PUT")]
        [EnableQuery]
        public async Task<IActionResult> UpdateAccount(AccountRequest AccountAPIModel)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
        
            //-- fetches the account
            var responsData = db.CompanyAccounts.Where(a => a.CompanyId == AccountAPIModel.CompanyId && a.Username == AccountAPIModel.Username).Single<CompanyAccount>();
            //-- checks the account
            if (responsData != null)
            {
                //-- sets the new informations
                responsData.Email = AccountAPIModel.Email;
                responsData.Active = AccountAPIModel.Active;
                responsData.Username = AccountAPIModel.Username;

                //-- checks if password has been changed
                if (!string.IsNullOrEmpty(AccountAPIModel.Password))
                {
                    //-- creates new salt
                    var newSalt = GenerateSalt(32);
                    //-- sets new password informations
                    responsData.PasswordHash = GetHash(AccountAPIModel.Password, newSalt);
                    responsData.PasswordSalt = newSalt;
                }
                //-- Sets the data entry and sate
                db.Entry(responsData).State = EntityState.Modified;
                //-- saves the data
                await db.SaveChangesAsync();
                //-- returns
                return Ok(true);
            }
            else
            {
            //-- returns
            return NotFound(false);
            }
            
        }
        #endregion

        #region Delete methods

        /// <summary>
        /// Deletes an account based on company id and username
        /// </summary>
        /// <param name="AccountRequest"></param>
        /// <returns></returns>
        [AcceptVerbs("DELETE")]
        [EnableQuery]
        public async Task<IActionResult> Delete(int companyId, string username)
        {      
            //-- fetches an account based on the informations
            var account = db.CompanyAccounts.Where(a => a.CompanyId == companyId && a.Username == username).Single<CompanyAccount>();
            //-- checks the account
            if (account != null)
            {
                //-- removes the account
                db.CompanyAccounts.Remove(account);
                //-- informs of the state
                db.Entry(account).State = EntityState.Deleted;
                //-- saves the state
                await db.SaveChangesAsync();
                //-- return
                return Ok(true);
            }
            else
            {
                return NotFound(false);
            }
        }

        #endregion
    }
}
