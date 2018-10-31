using Microsoft.Web.Http;
using System.Collections.Generic;
using System.Web.Http;

namespace WebAPIOauth.Controllers
{
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class TestController : ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [MapToApiVersion("1.0")]
        public IEnumerable<string> Get()
        {
            return new string[] { "Version 1 GET", "value2" };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>         
        [MapToApiVersion("2.0")] /*This is we need to define for version routing. Here When the user calling
        v2/test get mthod based on the mapapiverison attribute it will route. In Case the method not required versioning then no need to define. it default takes from the 
        Default attribute routing.*/
        public IEnumerable<string> GetV2() 
        {
            return new string[] { "Version 2 GET", "Version 2" };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/Test/5 /*This method is applicable for all the controller applicable versioning.*/
        public string Get(int id)
        {
            return "String ";
        }
       
        // POST: api/Test
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Test/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Test/5
        public void Delete(int id)
        {
        }
    }
}
