using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Privacy.Business.Models;
using Privacy.Business.Util;
using Privacy.Data.Entities;
using Privacy.WebApp.Models;

namespace Privacy.WebApp
{
    public class WelcomeModel : PageModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        

        #region Métodos
        public IActionResult OnGet()
        {
            //if (HttpContext.Session.GetObjectFromJson<Usuario>("USUARIO") == null)
            //    return RedirectToPage("/Login");
            return Page();
        }
        #endregion
    }
}