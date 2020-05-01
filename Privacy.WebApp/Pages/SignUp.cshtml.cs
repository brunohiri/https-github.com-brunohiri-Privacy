using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Privacy.Data.Entities;
using Privacy.WebApp.Models;
using m = Privacy.WebApp.Models;
using Privacy.Business.Util;
using Privacy.Business.Mail;
using System.Web;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Privacy.Pages
{
    public class SignUpModel : PageModel
    {
        #region Propriedades

        #region Passo 2

        [BindProperty]
        public string Nome { get; set; }

        [BindProperty]
        public string CPF { get; set; }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public DateTime DataNascimento { get; set; }

        [BindProperty]
        public List<SelectListItem> OptionsGenero { get; set; }

        [BindProperty]
        public List<SelectListItem> OptionsEtnia { get; set; }

        [BindProperty]
        public List<SelectListItem> OptionsInteresse { get; set; }
        #endregion

        #region Passo 3
        [BindProperty]
        public IFormFile Upload { get; set; }

        #endregion

        #region Passo 4
        [BindProperty]
        public string Senha { get; set; }
        [BindProperty]
        public string Celular { get; set; }

        public Usuario UsuarioLogado { get; set; }

        [BindProperty]
        public string FotoPerfilBase64 { get; set; }

        #endregion

        private IHostingEnvironment _environment;
        private IConfiguration _configuration;

        [BindProperty]
        public string Mensagem { get; set; }

        #endregion

        #region Construtores

        public SignUpModel(IConfiguration configuration, IHostingEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }
        #endregion

        public IActionResult OnGet()
        {
            using (PrivacyContext context = new PrivacyContext())
            {
                OptionsGenero = context.Genero.Select(a =>
                                 new SelectListItem
                                 {
                                     Value = a.IdGenero.ToString(),
                                     Text = a.Descricao
                                 }).ToList();

                OptionsEtnia = context.Etnia.Select(a =>
                                 new SelectListItem
                                 {
                                     Value = a.IdEtnia.ToString(),
                                     Text = a.Descricao
                                 }).ToList();

                OptionsInteresse = context.Interesse.Select(a =>
                                 new SelectListItem
                                 {
                                     Value = a.IdInteresse.ToString(),
                                     Text = a.Descricao
                                 }).ToList();
            }

            return Page();
        }



        public IActionResult OnGetVerificarDados2(string Nome, DateTime DataNascimento, string Email, string PerfilPublico, string CPF = null, int Etnia = 0, int Genero = 0, int Interesse = 0)
        {
            //bool isCPF = Util.IsCpf(CPF);
            bool achou = m.SignUpModel.VerificaUsuario(Email);

            if (DataNascimento >= DateTime.Today || DataNascimento.Ticks == 0 || DataNascimento < DateTime.Today.AddYears(-100))
            {
                Mensagem = "Data de nascimento inválida!";
                return new JsonResult(new { OK = false, Mensagem = Mensagem }, new Newtonsoft.Json.JsonSerializerSettings() { });
            }

            if (DataNascimento >= DateTime.Today.AddYears(-17))
            {
                Mensagem = "Somente pessoas maiores de idade podem se cadastrar!";
                return new JsonResult(new { OK = false, Mensagem = Mensagem }, new Newtonsoft.Json.JsonSerializerSettings() { });
            }

            //Ocultado por enquanto
            //if (!isCPF)
            //{
            //    Mensagem = "CPF inválido!";
            //    return new JsonResult(new { OK = false, Mensagem = Mensagem }, new Newtonsoft.Json.JsonSerializerSettings() { });
            //}

            if (achou)
            {
                Mensagem = "Este email já está cadastrado. Por favor, verifique!";
                return new JsonResult(new { OK = false, Mensagem = Mensagem }, new Newtonsoft.Json.JsonSerializerSettings() { });
            }

            Usuario usuario = new Usuario()
            {
                Nome = Nome.Trim(),
                CPF = CPF,
                DataNascimento = DataNascimento,
                Email = Email,
                IdEtnia = Etnia,
                IdGenero = Genero,
                IdInteresse = Interesse,
                PerfilPublico = (PerfilPublico == "1")
            };

            HttpContext.Session.SetObjectAsJson("USUARIO", usuario);


            return new JsonResult(new { OK = true, Mensagem = Mensagem }, new Newtonsoft.Json.JsonSerializerSettings() { });
        }

        public IActionResult OnPostPasso3(string FotoPerfil)
        {
            Usuario usuario = HttpContext.Session.GetObjectFromJson<Usuario>("USUARIO");
            bool retorno = true;

            string filePath = /*_environment.WebRootPath +*/ _configuration["CaminhoFotoPerfil"].ToString();
            byte[] bytes = Convert.FromBase64String(System.Web.HttpUtility.UrlDecode(FotoPerfil).Split(',')[1]);
            string fileName = string.Format("FotoPerfil_{0:dd_MM_yyyy_HH_mm_ss}.{1}", DateTime.Now, HttpUtility.UrlDecode(FotoPerfil).Contains("image/png") ? "png" : "jpg");

            //fileName = Path.Combine(filePath, fileName);

            if (bytes != null)
            {
                System.IO.File.WriteAllBytes(Path.Combine(_environment.WebRootPath + filePath, fileName), bytes);

                usuario.FotoPerfil = Path.Combine(filePath, fileName);
                HttpContext.Session.SetObjectAsJson("USUARIO", usuario);

                Mensagem = string.Empty;
            }
            else
            {
                retorno = false;
                Mensagem = "Não foi possível salvar a imagem! Por favor, tente novamente!";
            }
            return new JsonResult(new { OK = retorno, Mensagem = Mensagem }, new Newtonsoft.Json.JsonSerializerSettings() { });

        }

        public IActionResult OnGetPasso4(string Senha, string Celular, string QuantoQuer = null)
        {
            Usuario usuario = HttpContext.Session.GetObjectFromJson<Usuario>("USUARIO");

            usuario.Senha = Criptography.Encrypt(Senha);
            usuario.Celular = Celular;
            usuario.QuantoQuer = QuantoQuer.IsNullOrEmpty() ? (decimal?)null : decimal.Parse(QuantoQuer);

            HttpContext.Session.SetObjectAsJson("USUARIO", usuario);

            return new JsonResult(new { OK = true, Mensagem = Mensagem }, new Newtonsoft.Json.JsonSerializerSettings() { });

        }

        public IActionResult OnPostSalvarDados()
        {
            Usuario usuario = HttpContext.Session.GetObjectFromJson<Usuario>("USUARIO");

            if (usuario != null)
            {
                using (PrivacyContext context = new PrivacyContext())
                {
                    usuario.Ativo = false;
                    usuario.DataCadastro = DateTime.Now;
                    usuario.Login = usuario.Email;
                    context.Usuario.Add(usuario);
                    context.SaveChanges();
                }

                string html = MailModel.ReturnBodyTemplate();

                MailModel.Server = _configuration["Smtp:Server"];
                MailModel.User = _configuration["Smtp:User"];
                MailModel.Pass = _configuration["Smtp:Pass"];
                MailModel.Port = _configuration["Smtp:Port"];
                MailModel.EnableSSL = _configuration["Smtp:EnableSSL"];

                html = html.Replace("{Titulo}", "Privacy - Ativação de conta");
                html = html.Replace("{Subtitulo}", string.Format("Olá, {0} para concluir seu cadastro clique no link abaixo:", usuario.Nome));
                html = html.Replace("{Texto}", "<a href=\"" + _configuration["Url"].ToString() + "/ActivateAccount?q=" + HttpUtility.UrlEncode(Criptography.Encrypt(usuario.IdUsuario.ToString())) + "\">Ativar Conta</a>");

                MailModel.SendMail(_configuration["Smtp:User"].ToString(), usuario.Email, "Privacy | Ativação de Conta", html);

                usuario = null;

                return new JsonResult(new { OK = true, Mensagem = Mensagem }, new Newtonsoft.Json.JsonSerializerSettings() { });
            }
            else
                return new JsonResult(new { OK = false, Mensagem = "Falha ao salvar os dados!" }, new Newtonsoft.Json.JsonSerializerSettings() { });
        }

    }
}
