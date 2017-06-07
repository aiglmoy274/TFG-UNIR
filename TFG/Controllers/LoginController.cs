using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using TFG.Models;
using TFG.Models.ViewModels;
//using TFG.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TFG.Data;

namespace TFG.Controllers
{
    [Authorize(Roles = "Administrador,Usuario")]
    public class LoginController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly string _externalCookieScheme;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _context;

        
        public LoginController(
            ApplicationDbContext context,
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    IOptions<IdentityCookieOptions> identityCookieOptions,
    ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _externalCookieScheme = identityCookieOptions.Value.ExternalCookieAuthenticationScheme;
            _logger = loggerFactory.CreateLogger<LoginController>();
            _context = context;
        }

        //
        // GET: /Cuenta/CambiarPassword
        public IActionResult CambiarPassword()
        {
            return View();
        }

        //
        // POST: /Cuenta/CambiarPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarPassword(CambiarPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user != null)
            {
                var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation(3, "Contraseña cambiada con éxito");
                    return RedirectToAction(nameof(Index), new { Mensaje = "Contraseña cambiada con éxito" });
                }
                
                AddErrors(result);
                //ModelState.AddModelError(string.Empty, "Error al cambiar la contraseña");
                return View(model);
            }
            return RedirectToAction(nameof(Index), new { Mensaje = "No existe usuario" });
        }

        public IActionResult Index()
        {            
            return View();
        }

        public IActionResult Welcome()
        {
            return View();
        }

        // GET: /Account/Login
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> IniciarSesion(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.Authentication.SignOutAsync(_externalCookieScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IniciarSesion(IniciarSesionViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {               
                var result = await _signInManager.PasswordSignInAsync(model.CodigoCentro, model.Password, model.Recordarme, lockoutOnFailure: false);                
                if (result.Succeeded)
                {
                    _logger.LogInformation(1, "Sesión iniciada");
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Usuario y/o contraseña errónea");
                    return View(model);
                }
            }
            return View(model);
        }

       


        //
        // POST: /Cuenta/CerrarSesion
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CerrarSesion()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation(4, "Sesión cerrada");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }


        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Welcome");
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }


    }



}