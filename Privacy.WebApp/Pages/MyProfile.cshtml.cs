using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Privacy.Business.Models;
using Privacy.Business.Util;
using Privacy.Data.Entities;
using Privacy.WebApp.Models;
using Remotion.Linq.Utilities;

namespace Privacy.WebApp.Pages
{
    public class MyProfile : PageModel
    {
        #region Propriedades
        [BindProperty]
        public Usuario Entity { get; set; }

        [BindProperty]
        public string Mensagem { get; set; }

        [BindProperty]
        public string PerfilCidadePais { get; set; }

        [BindProperty]
        public string PerfilCelular { get; set; }

        private IHostingEnvironment _environment;
        private IConfiguration _configuration;

        [BindProperty]
        public decimal ValorMensal { get; set; }

        [BindProperty]
        public decimal ValorTrimestral { get; set; }

        [BindProperty]
        public decimal ValorSemestral { get; set; }

        #endregion

        #region Construtores
        public MyProfile(IConfiguration configuration, IHostingEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }
        #endregion

        #region Métodos
        public IActionResult OnGet(string Id)
        {

            if (HttpContext.Session.GetObjectFromJson<Usuario>("USUARIO") == null)
                return RedirectToPage("/Login");

            //Carregar Perfil de Usuário
            Entity = UsuarioModel.ObterUsuario(long.Parse(WebUtility.HtmlEncode(Criptography.Decrypt(Id))));

            return Page();
        }

        #region Método para obter os dados do usuário para a modal Pagamento

        public IActionResult OnGetObterDadosUsuarioLoado()
        {
            if (HttpContext.Session.GetObjectFromJson<Usuario>("USUARIO") == null)
                return RedirectToPage("/Login");

            Entity = HttpContext.Session.GetObjectFromJson<Usuario>("USUARIO");
            return new JsonResult(new { Usuario = Entity }, new Newtonsoft.Json.JsonSerializerSettings() { });
        }
        #endregion

        public IActionResult OnPostUploadCapa(string FotoCapa)
        {
            if (HttpContext.Session.GetObjectFromJson<Usuario>("USUARIO") == null)
                return RedirectToPage("/Login");

            Usuario usuario = HttpContext.Session.GetObjectFromJson<Usuario>("USUARIO");

            string filePath = _configuration["CaminhoFotoCapa"].ToString();
            if (FotoCapa != null)
            {
                byte[] bytes = Convert.FromBase64String(HttpUtility.UrlDecode(FotoCapa).Split(',')[1]);
                string fileName = string.Format("FotoCapa_{0:dd_MM_yyyy_HH_mm_ss}.{1}", DateTime.Now, HttpUtility.UrlDecode(FotoCapa).Contains("image/png") ? "png" : "jpg");

                if (bytes != null)
                {
                    System.IO.File.WriteAllBytes(Path.Combine(_environment.WebRootPath + filePath, fileName), bytes);

                    usuario.FotoCapa = Path.Combine(filePath, fileName);

                    using (PrivacyContext context = new PrivacyContext())
                    {
                        var UsuarioBanco = context.Usuario.Where(x => x.IdUsuario == usuario.IdUsuario).FirstOrDefault();
                        UsuarioBanco.FotoCapa = usuario.FotoCapa;
                        context.SaveChanges();
                    }

                    HttpContext.Session.SetObjectAsJson("USUARIO", usuario);
                }
                else
                {
                    Mensagem = "Não foi possível salvar a imagem! Por favor, tente novamente!";
                }
            }
            Entity = UsuarioModel.ObterUsuario(usuario.IdUsuario);

            return Page();

        }

