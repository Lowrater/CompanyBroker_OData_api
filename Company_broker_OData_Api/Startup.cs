using Company_broker_OData_Api.Models;
using CompanyBroker_DBS;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OData.Edm;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Company_broker_OData_Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            //-- Own data - This is entity framework core without
            services.AddDbContext<CompanyBrokerEntities>(options => options.UseSqlServer(Configuration.GetConnectionString("CompanyBrokerEntities")));
            //--- Adding odata to asp.net core's dependency injection system
            services.AddOData();
            //--- ODATA CONTENT ROUTE disabling - Odata does not support end point routing
            services.AddMvc(mvcOptions => mvcOptions.EnableEndpointRouting = false);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //--- ODATA CONTENT ROUTE for each controller with 'odata' infront of it
            app.UseMvc(routebuilder =>
            {
                // EnableDependencyInjection is required if we want to have OData routes and custom routes together in a controller
                routebuilder.EnableDependencyInjection();
                //- The route etc. localhost:50359/odata/Accounts
                //- sets the route name, prefix and Odata data model
                routebuilder.MapODataServiceRoute("ODataRoute", "odata/v4", GetEdmModel());
                //-- enables all OData query options, for example $filter, $orderby, $expand, etc.
                routebuilder.Select().Expand().Filter().OrderBy().MaxTop(100).Count();
            });
        }


        ////--- ODATA CONTENT - Create the OData IEdmModel as required:
        private IEdmModel GetEdmModel()
        {
            //-- Creates the builder 
            var odataBuilder = new ODataConventionModelBuilder();
            //-- Eks odataBuilder.EntitySet<ResourceDescription>("Resource Descriptions").EntityType.HasKey(p => p.DescriptionId);
            //-- Use Annotations with [Key] field on the primary key fields in the model instead of above one liner
            odataBuilder.EntitySet<CompanyAccount>("Accounts");
            odataBuilder.EntitySet<Company>("Companies");
            odataBuilder.EntitySet<CompanyResource>("Resources");
            odataBuilder.EntitySet<ResourceDescription>("Descriptions");

            //var getAccountF = odataBuilder.EntityType<CompanyAccount>().Function("GetAccount");
            //    getAccountF.Returns<AccountResponse>();
            //    getAccountF.Parameter<string>("username");

            //var GetResourcesByIdF = odataBuilder.EntityType<CompanyResource>().Function("GetResourcesByCompanyId");
            //     GetResourcesByIdF.ReturnsCollection<IList<CompanyResource>>();
            //     GetResourcesByIdF.Parameter<int>("companyId");

            //-- returns the IEdmModel
            return odataBuilder.GetEdmModel();
        }
    }
}
