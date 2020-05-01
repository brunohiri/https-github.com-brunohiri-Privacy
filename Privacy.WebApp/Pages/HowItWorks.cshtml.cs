using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Privacy.Business.Util;
using Privacy.Data.Entities;

namespace Privacy.WebApp.Pages
{
    public class HowItWorksModel : PageModel
    {
        public IActionResult OnGet()
        {
            return Page();
        }
    }
}