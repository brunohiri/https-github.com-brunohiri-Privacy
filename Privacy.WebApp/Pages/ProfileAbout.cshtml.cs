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
    public class ProfileAboutModel : PageModel
    {
        #region Propriedades
        [BindProperty]
        public Usuario Entity { get; set; }
        //public Valor Valor { get; set; }
        #endregion

        #region Construtores
        #endregion

        

        #region Métodos
        public IActionResult OnGet(string Id)
        {
            if (HttpContext.Session.GetObjectFromJson<Usuario>("USUARIO") == null)
                return RedirectToPage("/Login");

            #region Verifica se o usuário logado já comprou a mina rs            
            using (PrivacyContext context = new PrivacyContext())
            {
                Entity = UsuarioModel.ObterUsuario(long.Parse(WebUtility.HtmlEncode(Criptography.Decrypt(Id))));
                var UsuarioLogado = HttpContext.Session.GetObjectFromJson<Usuario>("USUARIO");

                var assinado = context.Transacao.Where(a => a.IdUsuario == UsuarioLogado.IdUsuario && a.IdPerfil == Entity.IdUsuario).FirstOrDefault();

                if (assinado != null)
                    return RedirectToPage("/ProfilePhotos", new { Id = Id });
            }
            #endregion

            //Carregar Perfil de Usuário
            Entity = UsuarioModel.ObterUsuario(long.Parse(WebUtility.HtmlEncode(Criptography.Decrypt(Id))));
            //Valor = Entity.Valor.Where(a => a.IdUsuario == Entity.IdUsuario).FirstOrDefault();
            return Page();
        }
        #endregion
    }
}