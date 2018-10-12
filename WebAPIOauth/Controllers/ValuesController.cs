using Microsoft.Web.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace WebAPIOauth.Controllers
{
    [ApiVersion("1.0", Deprecated = true)]
    [ApiVersion("2.0")]
    [ApiVersion("3.0")]
    [RoutePrefix("api/v{version:apiVersion}/values")]

    public class ValuesController : ApiController
    {


        [Route("")]
        [HttpGet]
        [ResponseType(typeof(string[]))]
        [Authorize]
        public IEnumerable<string> Get()
        {


            return new string[] { "version Default", "version Default " };
        }

        // GET: api/Values/5
        [Route("{id:int}")]
        [ResponseType(typeof(string))]
        public string Get(int id)
        {
            return "version 1";
        }

        [Route("")]
        [HttpGet, MapToApiVersion("2.0")]
        [MapToApiVersion("3.0")]
        [ResponseType(typeof(string[]))]
        public IEnumerable<string> GetV2()
        {
            return new string[] { "Version 2", "version 3" };
        }



        // GET: api/Values/5
        [Route("{id:int}")]
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
        [Authorize]
        // PUT: api/Values/5
        public void Put(int id, [FromBody]string value)
        {
        }
        [AllowAnonymous]
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
