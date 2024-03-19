using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Linq;

namespace Logisitcs.Dynamics365.Clone.Oportunidades
{
    public class CloneOportunidades : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            EntityReference opportunity = (EntityReference)context.InputParameters["Target"];
            var id = opportunity.Id.ToString();

            QueryExpression opportunityQuery = new QueryExpression("opportunity");
            opportunityQuery.ColumnSet.AddColumns(
                "name",
                "msdyn_forecastcategory",
                "pricelevelid"
                );
            opportunityQuery.Criteria.AddCondition("opportunityid", ConditionOperator.Equal, id);
            EntityCollection filterOpportunity = service.RetrieveMultiple(opportunityQuery);

            Entity clonedOpportunity = new Entity("opportunity");
            clonedOpportunity.Id = Guid.NewGuid();
            clonedOpportunity["name"] = filterOpportunity.Entities.First()["name"] + "(Cópia)";
            clonedOpportunity["msdyn_forecastcategory"] = filterOpportunity.Entities.First()["msdyn_forecastcategory"];
            if (filterOpportunity.Entities.First().Contains("pricelevelid"))
            {
                clonedOpportunity["pricelevelid"] = filterOpportunity.Entities.First()["pricelevelid"];
            }
            var clonedOpportunityId = service.Create(clonedOpportunity);



            QueryExpression productQuery = new QueryExpression("opportunityproduct");
            productQuery.ColumnSet.AddColumns(
                "productid",
                "opportunityid",
                "ispriceoverridden",
                "uomid",
                "quantity",
                "manualdiscountamount",
                "tax",
                "parentbundleid"

                );
            productQuery.Criteria.AddCondition("opportunityid", ConditionOperator.Equal, id);
            EntityCollection filterProducts = service.RetrieveMultiple(productQuery);

            if (filterProducts.Entities.Count() > 0)
            {
                for (var i = 0; i < filterProducts.Entities.Count(); i++)
                {
                    Entity clonedProduct = new Entity("opportunityproduct");
                    clonedProduct.Id = Guid.NewGuid();
                    clonedProduct["productid"] = filterProducts.Entities[i]["productid"];
                    clonedProduct["opportunityid"] = new EntityReference("opportunity", clonedOpportunityId);
                    clonedProduct["ispriceoverridden"] = filterProducts.Entities[i]["ispriceoverridden"];
                    clonedProduct["uomid"] = filterProducts.Entities[i]["uomid"];
                    clonedProduct["quantity"] = filterProducts.Entities[i]["quantity"];
                    service.Create(clonedProduct);

                }
            }


            context.OutputParameters["newid"] = clonedOpportunityId;
        }
    }
}
