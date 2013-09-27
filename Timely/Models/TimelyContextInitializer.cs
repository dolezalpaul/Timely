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
                new User() { name = "Guest", email = "premyslk@moravia.com" },
                new User() { name = "PremyslK", email = "premyslk@moravia.com" }
            };

            users.ForEach(user => context.Users.Add(user));

            var projects = new List<Project>()
            {
                new Project() { name = "Timely" },
                new Project() { name = "Symfonie" }
            };

            projects.ForEach(project => context.Projects.Add(project));

            var tasks = new List<Task>()
            {
                new Task() { name = "Programming" },
                new Task() { name = "Grooming" },
                new Task() { name = "Planning" },
                new Task() { name = "Surfing" }
            };

            tasks.ForEach(task => context.Tasks.Add(task));

            var favorites = new List<Favorite>()
            {
                new Favorite() { user = users[2], project = projects[1], task = tasks[0] }
            };

            favorites.ForEach(favorite => context.Favorites.Add(favorite));

            context.SaveChanges();
        }
    }
}