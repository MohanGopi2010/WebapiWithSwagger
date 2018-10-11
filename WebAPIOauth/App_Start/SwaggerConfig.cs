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

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace WebAPIOauth
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                    {
                        c.SingleApiVersion("v1", "Web Api With Swagger");
                        c.PrettyPrint();

                        //c.MultipleApiVersions(
                        //    (apiDesc, targetApiVersion) => ResolveVersionSupportByRouteConstraint(apiDesc, targetApiVersion),
                        //    (vc) =>
                        //    {
                        //        vc.Version("v1", "Swashbuckle Dummy API V1");
                        //        vc.Version("v2", "Swashbuckle Dummy API V2");
                        //    });

                        c.OAuth2("oauth2")
                        .Description("OAuth2 Password Grant")
                            .Flow("password")
                         .TokenUrl("http://localhost:65371/token");

                        c.IncludeXmlComments(GetXmlCommentsPath());

                        c.DescribeAllEnumsAsStrings();


                        c.OperationFilter<AssignOAuth2SecurityRequirements>();

                        //c.DocumentFilter<AuthTokenOperation>();

                    })
                .EnableSwaggerUi(c =>
                    {
                        // Use the "DocumentTitle" option to change the Document title.
                        // Very helpful when you have multiple Swagger pages open, to tell them apart.
                        //
                        c.DocumentTitle("My Swagger UI");

                        c.InjectJavaScript(thisAssembly, "WebAPIOauth.swaggerext.onComplete.js");

                        c.EnableOAuth2Support(
                            clientId: "client_id",
                            clientSecret: null,
                            realm: "test-realm",
                            appName: "Swagger UI"
                        //additionalQueryStringParams: new Dictionary<string, string>() { { "foo", "bar" } }
                        );
                        c.EnableDiscoveryUrlSelector();

                    });
        }



        public static bool ResolveVersionSupportByRouteConstraint(ApiDescription apiDesc, string targetApiVersion)
        {
            var versionConstraint = (apiDesc.Route.Constraints.ContainsKey("apiVersion"))
                ? apiDesc.Route.Constraints["apiVersion"] as RegexRouteConstraint
                : null;

            return (versionConstraint == null)
                ? false
                : versionConstraint.Pattern.Split('|').Contains(targetApiVersion);
        }

        private static string GetXmlCommentsPath()
        {
            return string.Format(@"{0}\bin\WebAPIOauth.xml", AppDomain.CurrentDomain.BaseDirectory);
        }
    }

    public class AuthTokenOperation : IDocumentFilter
    {
        /// <summary>
        /// Apply custom operation.
        /// </summary>
        /// <param name="swaggerDoc">The swagger document.</param>
        /// <param name="schemaRegistry">The schema registry.</param>
        /// <param name="apiExplorer">The api explorer.</param>
        public void Apply(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, IApiExplorer apiExplorer)
        {
            swaggerDoc.paths.Add("/token", new PathItem
            {
                post = new Operation
                {
                    tags = new List<string> { "Auth" },
                    consumes = new List<string>
                    {
                        "application/x-www-form-urlencoded"
                    },
                    parameters = new List<Parameter>
                    {
                        new Parameter
                        {
                            type = "string",
                            name = "grant_type",
                            required = true,
                            @in = "formData"
                        },
                        new Parameter
                        {
                            type = "string",
                            name = "username",
                            required = false,
                            @in = "formData"
                        },
                        new Parameter
                        {
                            type = "string",
                            name = "password",
                            required = false,
                            @in = "formData"
                        },
                    }
                }
            });
        }

    }

    internal class AssignOAuth2SecurityRequirements : IOperationFilter
    {

        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            //Adding Versioning 

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


            // Determine if the operation has the Authorize attribute
            var authorizeAttributes = apiDescription.ActionDescriptor.GetCustomAttributes<AuthorizeAttribute>();

            if (authorizeAttributes.Any())
            {

                // Initialize the operation.security property
                if (operation.security == null)
                    operation.security = new List<IDictionary<string, IEnumerable<string>>>();

                // Add the appropriate security definition to the operation
                var oAuthRequirements = new Dictionary<string, IEnumerable<string>>
            {
                { "oauth2", Enumerable.Empty<string>() }
            };

                operation.security.Add(oAuthRequirements);
            }


        }
    }
}
