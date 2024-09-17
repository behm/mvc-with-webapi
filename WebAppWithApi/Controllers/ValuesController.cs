using System;
using System.Collections.Generic;
using System.Web.Http;
using WebAppWithApi.Data;

namespace WebAppWithApi.Controllers
{
    public class ValuesController : ApiController
    {
        private readonly IAppInfoRepo _repo;

        public ValuesController(IAppInfoRepo repo)
        {
            _repo = repo;
        }

        // GET api/values
        public IEnumerable<string> Get()
        {
            var app = _repo.GetAppInfo(Guid.NewGuid());

            return new string[] { "value1", "value2", app.AppInfoId.ToString(), app.FirstName, app.LastName };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
