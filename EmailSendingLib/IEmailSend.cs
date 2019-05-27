using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace EmailSendingLib
{
    [ServiceContract]
    public interface IEmailSend
    {
        [OperationContract]
        void SendEmail(string email, string emailPassword, string subject, string content, string targetEmail);
    }
}
