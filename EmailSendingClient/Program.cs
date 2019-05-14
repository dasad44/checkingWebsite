using EmailSendingClient.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace EmailSendingClient
{
    public class Program
    {
        static EmailSendClient client = new EmailSendClient();

        public static void Main(string[] args)
        {
            Timer timer = new Timer(60000);
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.AutoReset = true;
            timer.Enabled = true;
            Console.WriteLine("Press <Enter> to terminate...");
            Console.ReadLine();
            client.Close();
        }

        public static void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            if (DateTime.Now.Minute == 00)
            {
                if (DateTime.Now.Hour == 9 || DateTime.Now.Hour == 12)
                {
                    try
                    {
                        client.SendEmail("wcfemailsender@gmail.com", "P@ssw0rd_", "I'm alive", "Usługa jest aktywna", "pawel.lukasiak@coloursfactory.pl");
                        Console.WriteLine("Done!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: {0}", ex.Message);
                        Console.ReadLine();
                    }
                }
            }
            else
                Console.WriteLine("{0}", DateTime.Now);
        }
    }
}
