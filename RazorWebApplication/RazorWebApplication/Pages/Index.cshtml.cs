using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility;

namespace RazorWebApplication.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            if(User.IsInRole(SD.AdminRole)) {
                return RedirectToPage("/Admin/Index");
            }
            if (User.IsInRole(SD.InstructorRole))
            {
                return RedirectToPage("/Instructor/index");
            }
            return Page();
        }
    }
}
