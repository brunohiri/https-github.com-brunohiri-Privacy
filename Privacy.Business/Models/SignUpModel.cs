using Privacy.Business.Util;
using Privacy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Privacy.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Privacy.WebApp.Models
{
    public static class SignUpModel
    {
        //public static Usuario Gravar(Usuario usuario)
        //{

        //}

        public static bool VerificaUsuario(string email)
        {
            bool achou = false;
            if (!email.IsNullOrEmpty())
            {
                using (PrivacyContext context = new PrivacyContext())
                {
                   
                    //var usuario = context.Usuario.Where(c => c.CPF.PadronizarCPF() == cpf.PadronizarCPF()).FirstOrDefault();
                    var usuario = context.Usuario.Where(c => c.Email.Trim().ToLower() == email.Trim().ToLower()).FirstOrDefault();

                    if (usuario != null)
                        achou = true;
                    
                }
            }
            return achou;
        }
    }
}
