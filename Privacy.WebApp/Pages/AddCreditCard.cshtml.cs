using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagarMe;
using Privacy.Business.Models;
using Privacy.Business.Util;
using Privacy.Data.Entities;
using Privacy.WebApp.Models;

namespace Privacy.WebApp
{
    public class AddCreditCardModel : PageModel
    {
        #region Propriedades
        [BindProperty]
        public Usuario Entity { get; set; }

        public Usuario UsuarioLogado { get; set; }
        #endregion

        #region Construtores
        #endregion
      
        #region Métodos
        public IActionResult OnGet(string TokenTransacao, string IdPerfil)
        {            
            if (HttpContext.Session.GetObjectFromJson<Usuario>("USUARIO") == null)
                return RedirectToPage("/Login");

            UsuarioLogado = HttpContext.Session.GetObjectFromJson<Usuario>("USUARIO");

            PagarMeService.DefaultApiKey = "ak_test_1V9G2oIoRaRSMUD6rQZKI9XQgJzYMg";
            PagarMeService.DefaultEncryptionKey = "ek_test_YTEFhGJ5f814Q75R3xMKPmTmNKAtK7";

            Transaction transaction = PagarMeService.GetDefaultService().Transactions.Find(TokenTransacao);
            //transaction.Capture(transaction.Amount);


            using (PrivacyContext context = new PrivacyContext())
            {
                var usuarioPerfil = UsuarioModel.ObterUsuario(long.Parse(WebUtility.HtmlEncode(Criptography.Decrypt(IdPerfil))));

                Transacao objetoTransacao = new Transacao();

                objetoTransacao.Valor = transaction.Amount;
                objetoTransacao.TokenPayPal = TokenTransacao;
                objetoTransacao.TransactionIdPayPal = transaction.Tid;
                objetoTransacao.Ip = transaction.IP;
                objetoTransacao.PaymentStatusPayPal = transaction.Status.ToString();
                objetoTransacao.DataTransacao = DateTime.Now;
                objetoTransacao.PayerIdPayPal = transaction.Billing.Id;
                objetoTransacao.IdUsuario = UsuarioLogado.IdUsuario;
                objetoTransacao.IdPerfil = usuarioPerfil.IdUsuario;

                context.Transacao.Add(objetoTransacao);

                int salvou = context.SaveChanges();

                if (salvou == 1)
                    //return RedirectToPage("/ProfilePhotos", new { Id = IdPerfil }); //Redirecionar para as fotos
                    return new JsonResult(new { OK = true }, new Newtonsoft.Json.JsonSerializerSettings() { });

            }

            return null;
        }
        #endregion
    }
}