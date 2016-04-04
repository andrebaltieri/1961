using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNet.Http.Authentication;
using Microsoft.AspNet.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNet.Authorization;

namespace TodoCore.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        public IActionResult Unauthorized()
        {
            return View();
        }

        public IActionResult Forbidden()
        {
            return View();
        }

        public async Task<IActionResult> Login(string returnUrl = null)
        {
            const string Issuer = "https://contoso.com";
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, "andrebaltieri", ClaimValueTypes.String, Issuer));
            var userIdentity = new ClaimsIdentity("SuperSecureLogin");
            userIdentity.AddClaims(claims);
            var userPrincipal = new ClaimsPrincipal(userIdentity);

            await HttpContext.Authentication.SignInAsync("Cookie", userPrincipal,
                new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.Now.AddMinutes(20),
                    IsPersistent = false,
                    AllowRefresh = false
                });

            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await HttpContext.Authentication.SignOutAsync("Cookie");
            return RedirectToAction("Index", "Home");
        }
    }
}