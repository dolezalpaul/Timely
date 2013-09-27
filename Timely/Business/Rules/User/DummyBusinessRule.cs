using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Moravia.Timely.Models;

namespace Moravia.Timely.Business.Rules
{
    public class DummyBusinessRule : BusinessRule<User>
    {
        public override void Apply(User entity)
        {
            entity.email = entity.name = "@moravia.com";
        }
    }
}