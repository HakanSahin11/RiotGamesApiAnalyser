using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace lol_decay_api.Helper_Classes
{
    public class MailService
    {
        private IUserClass _User;
        public MailService(IUserClass User)
        {
            _User = User;
        }
        public bool MailSender(EmailClass ec)
        {
            bool mailSent = true;
            try
            {
                MailMessage mm = new MailMessage();
                mm.From = new MailAddress("loldecayreminder@gmail.com");
                mm.To.Add(ec.To);
                mm.Subject = ec.Subject;
                mm.Body = ec.Body;
                mm.IsBodyHtml = false;
                SmtpClient smtp = new SmtpClient("smtp.gmail.com")
                {
                    UseDefaultCredentials = false,
                    Port = 587,
                    EnableSsl = true,
                    Credentials = new System.Net.NetworkCredential(_User.UserName, _User.Pass)
                };
                smtp.Send(mm);
            }
            catch
            {
                mailSent = false;
            }
            return mailSent;
        }

        



    }
}
