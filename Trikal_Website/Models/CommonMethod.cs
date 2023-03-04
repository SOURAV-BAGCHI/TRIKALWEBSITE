using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace Trikal_Website.Models
{
    public class CommonMethod
    {
        public static bool SendMail(String Mailbody, String EmailAddresses, String Subject, String MailAttachmentFilename, String TempImageFileName)
        {
            Boolean success = false;
            try
            {
                SmtpSection smtpSection = ConfigurationManager.GetSection("system.net/mailSettings/smtp") as SmtpSection;
                String From = smtpSection.From;
                String Host = smtpSection.Network.Host;
                Int32 Port = smtpSection.Network.Port;
                String UserName = smtpSection.Network.UserName;
                String Password = smtpSection.Network.Password;
                Boolean SSL = smtpSection.Network.EnableSsl;

                MailMessage mail = new MailMessage();
                List<String> emailids = EmailAddresses.Trim().Split(',').ToList();
                foreach (String str in emailids)
                {
                    mail.To.Add(str.Trim());
                }
                mail.From = new MailAddress(From);
                var smtp = new SmtpClient(Host, Port);
                smtp.EnableSsl = SSL;
                smtp.Credentials = new System.Net.NetworkCredential(UserName, Password);

                mail.Subject = Subject;
                mail.Body = Mailbody;

                var myAttach = new MemoryStream(File.ReadAllBytes(MailAttachmentFilename));

                mail.Attachments.Add(new Attachment(myAttach, TempImageFileName));

                mail.IsBodyHtml = true;
                smtp.SendCompleted += (s, e) => {

                    mail.Dispose();

                };

                smtp.Send(mail);
                success = true;
            }
            catch (Exception ex)
            {

            }
            return success;
        }

        public static String ReadHtmlFile(String htmlFilePath)
        {
            StringBuilder store = new StringBuilder();

            try
            {
                using (StreamReader htmlReader = new StreamReader(htmlFilePath))
                {
                    String line;
                    while ((line = htmlReader.ReadLine()) != null)
                    {
                        store.Append(line);
                    }
                }
            }
            catch (Exception ex) { }

            return store.ToString();
        }
    }

    
}