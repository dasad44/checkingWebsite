using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace EmailSendingLib
{
    // UWAGA: możesz użyć polecenia „Zmień nazwę” w menu „Refaktoryzuj”, aby zmienić nazwę interfejsu „IService1” w kodzie i pliku konfiguracji.
    [ServiceContract]
    public interface IEmailSend
    {
        [OperationContract]
        void SendEmail(string email, string emailPassword, string subject, string content, string targetEmail);
        //[OperationContract]
        //void Connect(string address, int port);
    }
}
