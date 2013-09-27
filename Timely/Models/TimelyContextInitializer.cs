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
            var users = new List<User>()
            {
                new User() { name = "Administrator", email = "premyslk@moravia.com" },
                new User() { name = "Guest", email = "premyslk@moravia.com" }
            };

            users.ForEach(u => context.Users.Add(u));

            var teams = new List<Team>()
            {
                new Team() { name = "Moravia", users = users }
            };

            teams.ForEach(t => context.Teams.Add(t));
        }
    }
}