using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Privacy.Business.Mail;
using Privacy.Business.Util;
using Privacy.Data.Entities;
using Privacy.WebApp.Models;

namespace Privacy.Pages
{
    public class LoginModel : PageModel
    {
        #region Propriedades
        [BindProperty]
        public string Usuario { get; set; }

        [BindProperty]
        public string Senha { get; set; }

        [BindProperty]
        public string Mensagem { get; set; }

        private IHostingEnvironment _environment;
        private IConfiguration _configuration;
        #endregion

        public LoginModel(IConfiguration configuration, IHostingEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        public IActionResult OnGet()
        {
            HttpContext.Session.SetObjectAsJson("USUARIO", null);
            return Page();
        }

        public IActionResult OnPost()
        {
            if ((Usuario.IsNullOrEmpty() || Senha.IsNullOrEmpty()))
                Mensagem = "Login/Senha obrigatórios em branco!";
            else
            {
                using (PrivacyContext context = new PrivacyContext())
                {
                    var usuario = context.Usuario.Where(x => x.Login.ToLower() == Usuario.ToLower() && x.Senha == Criptography.Encrypt(Senha)).FirstOrDefault();

                    if (usuario != null)
                    {
                        HttpContext.Session.SetObjectAsJson("USUARIO", usuario);
                        if (!usuario.Ativo)
                            return RedirectToPage("/Welcome");
                        //Mensagem = "Usuário inativo! Verifique a caixa de entrada de seu email!";
                        else
                        {
                            return RedirectToPage("/Index");
                        }
                    }
                    else
                        Mensagem = "Login/Senha inválidos!";
                }
            }

            return Page();
        }
    }
}
