using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Configuration;
using MongoDB.Driver;

namespace Moravia.Timely.Components
{
    public class ActiveDirectoryComponent : Component
    {
        public ActiveDirectoryComponent(MongoDatabase database, IPrincipal principal)
            : base(database, principal)
        {
            Membership = WebConfigurationManager.GetSection("system.web/membership") as MembershipSection;
        }

        private MembershipSection Membership { get; set; }

        public void ImportUser(string provider, string username)
        {
            var setting = Membership.Providers[provider];
            var connectionStringName = WebConfigurationManager.ConnectionStrings[setting.Parameters["connectionStringName"]].ConnectionString;
            var connectionUsername = setting.Parameters["connectionUsername"];
            var connectionPassword = setting.Parameters["connectionPassword"];

            PrincipalContext domain = new PrincipalContext(ContextType.Domain, connectionStringName, connectionUsername, connectionPassword);

            UserPrincipal user = UserPrincipal.FindByIdentity(domain, username);

            Console.WriteLine(user.EmailAddress);
        }
    }
}