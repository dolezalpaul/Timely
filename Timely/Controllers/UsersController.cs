using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.OData;
using Moravia.Timely.Models;

namespace Moravia.Timely.Controllers
{
    public class UsersController : ApiController
    {
        private TimelyContext Context { get; set; }

        public UsersController(TimelyContext context)
        {
            Context = context;
        }

        public IQueryable<User> Get()
        {
            return Context.Users;
        }

        public User Get(int id)
        {
            return Context.Users.FirstOrDefault(p => p.id == id);
        }
    }
}
