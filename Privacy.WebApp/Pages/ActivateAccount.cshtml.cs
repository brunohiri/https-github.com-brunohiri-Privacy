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
    public class ActivateAccountModel : PageModel
    {
        public IActionResult OnGet(string q = null)
        {
            if (!q.IsNullOrEmpty())
            {
                long IdUsuario = Criptography.Decrypt(q).ExtractLong();

                using(PrivacyContext context = new PrivacyContext())
                {
                    Usuario user = context.Usuario.Where(x => x.IdUsuario == IdUsuario).FirstOrDefault();
                    user.Ativo = true;
                    user.DataCadastro = DateTime.Now;
                    context.SaveChanges();
                }

                return Page();
            }
            else
                return RedirectToAction("/Login");
        }
    }
}