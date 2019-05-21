using EmailSendingClient.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace EmailSendingClient
{
    public class Program
    {
        static EmailSendClient client = new EmailSendClient();
        static int sent = 0; //Zmienna licząca liczbę wysłanych maili błędu strony
        //static string address = "192.168.11.150";
        //static int port = 800;

        public static void Main(string[] args)
        {
            Timer timer = new Timer(300000); //Timer odpowiedzialny za sprawdzanie połączenia z 192.168.11.150:800 i ewentualne poinformowanie o błędzie
            Timer mailTimer = new Timer(60000); //Timer odpowiedzialny za wysłanie maili o prawidłowym działaniu usługi

            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent); //Wywołuje event po upływie czasu z timera
            timer.AutoReset = true;
            timer.Enabled = true;

            mailTimer.Elapsed += new ElapsedEventHandler(SendControlEmail);
            mailTimer.AutoReset = true;
            mailTimer.Enabled = true;
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
            Console.ReadKey();
        }

        //pawel.lukasiak@coloursfactory.pl
        public static void OnTimedEvent(object sender, ElapsedEventArgs e)
        {

            using (var wClient = new WebClient())
            {
                try
                {
                    string url = wClient.DownloadString("http://192.168.11.150:800/index.php"); //Łączy się ze stroną internetową
                    Console.WriteLine("Połączono ze stroną!");
                    sent = 0;
                }
                catch (Exception ex)
                {
                    if (ex is WebException || ex is TimeoutException)
                    {
                        if (sent < 1) //Sprawdza czy mail został już wysłany/ pojedyncze wyslanie
                            client.SendEmail("wcfemailsender@gmail.com", "P@ssw0rd_", "Błąd", "Strona 192.168.11.150:800 nie działa poprawnie!", "pawel.lukasiak@coloursfactory.pl"); //Wysyła emaila
                        sent++;
                        Console.WriteLine("Error! " + ex.Message);
                        Console.ReadLine();
                    }
                    else
                        Console.WriteLine("Nieznany błąd");
                }
            }
        }

        //pawel.lukasiak@coloursfactory.pl
        public static void SendControlEmail(object sender, ElapsedEventArgs e)
        {
            if (DateTime.Now.Minute == 00) //Sprawdza czy godzina jest pełna
            {
                if (DateTime.Now.Hour == 9 || DateTime.Now.Hour == 12)
                {
                    try
                    {
                        client.SendEmail("wcfemailsender@gmail.com", "P@ssw0rd_", "Stan usługi", "Usługa działa!", "pawel.lukasiak@coloursfactory.pl");
                        Console.WriteLine("Email o stanie usługi został wysłany!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: {0}", ex.Message);
                    }
                }
            }
        }
    }
}
