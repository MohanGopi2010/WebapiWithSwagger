using Microsoft.Web.Http.Routing;
using Microsoft.Web.Http.Versioning;
using System.Web.Http;
using System.Web.Http.Routing;

namespace WebAPIOauth
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes


            var constraintResolver = new DefaultInlineConstraintResolver()
            {
                ConstraintMap ={
                                    ["apiVersion"] = typeof( ApiVersionRouteConstraint )
                                }
            };
            //// Web API routes

            config.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                //  options.ApiVersionReader = new HeaderApiVersionReader("apiVersion");
                options.ApiVersionReader = new UrlSegmentApiVersionReader();

                // options.DefaultApiVersion = new Microsoft.Web.Http.ApiVersion(2,0);
                options.ErrorResponses = new VersionErrorResponseProvider();


                //options.Conventions.Controller<WritersControllerV2>()
                //    .HasApiVersion(new ApiVersion(2, 0)); /*Spicify the versioning in single place*/
            }
            );

            //config.AddVersionedApiExplorer(expOptions => { expOptions. }
            //);





            config.MapHttpAttributeRoutes(constraintResolver);

            
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new
                {
                    id = RouteParameter.Optional
                }
            );

        } 
    }

    public class VersionErrorResponseProvider : DefaultErrorResponseProvider
    {
        // note: in Web API the response type is HttpResponseMessage
        public override System.Net.Http.HttpResponseMessage CreateResponse(ErrorResponseContext context)
        {
            switch (context.ErrorCode)
            {
                case "UnsupportedApiVersion":
                    context = new ErrorResponseContext(
                        context.Request,
                        context.StatusCode,
                        context.ErrorCode,
                        "My custom error message.",

                        context.MessageDetail);
                    break;
                default:
                    context = new ErrorResponseContext(
                        context.Request,
                        context.StatusCode,
                        context.ErrorCode,
                        "My custom error message.  default",

                        context.MessageDetail);
                    break;
            }

            return base.CreateResponse(context);
        }
    }
}
