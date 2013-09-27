using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Moravia.Timely.Controllers
{
    public class FavoritesController : RestController<Models.Favorite, ViewModels.Favorite> { }
}
