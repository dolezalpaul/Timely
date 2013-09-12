using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Moravia.Timely.Components;

namespace Moravia.Timely.Controllers
{
    public class AccountController : Controller
    {
        public ActiveDirectoryComponent ActiveDirectoryComponent { get; set; }

        public AccountController(ActiveDirectoryComponent activeDirectoryComponent)
        {
            ActiveDirectoryComponent = activeDirectoryComponent;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View("Login");
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Index(string username, string password, Uri returnUrl)
        {
            if (ModelState.IsValid)
            {
                ActionResult response;
                response = TryAuthenticate("CZ", username, password, "Moravia|Vendor");
                if (response != null)
                {
                    return response;
                }
                response = TryAuthenticate("MNET", username, password, "Vendor");
                if (response != null)
                {
                    return response;
                }
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View("Login");
        }

        public ActionResult TryAuthenticate(string provider, string username, string password, string roles)
        {
            if (Membership.Providers[provider].ValidateUser(username, password))
            {
                //ActiveDirectoryComponent.ImportUser(provider, username);

                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                  username,
                  DateTime.Now,
                  DateTime.Now.AddDays(7),
                  true,
                  roles,
                  FormsAuthentication.FormsCookiePath);

                // Encrypt the ticket.
                string encTicket = FormsAuthentication.Encrypt(ticket);

                // Create the cookie.
                Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

                // Redirect back to original URL.
                var url = FormsAuthentication.GetRedirectUrl(username, true);
                if (!url.EndsWith("/"))
                {
                    url += "/";
                }
                return Redirect(url);
            }
            return null;
        }

        [HttpGet]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return this.Redirect(Request.ApplicationPath);
        }
    }
}
