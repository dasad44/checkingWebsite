using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using WindowsService.ServiceReference1;

namespace WindowsService
{
    public class EmailSender
    {
        static EmailSendClient client = new EmailSendClient();
        static bool ifSent; //Zmienna licząca liczbę wysłanych maili błędu strony

        public static void Timers()
        {
            Timer timer = new Timer(300000); //Timer odpowiedzialny za sprawdzanie połączenia z 192.168.11.150:800 i ewentualne poinformowanie o błędzie
            Timer mailTimer = new Timer(60010); //Timer odpowiedzialny za wysłanie maili o prawidłowym działaniu usługi

            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent); //Wywołuje event po upływie czasu z timera
            timer.AutoReset = true;
            timer.Enabled = true;

            mailTimer.Elapsed += new ElapsedEventHandler(SendControlEmail);
            mailTimer.AutoReset = true;
            mailTimer.Enabled = true;
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
                    ifSent = true;
                }
                catch (Exception ex)
                {
                    if (ex is WebException || ex is TimeoutException)
                    {
                        if (ifSent == true) //Sprawdza czy mail został już wysłany/ pojedyncze wyslanie
                            client.SendEmail("wcfemailsender@gmail.com", "P@ssw0rd_", "Błąd", "Strona 192.168.11.150:800 nie działa poprawnie!", "mateusz.wnuk06@gmail.com"); //Wysyła emaila
                        ifSent = false;
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
                        client.SendEmail("wcfemailsender@gmail.com", "P@ssw0rd_", "Stan usługi", "Usługa działa!", "mateusz.wnuk06@gmail.com");
                        Console.WriteLine("Email o stanie usługi został wysłany!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: {0}", ex.Message);
                    }
                }
            }
        }

        public static void Close()
        {
            client.Close();
        }
    }
}
