using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Document;
using WebRavenDB.Models;
using System.Collections;
using System.Linq;

namespace WebRavenDB.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public IList Get()
        {
            IList users = new List<User>();

            using (var ds = new DocumentStore { Url = "http://localhost:8024/" }.Initialize())
            {
                using (var session = ds.OpenSession("db_home"))
                {
                    users = (from user in session.Query<User>()
                             select user).ToList();
                }
            }

            return users;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public User Get(Guid id)
        {
            using (var ds = new DocumentStore { Url = "http://localhost:8024/" }.Initialize())
            {
                using (var session = ds.OpenSession("db_home"))
                {
                    return session.Load<User>("users/" + id);
                }
            }

        }

        // POST api/values
        [HttpPost]
        public ActionResult Post([FromBody]User user)
        {
            if (user == null)
            {
                return NotFound("Sorry !!");
            }

            using (var ds = new DocumentStore { Url = "http://localhost:8024/" }.Initialize())
            {
                using (var session = ds.OpenSession("db_home"))
                {

                    session.Store(user);
                    session.SaveChanges();
                }
            }

            return Ok("Congratulations !!");
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(Guid id, [FromBody]User user)
        {
            using (var ds = new DocumentStore { Url = "http://localhost:8024/" }.Initialize())
            {
                using (var session = ds.OpenSession("db_home"))
                {
                    user.Id = id;
                    session.Store(user);
                    session.SaveChanges();

                }
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            using (var ds = new DocumentStore { Url = "http://localhost:8024/" }.Initialize())
            {
                using (var session = ds.OpenSession("db_home"))
                {
                    var user = session.Load<User>("users/" + id);

                    if (user != null)
                    {
                        session.Delete(user);
                    }
                }
            }
        }
    }
}
