using FirstAidBG_v._2.Business.Services;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FirstAidBG_v._2.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserService _userService;

        public LoginController(UserService userService)
        {
            _userService = userService;
        }
        public IActionResult Index()
        {

            return View();
        }
        [HttpGet("Denied")]
        public IActionResult Denied()
        {
            return View();
        }
        [HttpGet("Login")]
        public IActionResult Login(string returnUrl)
        {
            ViewData["ReturnUrl"]= returnUrl;
            return View();
        }
        [HttpGet("Login/{provider}")]
        public IActionResult LoginExternal([FromRoute]string provider, [FromQuery]string returnUrl)
        {
            if (returnUrl == "/Logout")
            {
                returnUrl = "Home";
            }
            if (User != null && User.Identities.Any(identity => identity.IsAuthenticated))
            {
                RedirectToAction("", "Home");

            }
            returnUrl = string.IsNullOrEmpty(returnUrl) ? "/" : returnUrl;
            var authenticationProperties = new AuthenticationProperties { RedirectUri = returnUrl };
            //await HttpContext.ChallengeAsync("google", authenticationProperties).ConfigureAwait(false);

            return new ChallengeResult(provider, authenticationProperties);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Validate(string username, string password, string returnUrl)
        {
            if (returnUrl == "/Logout")
            {
                returnUrl = "Home";
            }

            ViewData["ReturnUrl"] = returnUrl;
            if(_userService.TryValidateUser(username,password, out List<Claim> claims))
            {
               
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);


                var items = new Dictionary<string, string>();
                items.Add(".AuthScheme", CookieAuthenticationDefaults.AuthenticationScheme);
                var properties = new AuthenticationProperties(items);

                await HttpContext.SignInAsync(claimsPrincipal, properties);
                return Redirect(returnUrl);
            }
            else
            {
                TempData["Error"] = "Грешка. Потребителското име или паролата са грешни!";
                return View("Login");
            }
            
        }
        //[Authorize]
        //public async Task<IActionResult> ChangeNameForExternalLogin(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var question = await DbContext.Questions.FindAsync(id);
        //    if (question == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(question);
        //}

        //// POST: Forum/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[Authorize]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ChangeNameForExternalLogin(int id, [Bind("QuestionId,Text,DatePublished")] Question question)
        //{
        //    if (id != question.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(question);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!QuestionExists(question.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(question);
        //}

        
        //style the forum
        //two more pages for info

        [HttpGet("Logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {

            var scheme = User.Claims.FirstOrDefault(i => i.Type == ".AuthScheme").Value;
            if (scheme == "google")
            {
                await HttpContext.SignOutAsync();
                //return Redirect("/");
                return Redirect("https://www.google.com/accounts/Logout?continue=https://appengine.google.com/_ah/logout?continue=https://localhost:7065");
            }          
            else
            {
                await HttpContext.SignOutAsync();
                return RedirectToAction("Index", "Home");
            }
            
        }
    }
}
