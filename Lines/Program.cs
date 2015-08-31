using System;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace Lines {
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            try {
                using (var game = new LinesGame(false)) {
                    game.Run();
                }
            } catch (Exception e) {
                string hash = GetHash(e.Message);

                MailMessage mail = new MailMessage("linesbugs@gmail.com", "linesbugs@gmail.com");
                mail.Subject = "Error report.";
                mail.Body = "Hash: " + hash + "\n\n" +
                    "Message: " + e.Message + "\n\n" +
                    "StackTrace: " + e.StackTrace;

                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                // Google does't allow easy pass. Don't hack me please
                client.Credentials = new NetworkCredential("linesbugs@gmail.com", "57b4zdkn");
                client.Send(mail);

                MessageBox.Show("Your hash is " + hash + ". We automatically sent error report to support.\n\n" +
                    "If you want know more - contact me: yegorf1@gmail.com", "Error report");
            }
        }

        static string GetHash(string from) {
            byte[] encodedPassword = new UTF8Encoding().GetBytes(from);
            byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedPassword);

            return BitConverter.ToString(hash).Replace("-", string.Empty).Substring(0, 6).ToLower();
        }
    }
#endif
}
