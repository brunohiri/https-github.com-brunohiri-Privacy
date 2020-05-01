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

namespace Privacy.WebApp
{
    public class ProfileFriendsModel : PageModel
    {
        #region Propriedades
        [BindProperty]
        public Usuario Entity { get; set; }
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public IActionResult OnGet(string Id)
        {

            //Carregar Perfil de Usuário
            Entity = UsuarioModel.ObterUsuario(long.Parse(WebUtility.HtmlEncode(Criptography.Decrypt(Id))));

            return Page();
        }
        #endregion
    }
}