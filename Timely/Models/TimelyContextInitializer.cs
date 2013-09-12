using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Moravia.Timely.Models
{
    public class TimelyContextInitializer :
        DropCreateDatabaseIfModelChanges<TimelyContext>
    {
        protected override void Seed(TimelyContext context)
        {
            var products = new List<User>()
            {
                new User() { name = "Administrator", email = "premyslk@moravia.com" }
            };

            products.ForEach(p => context.Users.Add(p));
        }
    }
}