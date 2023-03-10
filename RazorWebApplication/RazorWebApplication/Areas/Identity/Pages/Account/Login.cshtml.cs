using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Utility;

namespace RazorWebApplication.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;

        public LoginModel(SignInManager<IdentityUser> signInManager, 
            ILogger<LoginModel> logger,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _roleManager = roleManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }
        public bool IsAdminRole { get; set; }
        public bool IsValid { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string returnUrl = null, bool isAdminRole = false)
        {
            IsValid = true;
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Index");
                //return RedirectToAction("Index", "");
            }
            else
            {
                if (!string.IsNullOrEmpty(ErrorMessage))
                {
                    ModelState.AddModelError(string.Empty, ErrorMessage);
                    IsValid = false;
                }

                returnUrl ??= Url.Content("~/");
                IsAdminRole = isAdminRole;

                // Clear the existing external cookie to ensure a clean login process
                await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

                ReturnUrl = returnUrl;
                return Page();
            }   
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                IsValid = true;
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");

                    var user = await _userManager.FindByEmailAsync(Input.Email);
                    // Get the roles for the user
                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles.Contains(SD.AdminRole))
                    {
                        returnUrl = @Url.Page("/Admin/Index", new { area = "" });
                    }
                    if (roles.Contains(SD.InstructorRole))
                    {
                        returnUrl = @Url.Page("/Instructor/Index", new { area = "" });
                    }
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    IsValid = false;
                    return Page();
                }
            }
            else
            {
                IsValid = false;
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
