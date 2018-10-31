using Microsoft.Web.Http;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;

namespace WebAPIOauth.Controllers
{
    /// <summary>
    /// Values Controller.
    /// </summary>
    //Here we can metion the applicable versions and Deprecated details.
    [ApiVersion("1.0", Deprecated = true)]
    [ApiVersion("2.0")]
    [RoutePrefix("api/v{apiversion}/values")] /*Need to decrote the controller routeprefix when we defined any Route in the methods.*/
    public class ValuesController : ApiController
    {
        /// <summary>
        /// Get all the values.
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(string[]))]
        [MapToApiVersion("1.0")]
        [Obsolete("Method1 is deprecated, please use Method2 instead.", true)]
        public IEnumerable<string> Get()
        { 
            return new string[] { "version Default", "version Default " };
        }


        /// <summary>
        /// Get the specific value.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(string))]
        public string Get(int id)
        {
            return "version 1";
        }

        /// <summary>
        /// Version 2 get all the values.
        /// </summary>
        /// <returns></returns>
        [HttpGet, MapToApiVersion("2.0")] 
        [ResponseType(typeof(string[]))]
        public IEnumerable<string> GetV2()
        {
            return new string[] { "Version 2", "version 3" };
        }


        /// <summary>
        /// Version 2 get single value based on id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/Values/5 
        [HttpGet, MapToApiVersion("2.0")]
        [ResponseType(typeof(string))]
        public string GetV2(int id)
        {
            return "version 2";
        }

        // POST: api/Values 
        public void Post([FromBody]string value)
        {
        } 

        // PUT: api/Values/5
        public void Put(int id, [FromBody]string value)
        {
        }
         
        // DELETE: api/Values/5
        public void Delete(int id)
        {
        }

        /// <summary>
        /// Get user 
        /// </summary>
        /// <returns></returns>
        [Route("getuser")]
        [ResponseType(typeof(string))]
        public string GetUser()
        {
            return "version 1";
        }

    }
}
