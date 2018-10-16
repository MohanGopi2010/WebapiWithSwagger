using System.Web.Http;
using WebActivatorEx;
using WebAPIOauth;
using Swashbuckle.Application;
using System.Web.Http.Description;
using System;
using Swashbuckle.Swagger;
using System.Linq;
using System.Collections.Generic;
using System.Web.Http.Routing.Constraints;
using System.Web.Http.Filters;
using System.Net.Http;

//[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace WebAPIOauth
{
    public class SwaggerConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //var thisAssembly = typeof(SwaggerConfig).Assembly;

            var apiExplorer = config.AddVersionedApiExplorer(
                options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                    // can also be used to control the format of the API version in route templates
                    options.SubstituteApiVersionInUrl = true;
                });


            config

                .EnableSwagger("{apiVersion}/swagger", c =>
                  {
                      c.RootUrl(req =>
         req.RequestUri.GetLeftPart(UriPartial.Authority) +
         req.GetRequestContext().VirtualPathRoot.TrimEnd('/'));

                      //c.SingleApiVersion("v1", "Web Api With Swagger");
                      c.PrettyPrint();

                      c.MultipleApiVersions(
                                  (apiDescription, version) => apiDescription.GetGroupName() == version,
                                  info =>
                                  {
                                      foreach (var group in apiExplorer.ApiDescriptions)
                                      {
                                          var description = "A sample application with Swagger, Swashbuckle, and API versioning.";

                                          if (group.IsDeprecated)
                                          {
                                              description += " This API version has been deprecated.";
                                          }
                                          info.Version(group.Name, $"Sample API {group.ApiVersion}")
                                              .Contact(_contact => _contact.Name("Harman").Email("Harman@.com"))
                                              .Description(description)
                                              .TermsOfService("Terms of condition");
                                      }
                                  });


                      c.OAuth2("oauth2")
                      .Description("OAuth2 Password Grant")
                          .Flow("password")
                       .TokenUrl("http://localhost:65371/token");

                      c.IncludeXmlComments(GetXmlCommentsPath());

                      c.DescribeAllEnumsAsStrings();

                      c.OperationFilter<AssignOAuth2SecurityRequirements>();
                    //  c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                     

                  })
                .EnableSwaggerUi(c =>
                    {
                        c.DocumentTitle("My Swagger UI");
                      //  c.DocExpansion(DocExpansion.Full);

                        //  c.InjectJavaScript(thisAssembly, "WebAPIOauth.swaggerext.onComplete.js");

                        //c.EnableOAuth2Support(
                        //    clientId: "client_id",
                        //    clientSecret: null,
                        //    realm: "test-realm",
                        //    appName: "Swagger UI"
                        ////additionalQueryStringParams: new Dictionary<string, string>() { { "foo", "bar" } }
                        //);
                        c.EnableDiscoveryUrlSelector();

                    });
        }




        private static string GetXmlCommentsPath()
        {
            return string.Format(@"{0}\bin\WebAPIOauth.xml", AppDomain.CurrentDomain.BaseDirectory);
        }
    }

    internal class AssignOAuth2SecurityRequirements : IOperationFilter
    {

        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            //Adding Versioning 
            if (operation != null)
            {

                if (operation.parameters != null)

                {
                    foreach (var parameter in operation.parameters)
                    {
                        var description = apiDescription.ParameterDescriptions
                                                        .First(p => p.Name == parameter.name);

                        if (parameter.description == null)
                        {
                            parameter.description = description.Documentation;
                        } 
                        if (parameter.@default == null)
                        {
                            parameter.@default = description.ParameterDescriptor?.DefaultValue;
                        } 

                    }


                }
            }




            // Determine if the operation has the Authorize attribute
            var authorizeAttributes = apiDescription.ActionDescriptor.GetCustomAttributes<AuthorizeAttribute>();

            //if (authorizeAttributes.Any())
            //{

            // Initialize the operation.security property
            if (operation.security == null)
                operation.security = new List<IDictionary<string, IEnumerable<string>>>();

            // Add the appropriate security definition to the operation
            var oAuthRequirements = new Dictionary<string, IEnumerable<string>>
            {
                { "oauth2", Enumerable.Empty<string>() }
            };

            operation.security.Add(oAuthRequirements);
            // }


        }
    }
}
