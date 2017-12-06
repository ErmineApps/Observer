using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace Observer.ControllersApi
{
    [System.Web.Http.Authorize]
    public class AccountController : ApiController
    {
        [System.Web.Http.AllowAnonymous]
        public IHttpActionResult Login(string returnUrl)
        {
            
            return Ok();
        }

   
    }
}
