//
// Author    : Vinayak Ushakola
// Date      : 29 June 2020
// Purpose   : Recieve data from the Queue
//

using System;
using System.Net;
using System.Net.Mail;

namespace ChatAppMsmq
{
    public class SmtpMailSender
    {
        /// <summary>
        /// Send Mail
        /// </summary>
        /// <param name="token">Token</param>
        /// <param name="email">Email</param>
        /// <returns>If Mail Sent Successfully return true else false</returns>
        public static bool SendMail(string token, string email)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(token))
                {

                    string FROMNAME = "Vinayak Ushakola", FROM = "vinayak.mailtesting@gmail.com", TO = email, SUBJECT = "Reset Password";
                    int PORT = 587;
                    string message = "\nCopy this token & paste in your Postman or Swagger: \n" + token;
                    var BODY = "Hi," + message;

                    MailMessage mailMessage = new MailMessage();
                    SmtpClient client = new SmtpClient("smtp.gmail.com", PORT);
                    mailMessage.From = new MailAddress(FROM, FROMNAME);
                    mailMessage.To.Add(new MailAddress(TO));
                    mailMessage.Subject = SUBJECT;
                    mailMessage.Body = BODY;

                    client.Credentials = new NetworkCredential(FROM, "@bcd.1234");
                    client.EnableSsl = true;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.Send(mailMessage);

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}