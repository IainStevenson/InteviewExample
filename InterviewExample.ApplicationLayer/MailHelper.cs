using System.Net.Mail;

namespace InterviewExample.ApplicationLayer
{
    public static class MailHelper
    {
        public static void Send(string address, string title, string body)
        {
            MailMessage mail = new MailMessage("you@yourcompany.com", address);
            SmtpClient client = new SmtpClient();
            mail.Subject = title;
            mail.Body = body;
            client.Send(mail);
        }
    }
}
