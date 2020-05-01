using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Privacy.Business.Mail
{
    public class MailModel
    {
        #region Propriedades
        public static string Server { get; set; }
        public static string User { get; set; }
        public static string Pass { get; set; }
        public static string Port { get; set; }
        public static string EnableSSL { get; set; }

        #endregion

        #region Construtores
        public MailModel()
        {

        }
        #endregion

        #region Métodos

        public static string ReturnBodyTemplate()
        {
            string HtmlBody = string.Empty;
            HtmlBody += "<!DOCTYPE html>";
            HtmlBody += "<html lang=\"pt-br\">";
            HtmlBody += "   <head>";
            HtmlBody += "		<meta charset=\"utf-8\" />";
            HtmlBody += "		<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\" />";
            HtmlBody += "		<title>Privacy</title>";
            HtmlBody += "		<meta content=\"\" name=\"keywords\">";
            HtmlBody += "		<meta content=\"\" name=\"description\">";
            HtmlBody += "		<link rel=\"icon\" type=\"image/png\" href=\"http://webprivacy.azurewebsites.net/assets/img/favicon.png\" />";
            HtmlBody += "         <style>";
            HtmlBody += "			body{";
            HtmlBody += "				background:#F0F0F0;";
            HtmlBody += "				display: -webkit-box;";
            HtmlBody += "				display: -ms-flexbox;";
            HtmlBody += "				display: flex;";
            HtmlBody += "				-webkit-box-align: center;";
            HtmlBody += "				-ms-flex-align: center;";
            HtmlBody += "				align-items: center;";
            HtmlBody += "				min-height: calc(100vh - 533px);";
            HtmlBody += "			}";
            HtmlBody += "			.container{";
            HtmlBody += "				margin-left: 100px;";
            HtmlBody += "			}";
            HtmlBody += "			.form-panel{";
            //HtmlBody += "				width: 100%;";
            //HtmlBody += "				background: #fff;";
            //HtmlBody += "				border: 1px solid #e8e8e8;";
            //HtmlBody += "				border-radius: 8px;";
            //HtmlBody += "				padding: 30px;";
            HtmlBody += "			}";
            HtmlBody += "			.form-panel p{";
            HtmlBody += "				padding-bottom:10px;  ";
            HtmlBody += "			}";
            HtmlBody += "		</style>";
            HtmlBody += "   </head>";
            HtmlBody += "   <body>";
            HtmlBody += "	    <div class=\"container\">";
            HtmlBody += "			<div id=\"logo\" class=\"logo\">";
            HtmlBody += "				<h1><img src=\"http://webprivacy.azurewebsites.net/assets/img/telaLogin/Logo-Azul.png\" alt=\"Privacy\" /></h1>";
            HtmlBody += "			</div>";
            HtmlBody += "            <div class=\"form-panel\">";
            HtmlBody += "				<h3>{Titulo}</h3>";
            HtmlBody += "				<p>{Subtitulo}</p>";
            HtmlBody += "               <p>{Texto}</p>";
            HtmlBody += "			    <div class=\"\">";
            HtmlBody += "				    <p>&copy; " + DateTime.Now.Year.ToString() + " - <strong><a href=\"http://webprivacy.azurewebsites.net/\">Privacy</a></strong>. Todos os Direitos Reservados.</p>";
            HtmlBody += "			    </div>";
            HtmlBody += "			</div>";
            HtmlBody += "		</div>";
            HtmlBody += "	</body>";
            HtmlBody += "</html>";
            return HtmlBody;
        }

        public static bool SendMail(string FromAddress, string ToAddress, string Subject, string HtmlBody)
        {
            try
            {
                SmtpClient client = new SmtpClient(Server);
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(User, Pass);
                client.EnableSsl = Convert.ToBoolean(EnableSSL);
                client.Port = int.Parse(Port);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(FromAddress);
                if (!ToAddress.Contains(";"))
                    mailMessage.To.Add(ToAddress);
                else
                {
                    ToAddress.Split(';').ToList().ForEach(delegate (string Item)
                    {
                        mailMessage.To.Add(Item);
                    });
                }
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = HtmlBody;
                mailMessage.Subject = Subject;
                client.Send(mailMessage);
            }
            catch (SmtpException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
            return true;
        }
        #endregion
    }
}
