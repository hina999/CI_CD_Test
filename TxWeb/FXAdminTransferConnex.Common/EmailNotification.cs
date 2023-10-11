//-----------------------------------------------------------------------
// <copyright file="EmailNotification.cs" company="Premiere Digital Services">
//     Copyright Premiere Digital Services. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace FXAdminTransferConnex.Common
{
    /// <summary>
    /// This Class EmailNotification will use to send email 
    /// </summary>
    public class EmailNotification
    {
        /// <summary>
        /// Sends the mail message.
        /// </summary>
        /// <param name="recipient">The recipient.</param>
        /// <param name="bcc">The BCC.</param>
        /// <param name="cc">The cc.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        /// <param name="emailSetting">The email setting.</param>
        /// <param name="attachment"></param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool SendMailMessage(string recipient, string bcc, string cc, string subject, string body, string attachment)
        {
            string email = System.Configuration.ConfigurationManager.AppSettings["Email"];
            string password = System.Configuration.ConfigurationManager.AppSettings["passsword"];
            int PortNumber = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["PortNumber"]);
            string HostName = System.Configuration.ConfigurationManager.AppSettings["HostName"];

            // Instantiate a new instance of MailMessage 
            MailMessage mailMessage = new MailMessage();

            // Set the sender address of the mail message 
            mailMessage.From = new MailAddress(email);

            // Set the recipient address of the mail message 
            // mailMessage.To.Add(new MailAddress(recipient));
            if (!string.IsNullOrEmpty(recipient))
            {
                string[] strRecipient = recipient.Replace(";", ",").TrimEnd(',').Split(new char[] { ',' });

                // Set the Bcc address of the mail message 
                foreach (string t in strRecipient)
                {
                    mailMessage.To.Add(new MailAddress(t));
                }
            }

            // Check if the bcc value is nothing or an empty string 
            if (!string.IsNullOrEmpty(bcc))
            {
                string[] strBcc = bcc.Split(new char[] { ',' });

                // Set the Bcc address of the mail message 
                foreach (string t in strBcc)
                {
                    mailMessage.Bcc.Add(new MailAddress(t));
                }
            }

            // Check if the cc value is nothing or an empty value 
            if (!string.IsNullOrEmpty(cc))
            {
                // Set the CC address of the mail message 
                string[] strCc = cc.Split(',');
                foreach (string t in strCc)
                {
                    mailMessage.CC.Add(new MailAddress(t));
                }
            }

            // Set the subject of the mail message 
            mailMessage.Subject = subject;

            // Set the body of the mail message 
            mailMessage.Body = body;

            // Set the format of the mail message body as HTML 
            mailMessage.IsBodyHtml = true;

            // Set the priority of the mail message to normal 
            mailMessage.Priority = MailPriority.Normal;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                   | SecurityProtocolType.Tls11
                                   | SecurityProtocolType.Tls12;

            // Instantiate a new instance of SmtpClient 
            SmtpClient smtpClient = new SmtpClient();

            //  mailMessage.Attachments.Add(new Attachment(attachment));
            try
            {
                smtpClient.EnableSsl = true;
                smtpClient.Host = HostName;
                smtpClient.Port = PortNumber;
                smtpClient.Credentials = new System.Net.NetworkCredential(email, password);

                // Send the mail message 
                smtpClient.Send(mailMessage);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
            finally
            {
                Security.DisposeOf(mailMessage);
                Security.DisposeOf(smtpClient);
            }
        }

        public static bool SendAsyncEmail(string recipient, string bcc, string cc, string subject, string body, string attachment)
        {
            Task task = new Task(() => SendMailMessage(recipient, bcc, cc, subject, body, attachment));
            task.Start();
            return true;
        }

        /// <summary>
        /// Class EmailSetting.
        /// </summary>
        public class EmailSetting
        {
            /// <summary>
            /// Gets or sets from email.
            /// </summary>
            /// <value>From email.</value>
            public string FromEmail { get; set; }

            /// <summary>
            /// Gets or sets the name of the email host.
            /// </summary>
            /// <value>The name of the email host.</value>
            public string EmailHostName { get; set; }

            /// <summary>
            /// Gets or sets the email port.
            /// </summary>
            /// <value>The email port.</value>
            public int EmailPort { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether [email enable SSL].
            /// </summary>
            /// <value><c>true</c> if [email enable SSL]; otherwise, <c>false</c>.</value>
            public bool EmailEnableSsl { get; set; }

            /// <summary>
            /// Gets or sets the email user-name.
            /// </summary>
            /// <value>The email user name.</value>
            public string EmailUsername { get; set; }

            /// <summary>
            /// Gets or sets the email password.
            /// </summary>
            /// <value>The email password.</value>
            public string EmailPassword { get; set; }

            /// <summary>
            /// Gets or sets from name.
            /// </summary>
            /// <value>From Name</value>
            public string FromName { get; set; }
        }
    }
}
