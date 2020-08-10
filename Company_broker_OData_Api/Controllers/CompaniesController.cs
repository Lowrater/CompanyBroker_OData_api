﻿using Company_broker_OData_Api.Models;
using CompanyBroker_DBS;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Company_broker_OData_Api.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
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
        /// </summary>
        /// <returns></returns>
        [EnableQuery]
        [ODataRoute]
        public async Task<ActionResult<IList<CompanyResponse>>> GetCompanies()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //-- Uses the CompanyBrokerCompaniesEntities to connect to the database
            //-- Fetches all companies                
            var responsdata = await db.Companies.AsQueryable().Select(c => new CompanyResponse(c)).ToListAsync();

            if(responsdata != null)
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
