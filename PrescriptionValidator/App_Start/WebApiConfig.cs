using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.OData.Builder;
using PrescriptionValidator.Models;

namespace PrescriptionValidator
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );


            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Producer>("Producer");
            builder.EntitySet<Product>("Product");
            builder.EntitySet<Variant>("Variant");
            builder.EntitySet<Composition>("Composition");
            builder.EntitySet<Substance>("Substance");
            builder.EntitySet<Dosage>("Dosage");
            builder.EntitySet<Contraindication>("Contraindication");
            config.Routes.MapODataRoute("odata", "odata", builder.GetEdmModel());
        }
    }
}
