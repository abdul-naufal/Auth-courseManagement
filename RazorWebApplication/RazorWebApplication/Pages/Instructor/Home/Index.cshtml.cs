using DataAccess.Repository.IRepository;
using Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace RazorWebApplication.Pages.Instructor.Home
{
    [Authorize(Roles = "Instructor")]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
