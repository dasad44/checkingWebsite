using EmailSendingClient.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace EmailSendingClient
{
    public class Program
    {
        static EmailSendClient client = new EmailSendClient();
        static int sent = 0;
        //static string address = "192.168.11.150";
        //static int port = 800;

        public static void Main(string[] args)
        {
            Timer timer = new Timer(300000);
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent); //Wywołuje event po upływie czasu z timera
            timer.AutoReset = true;
            timer.Enabled = true;
            /*try
            {
                Console.WriteLine("Estabilishing connection to {0}:{1}", address, port);
                client.Connect(address, port);
                Console.WriteLine("Connection estabilished.");
            }
            catch (TypeInitializationException e)
            {
                Console.WriteLine("Error: {0}", e.Message);
            }*/
            Console.WriteLine("Press <Enter> to terminate...");
            Console.ReadLine();
            client.Close();
        }

        public static void OnTimedEvent(object sender, ElapsedEventArgs e)
        {

            using (var wClient = new WebClient())
            {
                try
                {              
                    string url = wClient.DownloadString("http://192.168.11.150:800/index.php"); //Łączy się ze stroną internetową
                    Console.WriteLine("No jest Git");
                    sent = 0;
                    //Console.WriteLine("Done!");
                }
                catch (Exception ex)
                {
                    if (ex is WebException || ex is TimeoutException)
                    {
                        if (sent < 1)
                            client.SendEmail("wcfemailsender@gmail.com", "P@ssw0rd_", "Błąd", "Strona 192.168.11.150:800 nie działa poprawnie!", "mateusz.wnuk06@gmail.com"); //Wysyła emaila
                        sent++;
                        Console.WriteLine("Error! " + ex.Message);
                        //Console.WriteLine("Error: {0}", ex.Message);
                        Console.ReadLine();
                    }
                    else
                        Console.WriteLine("Nieznany błąd");
                }
            }
        }
    }
}
