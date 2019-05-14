using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace EmailSendingLib
{
    public class EmailSendService : IEmailSend
    {
        public void SendEmail(string email, string emailPassword, string subject, string content, string targetEmail)
        {
            string host = "smtp.gmail.com";
            int port = 587;
            SmtpClient client = new SmtpClient(host, port);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false; 
            client.Credentials = new NetworkCredential(email, emailPassword);
            MailAddress from = new MailAddress(email, "WCF SERVICE", Encoding.UTF8);
            MailAddress to = new MailAddress(targetEmail);
            MailMessage mail = new MailMessage(from, to)
            {
                Body = content,
                Subject = subject
            };
           client.Send(mail);
        }

        /*public void Connect(string address, int port)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(address, port);
        }*/
    }
}
