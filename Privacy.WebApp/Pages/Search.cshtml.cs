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
    public class SearchModel : PageModel
    {
        #region Propriedades
        [BindProperty]
        public Usuario Entity { get; set; }

        [BindProperty]
        public List<ResultPesquisaModel> ListPesquisa { get; set; }
        #endregion                                                           

        #region Construtores
        #endregion

        #region Métodos
        public IActionResult OnGet(string TextoPesquisa = null)
        {
            if (HttpContext.Session.GetObjectFromJson<Usuario>("USUARIO") == null)
                return RedirectToPage("/Login");

            Entity = HttpContext.Session.GetObjectFromJson<Usuario>("USUARIO");

            if (!TextoPesquisa.IsNullOrEmpty())
                ListPesquisa = UsuarioModel.ObterPesquisa(TextoPesquisa);

            return Page();
        }

        public IActionResult OnPost([FromBody] string TextoPesquisa)
        {
           if (HttpContext.Session.GetObjectFromJson<Usuario>("USUARIO") == null)
                return RedirectToPage("/Login");

            if (!TextoPesquisa.IsNullOrEmpty())
                ListPesquisa = UsuarioModel.ObterPesquisa(TextoPesquisa);

            //return Page();
            return new JsonResult(new { OK = true, ListPesquisa = ListPesquisa }, new Newtonsoft.Json.JsonSerializerSettings() { });
        }
        #endregion
    }
}