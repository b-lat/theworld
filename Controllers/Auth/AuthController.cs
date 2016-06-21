namespace TheWorld.Controllers.Auth
{
    using System.Threading.Tasks;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Mvc;

    using TheWorld.Models;
    using TheWorld.ViewModels;

    public class AuthController : Controller
    {
        private readonly SignInManager<WorldUser> _signInManager;

        public AuthController(SignInManager<WorldUser> signInManager)
        {
            this._signInManager = signInManager;
        }

        public IActionResult Login()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return this.RedirectToAction("Trips", "App");
            }

            return this.View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel vm, string returnUrl)
        {
            if (this.ModelState.IsValid)
            {
                var signInResult = await this._signInManager.PasswordSignInAsync(vm.Username, 
                    vm.Password, 
                    true, false);

                if (signInResult.Succeeded)
                {
                    if (string.IsNullOrWhiteSpace(returnUrl))
                    {
                        return this.RedirectToAction("Trips", "App");
                    }
                    else
                    {
                        return this.Redirect(returnUrl);
                    }
                }
                else
                {
                    this.ModelState.AddModelError("", "Username or password incorrect");
                }
            }

            return this.View();
        }

        public async Task<IActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                await this._signInManager.SignOutAsync();
            }

            return RedirectToAction("Index", "App");
        }
    }
}