        public IActionResult OnPostUploadPerfil(string FotoPerfil)
        {
            if (HttpContext.Session.GetObjectFromJson<Usuario>("USUARIO") == null)
                return RedirectToPage("/Login");

            Usuario usuario = HttpContext.Session.GetObjectFromJson<Usuario>("USUARIO");

            string filePath = _configuration["CaminhoFotoCapa"].ToString();
            if (FotoPerfil != null)
            {
                byte[] bytes = Convert.FromBase64String(HttpUtility.UrlDecode(FotoPerfil).Split(',')[1]);
                string fileName = string.Format("FotoCapa_{0:dd_MM_yyyy_HH_mm_ss}.{1}", DateTime.Now, HttpUtility.UrlDecode(FotoPerfil).Contains("image/png") ? "png" : "jpg");

                if (bytes != null)
                {
                    System.IO.File.WriteAllBytes(Path.Combine(_environment.WebRootPath + filePath, fileName), bytes);

                    usuario.FotoPerfil = Path.Combine(filePath, fileName);

                    using (PrivacyContext context = new PrivacyContext())
                    {
                        var UsuarioBanco = context.Usuario.Where(x => x.IdUsuario == usuario.IdUsuario).FirstOrDefault();
                        UsuarioBanco.FotoPerfil = usuario.FotoPerfil;
                        context.SaveChanges();
                    }

                    HttpContext.Session.SetObjectAsJson("USUARIO", usuario);
                }
                else
                {
                    Mensagem = "Não foi possível salvar a imagem! Por favor, tente novamente!";
                }
            }
            Entity = UsuarioModel.ObterUsuario(usuario.IdUsuario);

            return Page();

        }

        public IActionResult OnPostSalvarSobreMim(string PerfilSobreMim, string PerfilCidadePais, string PerfilCelular)
        {
            if (HttpContext.Session.GetObjectFromJson<Usuario>("USUARIO") == null)
                return RedirectToPage("/Login");

            Usuario usuario = HttpContext.Session.GetObjectFromJson<Usuario>("USUARIO");

            if (PerfilSobreMim != null)
            {
                using (PrivacyContext context = new PrivacyContext())
                {
                    var UsuarioBanco = context.Usuario.Where(x => x.IdUsuario == usuario.IdUsuario).FirstOrDefault();
                    UsuarioBanco.Cidade = PerfilCidadePais.IsNullOrEmpty() ? null : PerfilCidadePais.Split(',')[0].ToString().Trim();
                    UsuarioBanco.Pais = PerfilCidadePais.IsNullOrEmpty() ? null : PerfilCidadePais.Split(',')[1].ToString().Trim();
                    UsuarioBanco.Celular = PerfilCelular.IsNullOrEmpty() ? null : PerfilCelular;
                    UsuarioBanco.SobreMim = PerfilSobreMim;
                    context.SaveChanges();
                }
                Mensagem = "Perfil salvo com sucesso!";

            }
            else
                Mensagem = "Problema ao salvar!";
            Entity = UsuarioModel.ObterUsuario(usuario.IdUsuario);

            return Page();

        }

        public IActionResult OnPostSalvarValores(decimal ValorMensal, decimal ValorTrimestral, decimal ValorSemestral)
        {
            if (HttpContext.Session.GetObjectFromJson<Usuario>("USUARIO") == null)
                return RedirectToPage("/Login");

            Usuario usuario = HttpContext.Session.GetObjectFromJson<Usuario>("USUARIO");

            if (ValorMensal > 0 && ValorTrimestral > 0 && ValorSemestral > 0)
            {
                using(PrivacyContext context = new PrivacyContext())
                {
                    Valor valor = new Valor();

                    valor.IdUsuario = usuario.IdUsuario;
                    valor.ValorMensal = ValorMensal;
                    valor.ValorTrimestral = ValorTrimestral;
                    valor.ValorSemestral = ValorSemestral;

                    context.Valor.Add(valor);

                    context.SaveChanges();
                }
                Mensagem = "Valores salvos com sucesso!";
                
            }
            else
                Mensagem = "Problema ao salvar!";
            Entity = UsuarioModel.ObterUsuario(usuario.IdUsuario);

            return Page();

        }

        #endregion


    }


}