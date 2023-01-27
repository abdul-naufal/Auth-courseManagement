using DataAccess.Repository.IRepository;
using Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace RazorWebApplication.Pages.Admin.Login
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        public IActionResult OnGet()
        {
            return RedirectToPage("/Account/Login", new { area = "Identity", IsAdminRole = true });
        }

    }
}
