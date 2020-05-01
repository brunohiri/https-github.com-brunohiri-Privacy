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

namespace Privacy.WebApp.Pages
{
    public class ProfilePhotosModel : PageModel
    {
        #region Propriedades
        [BindProperty]
        public Usuario Entity { get; set; }

        [BindProperty]
        public List<Album> ListAlbum { get; set; }
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public IActionResult OnGet(string Id)
        {

            if (HttpContext.Session.GetObjectFromJson<Usuario>("USUARIO") == null)
                return RedirectToPage("/Login");

            Usuario usuario = HttpContext.Session.GetObjectFromJson<Usuario>("USUARIO");

            //Carregar Perfil de Usuário
            Entity = UsuarioModel.ObterUsuario(long.Parse(WebUtility.HtmlEncode(Criptography.Decrypt(Id))));

            ListAlbum = AlbumModel.ObterAlbumUsuario(long.Parse(WebUtility.HtmlEncode(Criptography.Decrypt(Id))));

            return Page();
        }
        #endregion
    }
}