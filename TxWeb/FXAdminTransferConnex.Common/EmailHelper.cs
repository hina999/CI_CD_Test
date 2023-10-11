using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace FXAdminTransferConnex.Common
{
    public static class EmailHelper
    {
        public static bool Send(string mailTo, string bodyTemplate, string subject = "", string ccMail = "")
        {
            string email = System.Configuration.ConfigurationManager.AppSettings["Email"];
            string password = System.Configuration.ConfigurationManager.AppSettings["passsword"];
            int PortNumber = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["PortNumber"]);
            string HostName = System.Configuration.ConfigurationManager.AppSettings["HostName"];
            MailMessage mail = new MailMessage();
            mail.To.Add(mailTo);
            mail.From = new MailAddress(email,"Transfer Connex");
            mail.Subject = subject;
            mail.Body = bodyTemplate;
            mail.IsBodyHtml = true;

            if (ccMail != "" && ccMail != null)
            {
                mail.CC.Add(ccMail);
            }

            SmtpClient smtp = new SmtpClient();
            //smtp.Host = "smtp.gmail.com";
            smtp.Host = HostName;

            //smtp.Port = 587;
            smtp.Port = PortNumber;

            //smtp.UseDefaultCredentials = false;
            //smtp.EnableSsl = false;

            smtp.UseDefaultCredentials = true;
            smtp.EnableSsl = true;

            smtp.Credentials = new System.Net.NetworkCredential(email, password);// Enter seders User name and password
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;           
            try
            {
                smtp.Send(mail);
            }
            catch (Exception)
            {

                throw;
            }

            return true;
        }
    }
}
