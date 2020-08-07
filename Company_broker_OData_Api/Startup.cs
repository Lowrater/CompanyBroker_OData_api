using CompanyBroker_DBS;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OData.Edm;
using System.Linq;

namespace Company_broker_OData_Api
{
    // -- Guide: https://devblogs.microsoft.com/odata/experimenting-with-odata-in-asp-net-core-3-1/ 
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
            //services.AddControllers();
            //-- Own data
            //services.AddDbContext<CompanyBrokerEntities>(options => options.UseSqlServer(Configuration.GetConnectionString("Company_broker_OData_ApiContext")));

            //--- Adding odata to asp.net core's dependency injection system
            services.AddOData();
            services.AddODataQueryFilter();
            //--- ODATA CONTENT ROUTE disabling - Odata does not support end point routing
            services.AddControllers(mvcOptions => mvcOptions.EnableEndpointRouting = false);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            //--- ODATA CONTENT ROUTE for each controller with 'odata' infront of it
            app.UseMvc(routebuilder =>
            {
                //-- enables all OData query options, for example $filter, $orderby, $expand, etc.
                routebuilder.Select().Expand().Filter().OrderBy().MaxTop(100).Count().SkipToken();
                //- The route etc. localhost:50359/odata/Accounts
                //- sets the route name, prefix and Odata data model
                routebuilder.MapODataServiceRoute("odata", "odata", GetEdmModel());
            });
        }


        ////--- ODATA CONTENT - Create the OData IEdmModel as required:
        private IEdmModel GetEdmModel()
        {
            //-- Creates the builder 
            var odataBuilder = new ODataConventionModelBuilder();
            //-- Adds all the database models - HaskKey = pointing at primary key of table
            odataBuilder.EntitySet<CompanyAccount>("Accounts").EntityType.HasKey(p => p.UserId);
            odataBuilder.EntitySet<Company>("Companies").EntityType.HasKey(p => p.CompanyId);
            odataBuilder.EntitySet<CompanyResource>("Resources").EntityType.HasKey(p => p.ResourceId);
            odataBuilder.EntitySet<ResourceDescription>("Resource Descriptions").EntityType.HasKey(p => p.DescriptionId);
            //-- returns the IEdmModel
            return odataBuilder.GetEdmModel();
        }
    }
}
